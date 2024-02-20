using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBase : MonoBehaviour
{
    public Transform OnObject { get; set; }
    //---------------------------------------------
    protected virtual void Awake() { OnObject = null; }
    //---------------------------------------------
    public virtual void PutOnObject(Transform objectTransform)
    {
        if (OnObject == null)
        {
            OnObject = objectTransform;
            OnObject.parent = transform;
            OnObject.position = transform.position + Vector3.up;

            OnObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    //---------------------------------------------
    public virtual void Operate() { }
    public virtual void Operate(GameObject player) { }
}

