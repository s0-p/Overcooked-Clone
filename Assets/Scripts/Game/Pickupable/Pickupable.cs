using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected virtual void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }
    public void Freeze(bool isOn)
    {
        _rigidbody.isKinematic = isOn;
        GetComponentInChildren<Collider>().enabled = !isOn;
    }
}
