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
        //  Leftctrl �Է� 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //  �տ� ������Ʈ�� ������ ������
            if (_pickupTransform.childCount > 0)
                Throw();

            //  ��� && ������ ���̺��� ������ ���̺� ��� ����
            else if(_detectedTable != null)
                _detectedTable.Operate();
        }

        //Space �Է� -> ����/����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  �� ��
            if (_pickupTransform.childCount <= 0)
            {
                //  ������ ������Ʈ > ������ ���̺� �� ������Ʈ

                //  ������ ������Ʈ ����
                if (_detectedFood != null)
                {
                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }

                //���̺� �� ������Ʈ�� ������ ���
                else if (_detectedTable != null &&
                        _detectedTable.InObject != null)
                {
                    _detectedTable.InObject = null;
                    _playerMove.enabled = false;
                    _playerAnimation.PickUpAni();
                }
            }
            //  �� ��X
            else
                _playerAnimation.PutDownAni();
        }
    }
    //-----------------------------------
    //  ���� ����
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
        //������ ���̺��� �ְ� ���� ������Ʈ�� �������
        if (_detectedTable != null && _detectedTable.InObject == null)
        {
            //���̺� ���� ����
            _detectedTable.PutOnObject(foodTransform);
        }

        /*  problem
            ������ ������ �����Ǵ� ��찡 ������
            �׳� ����߷ȴµ� ���̺��� �ν��ؼ�
            �� ���� �������°ɷ� ���� �ʿ�
        */

        //�νĵ� ���̺��� ���� ���
        else
        {
            //��������
            Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
            foodRigidbody.useGravity = true;
            foodRigidbody.isKinematic = false;

            foodTransform.parent = null;
        }
            foodTransform.GetComponent<Collider>().isTrigger = false;
    }
    //-----------------------------------
    //  ���� ������
    void Throw()
    {
        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.useGravity = true;
        itemRigidbody.isKinematic = false;
        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
}
