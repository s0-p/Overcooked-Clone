using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSpawnTable : BasicTable
{
    [SerializeField]
    GameObject _platePrefab;
    [SerializeField]
    int _plateCount;
    Vector3 _spawnPosition;
    //-----------------------------------------------------------------
    List<GameObject> _plates = new List<GameObject>();
    //-----------------------------------------------------------------
    protected override void Awake() 
    {
        _spawnPosition = transform.position + 0.5f * Vector3.up;
        SpawnPlates(); 
    }
    public override void PutOnObject(Transform objectTransform) { }
    void SpawnPlates()
    {
        for (int count = 1; count <= _plateCount; count++)
        {
            GameObject plate = Instantiate(_platePrefab, _spawnPosition, Quaternion.identity);
            plate.GetComponent<PlateCtrl>().SpawnPosition = _spawnPosition;
            _plates.Add(plate);
        }
    }
}
