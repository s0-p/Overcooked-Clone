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
    eCOOKERY[] _cookerys;
    //---------------------------------------------------------------
    [SerializeField]
    int _maxCount;
    //---------------------------------------------------------------
    ObjectPool _objectPool;
    //---------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();
        _objectPool = GetComponent<ObjectPool>();
    }
    void Start() { _objectPool.Init(_maxCount, _ingredientPrefab); }
    public override void Operate(GameObject player)
    {
        GameObject ingredient = _objectPool.Get();
        ingredient.transform.position = transform.position + Vector3.up * 0.2f;

        IngredientCtrl ingredientCtrl = ingredient.GetComponent<IngredientCtrl>();

        ingredientCtrl.Id = (int)_type;
        ingredientCtrl.ObjectPool = _objectPool;
        ingredientCtrl._cookerys.AddRange(_cookerys);

        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerAnimation>().PickUpAni();
    }
}
