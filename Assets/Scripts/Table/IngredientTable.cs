using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : BasicTable
{
    [SerializeField]
    GameObject _ingredientPrefab;
    [SerializeField]
    eINGREDIENT _type;
    //---------------------------------------------------------------
    [SerializeField]
    int _maxCount;
    //---------------------------------------------------------------
    ObjectPoolingManager _poolingManager;
    //---------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();
        _poolingManager = GetComponent<ObjectPoolingManager>();
    }
    void Start() { _poolingManager.Init(_maxCount, _ingredientPrefab); }
    public override void Operate(GameObject player)
    {
        GameObject ingredient = _poolingManager.Get();
        ingredient.transform.position = transform.position + Vector3.up * 0.1f;

        IngredientCtrl ingredientCtrl = ingredient.GetComponent<IngredientCtrl>();
        ingredientCtrl.BitId = (int)_type;
        ingredientCtrl.PoolingManager = _poolingManager;

        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerAnimation>().PickUpAni();
    }
}
