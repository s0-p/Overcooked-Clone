using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCtrl : MonoBehaviour
{
    [SerializeField]
    float _spawnDeley = 3;
    //--------------------------------------------------------------
    public Vector3 SpawnPosition { get; set; }
    //--------------------------------------------------------------
    void OnDisable() { Invoke("Spawn", _spawnDeley); }
    void Spawn()
    {
        transform.position = SpawnPosition;
        gameObject.SetActive(true);
    }
}
