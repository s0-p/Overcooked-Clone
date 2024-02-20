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
    //  ���̺�, ���� ����
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
            _detectedTable = other.GetComponentInParent<TableBase>();
            _detectedTable.GetComponentInChildren<DetectedCtrl>().Enter();
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
                _detectedTable.GetComponentInChildren<DetectedCtrl>().Exit();
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
        //  Leftctrl �Է� 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //  �տ� ������Ʈ�� ������ ������
            if (_pickupTransform.childCount > 0)
                Throw();

            //  ��� && ������ ���̺��� ������ ���̺� ��� ����
            else if (_detectedTable != null)
                _detectedTable.Operate();
        }

        //Space �Է� -> ����/����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  �� �� -> ����
            if (_pickupTransform.childCount <= 0)
            {
                if (_detectedFood != null)
                {
                    Rigidbody foodRigidbody = _detectedFood.GetComponent<Rigidbody>();
                    foodRigidbody.isKinematic = true;
                    
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

        _detectedFood.GetComponent<Collider>().enabled = false;

        _playerMove.enabled = true;
    }
    public void PutDown()
    {
        _playerAnimation.PutDownAni();

        Transform foodTransform = _pickupTransform.GetChild(0);
        foodTransform.parent = null;

        Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
        foodRigidbody.isKinematic = false;

        foodTransform.GetComponent<Collider>().enabled = true;
    }
    //-----------------------------------
    //  ���� ������
    void Throw()
    {
        _playerAnimation.PutDownAni();
        
        _pickupTransform.GetComponentInChildren<Collider>().enabled = true;

        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.isKinematic = false;
        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
}
