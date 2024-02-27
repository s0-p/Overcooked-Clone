using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    void Awake()
    {
        _timeText = _timePanel.GetComponentInChildren<TMP_Text>();
        _timeSlider = _timePanel.GetComponentInChildren<Slider>();
    }
    //-----------------------------------------------------------------------------------
    [SerializeField]
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
    [SerializeField]
    Transform _ordersContentTransform;
    [SerializeField]
    GameObject _orderPrefab;
    public void CreateOrderSheet(SMenu menu)
    {
        GameObject orderSheet = Instantiate(_orderPrefab, _ordersContentTransform);
        orderSheet.GetComponentInChildren<TMP_Text>().text = menu.name;
    }
    public void RemoveOrderSheet(int index)
    {
        Destroy(_ordersContentTransform.GetChild(index).gameObject);
    }
}
