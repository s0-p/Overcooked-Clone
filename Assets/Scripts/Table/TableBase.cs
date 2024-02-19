using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBase : MonoBehaviour
{
    public Transform OnObject { get; set; }
    //---------------------------------------------
    void Awake()
    {
        OnObject = null;
    }
    //---------------------------------------------
    public void PutOnObject(Transform objectTransform)
    {
        OnObject = objectTransform;
        OnObject.parent = transform;
        OnObject.position = transform.position + Vector3.up;

        OnObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    //---------------------------------------------
    public virtual void Operate() { }
}

