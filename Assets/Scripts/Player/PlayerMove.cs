using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 5;
    [SerializeField]
    float _rotSpeed = 10;
    Vector3 _moveDir;
    //-----------------------------------
    Rigidbody _rigidbody;
    [Space, SerializeField]
    float _fastMove = 10;
    //-----------------------------------
    PlayerAnimation _playerAnimation;
    //-----------------------------------
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerAnimation = GetComponent<PlayerAnimation>();

    }
    //-----------------------------------
    void FixedUpdate() { Move(); }
    void Move()
    {
        _moveDir.x = Input.GetAxis("Horizontal");
        _moveDir.z = Input.GetAxis("Vertical");
        if (_moveDir != Vector3.zero)
        {
            _playerAnimation.MoveAni(true);
            _rigidbody.position += Time.deltaTime * _moveSpeed * _moveDir.normalized;
            _rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDir), Time.deltaTime * _rotSpeed);
        }
        else
            _playerAnimation.MoveAni(false);
    }
    //-----------------------------------
    void Update()
    {
        //LeftAlt 입력시
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            // 빠른 이동
            _rigidbody.AddForce(transform.forward * _fastMove, ForceMode.Impulse);
    }
    //-----------------------------------
    public void LookAtTable(Transform table) { transform.LookAt(table.position); }
}
