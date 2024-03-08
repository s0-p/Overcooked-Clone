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
    void Start()
    {
        _readyText.transform.localScale = _offset;
        _startText.transform.localScale = _offset;
        _endText.transform.localScale = _offset;
    }
    //-----------------------------------------------------------------------------------
    //  Message UI
    public enum eMESSAGE
    {
        Ready,
        Start,
        End
    }
    [SerializeField]
    GameObject _readyText;
    [SerializeField]
    GameObject _startText;
    [SerializeField]
    GameObject _endText;
    Vector3 _offset = new Vector3(0.1f, 0.1f, 0.1f);
    public void OnOffMessage(eMESSAGE type, bool isOn) 
    {
        GameObject message = null;
        switch(type)
        {
            case eMESSAGE.Ready:
                message = _readyText;
                break;
            case eMESSAGE.Start:
                message = _startText;
                break;
            case eMESSAGE.End:
                message = _endText;
                break;
        }

        message?.SetActive(isOn);
        if (isOn) StartCoroutine(CRT_ShowMessage(message));
    }
    IEnumerator CRT_ShowMessage(GameObject text)
    {
        while (text.transform.localScale.x < 1)
        {
            yield return new WaitForSeconds(0.02f);
            text.transform.localScale += _offset;
        }
    }
    //-----------------------------------------------------------------------------------
    //  시간 UI
    [Space, SerializeField]
    GameObject _timePanel;
    TMP_Text _timeText;
    Slider _timeSlider;
    [SerializeField]
    Image _timeSliderFill;
    [SerializeField]
    Color _maxTimeColor;
    [SerializeField]
    Color _midTimeColor;
    [SerializeField]
    Color _minTimeColor;
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

        float remainTimeRatio = _timeSlider.value / _timeSlider.maxValue;
        if (_timeSlider.value / _timeSlider.maxValue > 0.5f)
            _timeSliderFill.color = Color.Lerp(_midTimeColor, _maxTimeColor, (remainTimeRatio - 0.5f) * 2);
        else
            _timeSliderFill.color = Color.Lerp(_minTimeColor, _midTimeColor, remainTimeRatio * 2);
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
    //ObjectPool _ObjectPool;
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
