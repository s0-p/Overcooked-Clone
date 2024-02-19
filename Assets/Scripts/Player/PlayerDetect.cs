using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [SerializeField]
    float _throwPower = 30;
    //-----------------------------------
    [Space, SerializeField]
    Transform _pickupTransform;
    TableBase _detectedTable;
    //-----------------------------------
    Transform _detectedFood;
    //-----------------------------------
    PlayerMove _playerMove;
    PlayerAnimation _playerAnimation;
    //-----------------------------------
    void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }
    //-----------------------------------
    //  테이블, 음식 감지
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
            DetectTable(other);

        if (other.CompareTag("Food"))
            DetectFood(other);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Table"))
            DetectTable(other);

        if (other.CompareTag("Food"))
            DetectFood(other);
    }
    void DetectTable(Collider other)
    {
        if (_detectedTable == null)
        {
            _detectedTable = other.GetComponent<TableBase>();
            _detectedTable.GetComponent<DetectedCtrl>().Enter();
        }
    }
    void DetectFood(Collider other)
    {
        if (_detectedFood == null)
        {
            _detectedFood = other.transform;
            _detectedFood.GetComponent<DetectedCtrl>().Enter();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (_detectedTable != null)
            {
                _detectedTable.GetComponent<DetectedCtrl>().Exit();
                _detectedTable = null;
            }
        }
        if (other.CompareTag("Food"))
        {
            if (_detectedFood != null)
            {
                _detectedFood.GetComponent<DetectedCtrl>().Exit();
                _detectedFood = null;
            }
        }
    }
    //-----------------------------------
    void Update()
    {
        //  Leftctrl 입력 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //  손에 오브젝트가 있으면 던지기
            if (_pickupTransform.childCount > 0)
                Throw();

            //  빈손 && 감지된 테이블이 있으면 테이블 기능 실행
            else if(_detectedTable != null)
                _detectedTable.Operate();
        }

        //Space 입력 -> 집기/놓기
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  빈 손
            if (_pickupTransform.childCount <= 0)
            {
                //  감지된 오브젝트 > 감지된 테이블 위 오브젝트

                //  감지된 오브젝트 집기
                if (_detectedFood != null)
                {
                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }

                //테이블 위 오브젝트를 가져올 경우
                else if (_detectedTable != null &&
                        _detectedTable.InObject != null)
                {
                    _detectedTable.InObject = null;
                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }
            }
            //  빈 손X
            else
                _playerAnimation.PutDownAni();
        }
    }
    //-----------------------------------
    //  물건 집기
    public void PickUp()
    {
        _detectedFood.position = _pickupTransform.position;
        _detectedFood.parent = _pickupTransform;

        Rigidbody foodRigidbody = _detectedFood.GetComponent<Rigidbody>();
        foodRigidbody.useGravity = false;
        foodRigidbody.isKinematic = true;

        _detectedFood.GetComponent<Collider>().isTrigger = true;
        _playerMove.enabled = true;
    }
    public void PutDown()
    {
        Transform foodTransform = _pickupTransform.GetChild(0);
        //감지된 테이블이 있고 놓인 오브젝트가 없을경우
        if (_detectedTable != null && _detectedTable.InObject == null)
        {
            //테이블 위에 놓기
            _detectedTable.PutOnObject(foodTransform);
        }

        /*  problem
            음식을 던져서 안착되는 경우가 있으니
            그냥 떨어뜨렸는데 테이블이 인식해서
            그 위에 놓아지는걸로 수정 필요
        */

        //인식된 테이블이 없을 경우
        else
        {
            //내려놓기
            Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
            foodRigidbody.useGravity = true;
            foodRigidbody.isKinematic = false;

            foodTransform.parent = null;
        }
            foodTransform.GetComponent<Collider>().isTrigger = false;
    }
    //-----------------------------------
    //  물건 던지기
    void Throw()
    {
        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.useGravity = true;
        itemRigidbody.isKinematic = false;
        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
}
