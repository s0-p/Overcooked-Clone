using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    Transform _targetTransform;
    [SerializeField]
    Vector3 _offset;

    void LateUpdate()
    {
        transform.position = _targetTransform.position + _offset;
    }
}
