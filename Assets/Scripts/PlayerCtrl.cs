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
    Transform _detectedTableTransform;
    Transform _detectedFoodTransform;
    //-----------------------------------
    void FixedUpdate() { Move(); }
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
    //-----------------------------------
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            if (_detectedTableTransform != null)
                Exit(ref _detectedTableTransform);

            Detected(ref _detectedTableTransform, other.transform);
        }
        else if (other.CompareTag("Food"))
        {
            if (_detectedFoodTransform != null)
                Exit(ref _detectedFoodTransform);

            Detected(ref _detectedFoodTransform, other.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (_detectedTableTransform != null && other.CompareTag("Table"))
            Exit(ref _detectedTableTransform);
        else if (_detectedFoodTransform != null && other.CompareTag("Food"))
            Exit(ref _detectedFoodTransform);
    }
    void Detected(ref Transform originTransform, Transform newTransform)
    {
        originTransform = newTransform;
        originTransform.GetComponent<DetectedCtrl>().Enter();
    }
    void Exit(ref Transform originTransform)
    {
        originTransform.GetComponent<DetectedCtrl>().Exit();
        originTransform = null;
    }
    private void Update()
    {
        //leftctrl �Է��� �ְ� �տ� ������ ������ ������
        if (Input.GetKeyDown(KeyCode.LeftControl) && _pickupTransform.childCount > 0)
            Throw();

        //Space �Է� -> ����/����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //�� �� -> ������ ���� > ������ ���̺� �� ����
            if (_pickupTransform.childCount <= 0)
            {
                //������ ���� ����
                if (_detectedFoodTransform != null)
                {
                    _detectedFoodTransform.position = _pickupTransform.position;
                    _detectedFoodTransform.parent = _pickupTransform;

                    Rigidbody foodRigidbody = _detectedFoodTransform.GetComponent<Rigidbody>();
                    foodRigidbody.useGravity = false;
                    foodRigidbody.isKinematic = true;
                }
            }
            //�� ��X
            else
            {
                Transform foodTransform = _pickupTransform.GetChild(0);
                //������ ���̺��� ���� ���
                if (_detectedTableTransform != null && _detectedTableTransform.childCount <= 0)
                {
                    //���̺� ����
                    foodTransform.parent = _detectedTableTransform;
                    foodTransform.position = _detectedTableTransform.position + Vector3.up;
                }

                //�νĵ� ���̺��� ���� ���
                else
                {
                    //��������
                    Rigidbody foodRigidbody = foodTransform.GetComponent<Rigidbody>();
                    foodRigidbody.useGravity = true;
                    foodRigidbody.isKinematic = false;

                    foodTransform.parent = null;
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
