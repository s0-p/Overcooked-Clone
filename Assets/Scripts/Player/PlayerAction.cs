using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
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
            else if (_detectedTable != null)
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
                        _detectedTable.OnObject != null)
                {
                    _detectedTable.OnObject = null;
                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }
            }
            //  빈 손X -> 놓기
            else
                PutDown();
        }
    }
    //-----------------------------------
    //  물건 집기
    public void PickUp()
    {
        _detectedFood.position = _pickupTransform.position;
        _detectedFood.parent = _pickupTransform;
        _detectedFood.GetComponent<Collider>().enabled = false;

        Rigidbody foodRigidbody = _detectedFood.GetComponent<Rigidbody>();
        foodRigidbody.useGravity = false;
        foodRigidbody.isKinematic = true;

        _playerMove.enabled = true;
    }
    public void PutDown()
    {
        _playerAnimation.PutDownAni();

        Transform foodTransform = _pickupTransform.GetChild(0);
        foodTransform.parent = null;

        Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
        foodRigidbody.useGravity = true;
        foodRigidbody.isKinematic = false;

        foodTransform.GetComponent<Collider>().enabled = true;
    }
    //-----------------------------------
    //  물건 던지기
    void Throw()
    {
        _playerAnimation.PutDownAni();
        
        _pickupTransform.GetComponentInChildren<Collider>().enabled = true;

        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.useGravity = true;
        itemRigidbody.isKinematic = false;
        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
}
