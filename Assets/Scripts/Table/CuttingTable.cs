using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingTable : BasicTable
{
    Slider _slider;
    //------------------------------------------------------------
    IngredientCtrl _IngredientCtrl;
    //------------------------------------------------------------
    bool _isRunningCRTCutting = false;
    //------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();

        _slider = GetComponentInChildren<Slider>();
        _slider.gameObject.SetActive(false);
        _slider.value = 0;
    }
    //------------------------------------------------------------
    public override void Operate(GameObject player)
    {
        if (OnObject != null)
        {
            _IngredientCtrl = OnObject.GetComponent<IngredientCtrl>();
            if(_IngredientCtrl != null &&
                !_IngredientCtrl.IsCooked &&
                !_isRunningCRTCutting) 
                StartCoroutine(CRT_Cutting(player));
        }
    }
    IEnumerator CRT_Cutting(GameObject player)
    {
        _isRunningCRTCutting = true;
        player.GetComponent<PlayerAnimation>().CuttingAni(true);

        OnObject.GetComponent<Rigidbody>().isKinematic = true;
        _slider.gameObject.SetActive(true);
        while (_slider.value < _slider.maxValue)
        {
            _slider.value++;
            yield return new WaitForSeconds(0.3f);
        }
        _slider.value = 0;
        _slider.gameObject.SetActive(false);

        player.GetComponent<PlayerAnimation>().CuttingAni(false);
        
        _IngredientCtrl.ChangeToCookedModel();
        OnObject.GetComponent<Rigidbody>().isKinematic = false;
        _isRunningCRTCutting = false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            _isRunningCRTCutting = false;
        }
    }
}