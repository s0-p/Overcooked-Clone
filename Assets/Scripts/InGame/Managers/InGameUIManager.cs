using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    static InGameUIManager _instance;
    public static InGameUIManager Instance => _instance;
    //-----------------------------------------------------------------------------------
    public enum eMESSAGE
    {
        Ready,
        Start,
        End
    }
    [Header("Message UI")]
    [SerializeField]
    GameObject _readyText;
    [SerializeField]
    GameObject _startText;
    [SerializeField]
    GameObject _endText;
    Vector3 _messageOffset = new Vector3(0.1f, 0.1f, 0.1f);

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
            text.transform.localScale += _messageOffset;
        }
    }
    //-----------------------------------------------------------------------------------
    [Header("수익 UI")]
    [SerializeField]
    TMP_Text _currentProfitsText;
    public void UpdateProfits(int profits)
    {
        _currentProfitsText.text = profits.ToString();
    }
    //-----------------------------------------------------------------------------------
    [Header("시간 UI")]
    [SerializeField]
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
    
    void Awake()
    {
        _instance = this;

        _timeText = _timePanel.GetComponentInChildren<TMP_Text>();
        _timeSlider = _timePanel.GetComponentInChildren<Slider>();
    }
    void Start()
    {
        _readyText.transform.localScale = _messageOffset;
        _startText.transform.localScale = _messageOffset;
        _endText.transform.localScale = _messageOffset;
    }
}
