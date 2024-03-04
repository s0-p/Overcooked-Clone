using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCtrl : Pickupable
{
    [SerializeField]
    GameObject _origin;
    [SerializeField]
    GameObject _cooked;
    //--------------------------------------------------------------
    int _Id;
    public int Id { get { return _Id; } set { _Id = (int)Mathf.Pow(2, value); } }
    //--------------------------------------------------------------
    public ObjectPoolingManager PoolingManager { get; set; }
    //--------------------------------------------------------------
    public List<eCOOKERY> _cookerys = new List<eCOOKERY>();
    //--------------------------------------------------------------
    void OnEnable()
    {
        _origin.SetActive(true);
        _cooked.SetActive(false);

        _rigidbody.isKinematic = true;
        GetComponentInChildren<Collider>().enabled = true;
    }
    public void ChangeToCookedModel()
    {
        _cookerys.RemoveAt(0);

        _cooked.SetActive(true);
        _origin.SetActive(false);
    }
}
