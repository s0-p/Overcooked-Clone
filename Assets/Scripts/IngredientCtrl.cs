using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject _origin;
    [SerializeField]
    GameObject _cooked;
    //--------------------------------------------------------------
    int _bitId;
    public int BitId { get { return _bitId; } set { _bitId = (int)Mathf.Pow(2, value); } }
    public bool IsCooked { get; set; }
    //--------------------------------------------------------------
    void Awake()
    {
        _origin.SetActive(true);
        _cooked.SetActive(false);
    }
    public void ChangeToCookedModel()
    {
        IsCooked = true;
        _cooked.SetActive(true);
        _origin.SetActive(false);
    }
}
