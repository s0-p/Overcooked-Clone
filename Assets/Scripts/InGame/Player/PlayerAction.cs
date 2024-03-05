using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    float _throwPower = 30;
    //-----------------------------------
    [Space, SerializeField]
    Transform _pickupTransform;
    Pickupable _detectedPickupable;
    //-----------------------------------
    BasicTable _detectedTable;
    //-----------------------------------
    [SerializeField]
    GameObject _knife;
    //-----------------------------------
    PlayerMove _playerMove;
    PlayerAnimation _playerAnimation;
    //-----------------------------------
    void Awake()
    {
        _knife.SetActive(false);

        _playerMove = GetComponent<PlayerMove>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }
    //-----------------------------------
    public void Selected(bool isOn)
    {
        _playerMove.enabled = isOn;
        _playerAnimation.MoveAni(false);
        enabled = isOn;
    }
    //-----------------------------------
    //  테이블, 음식 감지
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Table"))
            DetectTable(other);

        else if (other.gameObject.layer == LayerMask.NameToLayer("Plate") || 
                other.gameObject.layer == LayerMask.NameToLayer("Food"))
            DetectFood(other);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Table"))
            DetectTable(other);

        else if (other.gameObject.layer == LayerMask.NameToLayer("Plate") ||
                other.gameObject.layer == LayerMask.NameToLayer("Food"))
            DetectFood(other);
    }
    void DetectTable(Collider other)
    {
        if (_detectedTable == null)
        {
            _detectedTable = other.GetComponentInParent<BasicTable>();
            _detectedTable.GetComponentInChildren<DetectedCtrl>().Enter();
        }
    }
    void DetectFood(Collider other)
    {
        if (_detectedPickupable == null || _detectedPickupable.gameObject.layer == 0)
        {
            Transform transform = other.transform;
            if (transform.parent != null)
                transform = transform.parent;

            _detectedPickupable = transform.GetComponent<Pickupable>();
            _detectedPickupable.GetComponentInChildren<DetectedCtrl>().Enter();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Table"))
        {
            if (_detectedTable != null)
            {
                _detectedTable.GetComponentInChildren<DetectedCtrl>().Exit();
                _detectedTable = null;
            }
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Plate") ||
                other.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            if (_pickupTransform.childCount <= 0)
            {
                other.GetComponent<DetectedCtrl>().Exit();
                _detectedPickupable = null;
            }
        }
    }
    //-----------------------------------
    void Update()
    {
        if (_detectedPickupable?.gameObject.activeSelf == false)
            _detectedPickupable = null;

        //  Leftctrl Down && 빈 손 && 감지된 테이블이 있으면 테이블 기능 실행
        if (Input.GetKeyDown(KeyCode.LeftControl) &&
            _pickupTransform.childCount <= 0)
        {
            _playerMove.LookAtTable(_detectedTable.transform);
            _detectedTable?.Operate(gameObject);
        }

        //  Leftctrl Up && 빈 손 x -> 던지기
        else if (Input.GetKeyUp(KeyCode.LeftControl) &&
            _pickupTransform.childCount > 0)
            Throw();

        //  Space 입력 -> 집기/놓기

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  빈 손X -> 놓기
            if(_pickupTransform.childCount > 0)
                PutDown();

            //  빈 손 && 감지된 음식이 있으면 -> 집기
            else if (_detectedPickupable != null)
            {
                _detectedPickupable.GetComponent<Rigidbody>().isKinematic = true;

                //  쓰레기통이 아닌 테이블 위 오브젝트를 가져올 경우
                if (_detectedTable?.OnObject != null)
                {
                    if (_detectedTable.CompareTag("Trash Table"))
                        return;

                    _detectedTable.OnObject = null;
                }

                _playerMove.enabled = false;
                _playerAnimation.PickUpAni();
            }
        }
    }
    //-----------------------------------
    //  물건 집기
    public void PickUp()
    {
        _detectedPickupable.transform.position = _pickupTransform.position;
        _detectedPickupable.transform.parent = _pickupTransform;

        _detectedPickupable.GetComponentInChildren<Collider>().enabled = false;
        _playerMove.enabled = true;
    }
    public void PutDown()
    {
        _playerAnimation.PutDownAni();

        _detectedPickupable.Freeze(false);
        _detectedPickupable.transform.parent = null;
    }
    //-----------------------------------
    //  물건 던지기
    void Throw()
    {
        _playerAnimation.PutDownAni();

        _detectedPickupable.Freeze(false);
        _detectedPickupable.GetComponent<Rigidbody>().AddForce(transform.forward * _throwPower);
        _detectedPickupable.transform.parent = null;
    }
    //-----------------------------------
    // 컷팅 온오프
    public void PauseCutting(bool isOn) 
    { 
        _knife.SetActive(isOn);
        if(!isOn) _detectedPickupable = null;
    }
}
