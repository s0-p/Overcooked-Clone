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
    //  ���̺�, ���� ����
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

        //  Leftctrl Down && �� �� && ������ ���̺��� ������ ���̺� ��� ����
        if (Input.GetKeyDown(KeyCode.LeftControl) &&
            _pickupTransform.childCount <= 0)
        {
            _playerMove.LookAtTable(_detectedTable.transform);
            _detectedTable?.Operate(gameObject);
        }

        //  Leftctrl Up && �� �� x -> ������
        else if (Input.GetKeyUp(KeyCode.LeftControl) &&
            _pickupTransform.childCount > 0)
            Throw();

        //  Space �Է� -> ����/����

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  �� ��X -> ����
            if(_pickupTransform.childCount > 0)
                PutDown();

            //  �� �� && ������ ������ ������ -> ����
            else if (_detectedPickupable != null)
            {
                _detectedPickupable.GetComponent<Rigidbody>().isKinematic = true;

                //  ���������� �ƴ� ���̺� �� ������Ʈ�� ������ ���
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
    //  ���� ����
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
    //  ���� ������
    void Throw()
    {
        _playerAnimation.PutDownAni();

        _detectedPickupable.Freeze(false);
        _detectedPickupable.GetComponent<Rigidbody>().AddForce(transform.forward * _throwPower);
        _detectedPickupable.transform.parent = null;
    }
    //-----------------------------------
    // ���� �¿���
    public void PauseCutting(bool isOn) 
    { 
        _knife.SetActive(isOn);
        if(!isOn) _detectedPickupable = null;
    }
}
