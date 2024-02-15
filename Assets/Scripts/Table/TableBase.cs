using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBase : MonoBehaviour
{
    public Transform InObject { get; set; }
    void Awake()
    {
        InObject = null;
    }
    public void PutOnObject(Transform objectTransform)
    {
        InObject = objectTransform;
        InObject.parent = transform;
        InObject.position = transform.position + Vector3.up;
    }
    public virtual void Operate() { }

}

