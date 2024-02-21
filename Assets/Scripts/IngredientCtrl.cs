using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject _origin;
    [SerializeField]
    GameObject _cooked;
    bool _isCooked = false;

    void Awake()
    {
        _origin.SetActive(true);
        _cooked.SetActive(false);
    }
    public void ChangeToCookedModel()
    {
        _isCooked = true;
        _cooked.SetActive(true);
        _origin.SetActive(false);
    }
}
