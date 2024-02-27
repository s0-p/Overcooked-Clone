using System;
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
    Transform _detectedFood;
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
    //  ���̺�, ���� ����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
            DetectTable(other);

        else if (other.CompareTag("Plate") || 
                other.gameObject.layer == LayerMask.NameToLayer("Food"))
            DetectFood(other);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Table"))
            DetectTable(other);

        else if (other.CompareTag("Plate") ||
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
        if (_detectedFood == null)
        {
            _detectedFood = other.transform;
            if(_detectedFood.parent != null && !_detectedFood.parent.CompareTag("Table"))
                _detectedFood = _detectedFood.parent;

            _detectedFood.GetComponentInChildren<DetectedCtrl>().Enter();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (_detectedTable != null)
            {
                _detectedTable.GetComponentInChildren<DetectedCtrl>().Exit();
                _detectedTable = null;
            }
        }

        else if (other.CompareTag("Plate") ||
                other.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            if (_pickupTransform.childCount <= 0)
            {
                other.GetComponent<DetectedCtrl>().Exit();
                _detectedFood = null;
            }
        }
    }
    //-----------------------------------
    void Update()
    {
        //  Leftctrl �Է� 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //  �� �� x -> ������
            if (_pickupTransform.childCount > 0)
                Throw();

            //  �� �� && ������ ���̺��� ������ ���̺� ��� ����
            else if (_detectedTable != null)
            {
                _playerMove.LookAtTable(_detectedTable.transform);
                _detectedTable.Operate(gameObject);
            }
        }

        //Space �Է� -> ����/����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  �� �� -> ����
            if (_pickupTransform.childCount <= 0)
            {
                if (_detectedFood != null)
                {
                    _detectedFood.GetComponent<Rigidbody>().isKinematic = true;
                    
                    //���̺� �� ������Ʈ�� ������ ���
                    if (_detectedTable != null && _detectedTable.OnObject != null)
                        _detectedTable.OnObject = null;

                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }
            }
            //  �� ��X -> ����
            else
                PutDown();
        }
    }
    //-----------------------------------
    //  ���� ����
    public void PickUp()
    {
        _detectedFood.position = _pickupTransform.position;
        _detectedFood.parent = _pickupTransform;

        _detectedFood.GetComponentInChildren<Collider>().enabled = false;
        _playerMove.enabled = true;
    }
    public void PutDown()
    {
        _playerAnimation.PutDownAni();

        Transform foodTransform = _pickupTransform.GetChild(0);
        foodTransform.parent = null;

        Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
        foodRigidbody.isKinematic = false;

        _detectedFood.GetComponentInChildren<Collider>().enabled = true;

        _detectedFood = null;
    }
    //-----------------------------------
    //  ���� ������
    void Throw()
    {
        _playerAnimation.PutDownAni();

        _detectedFood.GetComponentInChildren<Collider>().enabled = true;

        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.isKinematic = false;

        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
    //-----------------------------------
    // ���� �¿���
    public void PauseCutting(bool isOn) 
    { 
        _knife.SetActive(isOn);
        if(!isOn)
            _detectedFood = null;
    }
}
