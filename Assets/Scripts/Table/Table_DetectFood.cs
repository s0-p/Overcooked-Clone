using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table_DetectFood : MonoBehaviour
{
    TableBase _tableBase;
    void Awake()
    {
        _tableBase = GetComponentInParent<TableBase>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            _tableBase.PutOnObject(other.transform);
        }
    }
}
