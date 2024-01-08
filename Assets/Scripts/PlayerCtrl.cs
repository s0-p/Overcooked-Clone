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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            _detectedTableTransfom = other.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            _detectedTableTransfom = null;
        }
    }
    private void Update()
    {
        //Space 입력이 있고 테이블을 인식했을 경우
        if (Input.GetKeyDown(KeyCode.Space) && _detectedTableTransfom != null)
        {
            //손에 물건이 있고 테이블이 비어 있으면 물건 놓기
            if (_pickupTransform.childCount > 0)
            {
                Transform foodTransform = _pickupTransform.GetChild(0);
                if(foodTransform != null) 
                {
                    foodTransform.parent = _detectedTableTransfom;
                    foodTransform.position = _detectedTableTransfom.position + Vector3.up;
                }
                else
                {
                    foodTransform = _detectedTableTransfom.GetChild(0);
                    if(foodTransform != null ) 
                    {
                        foodTransform.parent = _pickupTransform;
                        foodTransform.position = _pickupTransform.position;
                    }
                }
            }
            //테이블에 물건이 있고 손이 비어있으면 물건 들기
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
