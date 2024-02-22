using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject _origin;
    [SerializeField]
    GameObject _cooked;
    public bool IsCooked { get; set; }

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
