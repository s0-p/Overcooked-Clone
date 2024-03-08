using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InGameManager : BasicTable
{
    [SerializeField]
    UnityEvent _startEvent;
    [SerializeField]
    UnityEvent _endEvent;
    //----------------------------------------------------------------------------------
    [SerializeField]
    InGameUIManager _uiManager;
    //----------------------------------------------------------------------------------
    [Header("About Stage")]
    [SerializeField]
    int _chapterValue;
    [SerializeField]
    int _stageValue;
    //----------------------------------------------------------------------------------
    SStage _currentStage;
    SMenu[] _currentStageMenus;
    //----------------------------------------------------------------------------------
    [Header("About Order")]
    [SerializeField]
    int _maxOrder;
    [SerializeField]
    float _orderDelay;

    float _currentOrderCool;
    List<SMenu> _orders = new List<SMenu>();
    //----------------------------------------------------------------------------------
    float _currentTime;
    //----------------------------------------------------------------------------------
    int _deliveredCount;
    
    int _failedCount;
    int _failedProfit;

    int _currentProfit;
    //----------------------------------------------------------------------------------
    // 첫 서빙이 있을 때부터 시간이 흐르도록 할건지 제어
    //[SerializeField]
    // bool _isOperateTimerAfterFirstServing;
    // bool _isOperateTimer = false;
    bool _isStart = false;
    //----------------------------------------------------------------------------------
    void Start()
    {
        _currentStage = DataManager.Instance.SeletedStage;
        _currentStageMenus = DataManager.Instance.GetMenus(_currentStage.menusBit);

        _currentTime = _currentStage.limitedTime;
        //Test=============================================================
        _currentTime = 3;
        //=============================================================Test
        _uiManager.SetLimitedTime(_currentStage.limitedTime);

        _currentProfit = 0;

        _currentOrderCool = _orderDelay;

        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Ready, false);
        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Start, false);
        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.End, false);

        FadeManager.Instance.StartFadeIn(() => StartCoroutine(CRT_Start()));
    }
    IEnumerator CRT_Start()
    {
        //  카메라 효과 추가 필요
        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Ready, true);
        yield return new WaitForSeconds(1);
        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Ready, false);

        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Start, true);
        yield return new WaitForSeconds(0.5f);
        _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.Start, false);

        _isStart = true;
        _startEvent.Invoke();
        for (int count = 0; count < 2; count++)
            Order();
        
    }
    //----------------------------------------------------------------------------------
    void Update()
    {
        if (_isStart)
        {
            //  제한 시간 체크
            if (_currentTime <= 0)
            {
                //  End Game
                _currentTime = 0;
                _endEvent.Invoke();
                _uiManager.OnOffMessage(InGameUIManager.eMESSAGE.End, true);

                int deliveredProfit = _currentProfit + _failedProfit;
                DataManager.Instance.SetResultInfo(_deliveredCount, deliveredProfit, _failedCount, _failedProfit, _currentProfit);

                _isStart = false;
                Invoke("LoadResult", 2f);
            }
            else
                _currentTime -= Time.deltaTime;

            _uiManager.UpdateTime(_currentTime);

            //  주문서 관리
            if (_currentOrderCool <= 0)
            {
                _currentOrderCool = _orderDelay;
                Order();
            }
            else
                _currentOrderCool -= Time.deltaTime;
        }
    }
    void LoadResult() { SceneManager.LoadScene("Result"); }
    //----------------------------------------------------------------------------------
    void Order()
    {
        if (_orders.Count < _maxOrder)
        {
            int randomIndex = Random.Range(0, _currentStageMenus.Length);
            _orders.Add(_currentStageMenus[randomIndex]);

            _uiManager.CreateOrderSheet(_currentStageMenus[randomIndex]);
        }
    }
    void RemoveOrder(int index)
    {
        _orders.RemoveAt(index);
        _uiManager.RemoveOrderSheet(index);
    }
    //----------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        PlateCtrl plate = other.GetComponent<PlateCtrl>();
        if (plate != null)
        {
            bool isCorrect = false;
            for (int index = 0; index < _orders.Count; index++)
            {
                if (plate.IncludedIngredientsBit == _orders[index].ingredientsBit)
                {
                    isCorrect = true;
                    Debug.Log(_orders[index].name +  "완성! 수익 증가!");
                    _currentProfit += 100;  //  임의 값 100
                    RemoveOrder(index);
                    break;
                }
            }
            if (!isCorrect)
            {
                Debug.Log("잘못된 요리..수익 감소..");
                _currentProfit -= 100;
                RemoveOrder(0);
            }

            //  주문서를 생성하도록 주문서 쿨타임 제거
            _currentOrderCool = 0;
            _uiManager.UpdateProfits(_currentProfit);

            plate.OnDisableCustom();
        }
        else
        {
            // 접시가 필요합니다 문구 표시
        }
    }
}
