using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCtrl : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 5;
    [SerializeField]
    float _rotSpeed = 10;
    Vector3 _moveDir;
    //--------------------------------------------------------------------------
    Rigidbody _rigidbody;
    //--------------------------------------------------------------------------
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _moveDir.x = Input.GetAxis("Horizontal");
        _moveDir.z = Input.GetAxis("Vertical");
        if (_moveDir != Vector3.zero)
        {
            _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * _moveSpeed * _moveDir);
            _rigidbody.MoveRotation(
                Quaternion.Slerp(_rigidbody.rotation,
                                Quaternion.LookRotation(_moveDir),
                                Time.deltaTime * _rotSpeed)
                );
        }
    }
}
