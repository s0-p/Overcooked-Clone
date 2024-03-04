using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    GameObject _readyText;
    [SerializeField]
    GameObject _startText;
    void Start()
    {
        _readyText.gameObject.SetActive(true);
        _startText.gameObject.SetActive(false);
    }
    //  Start UI
    public void OnOffReadyText(bool isOn) { _readyText.gameObject.SetActive(isOn); }
    public void OnOffStartText(bool isOn) { _startText.gameObject.SetActive(isOn); }
    //-----------------------------------------------------------------------------------
    //  시간 UI
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
    //  수익 UI
    [Space, SerializeField]
    TMP_Text _currentProfitsText;
    public void UpdateProfits(int profits)
    {
        _currentProfitsText.text = profits.ToString();
    }
    //-----------------------------------------------------------------------------------
    //  주문서 UI
    [Space, SerializeField]
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
    
}
