using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 5;
    [SerializeField]
    float _rotSpeed = 10;
    Vector3 _moveDir;
    //-----------------------------------
    [SerializeField]
    float _throwPower = 30;
    //-----------------------------------
    [SerializeField]
    Transform _pickupTransform;
    Transform _detectedTableTransfom;
    //-----------------------------------
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        _moveDir.x = Input.GetAxis("Horizontal");
        _moveDir.z = Input.GetAxis("Vertical");

        if (_moveDir != Vector3.zero)
        {
            transform.position += Time.deltaTime * _moveSpeed * _moveDir.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDir), Time.deltaTime * _rotSpeed);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (_detectedTableTransfom != null)
                _detectedTableTransfom.GetComponent<Table>().Exit();

            _detectedTableTransfom = other.transform;
            _detectedTableTransfom.GetComponent<Table>().Enter();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (_detectedTableTransfom != null)
            {
                _detectedTableTransfom.GetComponent<Table>().Exit();
                _detectedTableTransfom = null;
            }
        }
    }
    private void Update()
    {
        //leftctrl �Է��� �ְ� �տ� ������ ������ ������
        if (Input.GetKeyDown(KeyCode.LeftControl) && _pickupTransform.childCount > 0)
            Throw();

        //Space �Է�
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //���̺��� �ν����� ���
            if (_detectedTableTransfom != null)
            {
                //�տ� ������ �ְ� ���̺��� ��� ������ ���� ����
                if (_pickupTransform.childCount > 0)
                {
                    Transform foodTransform = _pickupTransform.GetChild(0);
                    if (foodTransform != null)
                    {
                        foodTransform.parent = _detectedTableTransfom;
                        foodTransform.position = _detectedTableTransfom.position + Vector3.up;
                    }
                    else
                    {
                        foodTransform = _detectedTableTransfom.GetChild(0);
                        if (foodTransform != null)
                        {
                            foodTransform.parent = _pickupTransform;
                            foodTransform.position = _pickupTransform.position;
                        }
                    }
                }
                //���̺� ������ �ְ� ���� ��������� ���� ���
                else
                {
                    if (_pickupTransform.childCount <= 0)
                    {
                        Transform itemTransform = _detectedTableTransfom.GetChild(0);
                        itemTransform.position = _pickupTransform.position;
                        itemTransform.parent = _pickupTransform;
                    }
                }
            }
        }
    }
    void Throw()
    {
        Rigidbody itemRigidbody = _pickupTransform.GetComponentInChildren<Rigidbody>();
        itemRigidbody.useGravity = true;
        itemRigidbody.isKinematic = false;
        itemRigidbody.transform.parent = null;
        itemRigidbody.AddForce(transform.forward * _throwPower);
    }
}
