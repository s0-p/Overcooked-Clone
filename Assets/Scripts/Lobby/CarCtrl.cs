using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    GameObject _flag;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        enabled = false;
        FadeManager.Instance.StartFadeIn(() => enabled = true);
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _flag?.GetComponent<FlagCtrl>().LoadInGame();
    }
    //--------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            _flag = other.gameObject;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            _flag = null;
    }
}
