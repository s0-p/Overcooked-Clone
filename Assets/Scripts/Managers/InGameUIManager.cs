using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    void Awake()
    {
        _timeText = _timePanel.GetComponentInChildren<TMP_Text>();
        _timeSlider = _timePanel.GetComponentInChildren<Slider>();

        //_poolManager.Init(, _orderPrefab);
    }
    //-----------------------------------------------------------------------------------
    //  주문서UI 관리
    [SerializeField]
    Transform _ordersContentTransform;
    [SerializeField]
    GameObject _orderPrefab;
    [SerializeField]
    Sprite[] _menuSprites;
    //----------------------------------------------------
    ObjectPoolingManager _poolManager;
    //----------------------------------------------------
    public void CreateOrderSheet(SMenu menu)
    {
        OrderSheetCtrl orderSheet = Instantiate(_orderPrefab, _ordersContentTransform).GetComponent<OrderSheetCtrl>();

        if((int)Mathf.Log(menu.bitId, 2) < _menuSprites.Length)
            orderSheet.SetMenuImage(_menuSprites[(int)Mathf.Log(menu.bitId, 2)]);
    }
    public void RemoveOrderSheet(int index)
    {
        Destroy(_ordersContentTransform.GetChild(index).gameObject);
    }
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    GameObject _timePanel;
    TMP_Text _timeText;
    Slider _timeSlider;
    public void SetLimitedTime(int limitedTime)
    {
        _timeSlider.maxValue = limitedTime;
        _timeSlider.value = limitedTime;

        _timeText.text = string.Format("{0:D2}:{1:D2}", (int)_timeSlider.value / 60, (int)_timeSlider.value % 60);
    }
    public void UpdateTime(float remainTime)
    {
        _timeSlider.value = remainTime;
        _timeText.text = string.Format("{0:D2}:{1:D2}", (int)_timeSlider.value / 60, (int)_timeSlider.value % 60);
    }
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    TMP_Text _currentProfitsText;
    public void UpdateProfits(int profits)
    {
        _currentProfitsText.text = profits.ToString();
    }
}
