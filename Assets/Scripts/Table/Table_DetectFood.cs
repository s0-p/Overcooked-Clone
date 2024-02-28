using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table_DetectFood : MonoBehaviour
{
    BasicTable _tableBase;
    void Awake()
    {
        _tableBase = GetComponentInParent<BasicTable>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Plate") ||
            other.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            Transform transform = other.transform;
            if (transform.parent != null && transform.parent.gameObject.layer != LayerMask.NameToLayer("Table"))
                transform = transform.parent;
           
            _tableBase.PutOnObject(transform);
        }
    }
}
