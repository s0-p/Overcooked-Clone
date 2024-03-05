using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCtrl : Pickupable
{
    [SerializeField]
    float _spawnDeley = 3;
    //--------------------------------------------------------------
    public Vector3 SpawnPosition { get; set; }
    //--------------------------------------------------------------
    int _includedIngredientsBit;
    public int IncludedIngredientsBit { get { return _includedIngredientsBit;} }
    //--------------------------------------------------------------
    private void OnEnable()
    {
        _includedIngredientsBit = 0;
    }
    void OnDisable()
    {
        Invoke("Spawn", _spawnDeley);
    }
    public void OnDisableCustom()
    {
        for (int index = 0; index < transform.childCount; index++)
        {
            IngredientCtrl childCtrl = transform.GetChild(index).GetComponent<IngredientCtrl>();
            childCtrl.ObjectPool.Return(childCtrl.gameObject);
        }
        gameObject.SetActive(false);
    }
    void Spawn()
    {
        transform.position = SpawnPosition;
        gameObject.SetActive(true);
    }
    //--------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        IngredientCtrl ingredientCtrl = collision.transform.GetComponent<IngredientCtrl>();
        if (ingredientCtrl?._cookerys.Count <= 0)
        {
            _includedIngredientsBit += ingredientCtrl.Id;

            ingredientCtrl.transform.parent = transform;
            ingredientCtrl.transform.SetPositionAndRotation(
                        transform.position + Vector3.up * 0.1f,
                        Quaternion.identity
                    );

            ingredientCtrl.Freeze(true);
            ingredientCtrl.gameObject.layer = 0;
        }
    }
}
