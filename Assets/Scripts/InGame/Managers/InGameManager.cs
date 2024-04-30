using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InGameManager : BasicTable
{
    static InGameManager _instance;
    public static InGameManager Instance => _instance;
    //----------------------------------------------------------------------------------
    [SerializeField]
    UnityEvent _startEvent;
    [SerializeField]
    UnityEvent _endEvent;
    //----------------------------------------------------------------------------------
    [Header("About Stage")]
    [SerializeField]
    int _chapterValue;
    [SerializeField]
    int _stageValue;
    //----------------------------------------------------------------------------------
    SStage _currentStage;
    //----------------------------------------------------------------------------------
    float _currentTime;
    //----------------------------------------------------------------------------------
    public int DeliveredCount { get; set; }

    public int FailedCount { get; set; }
    public int FailedProfit { get; set; }

    public int CurrentProfit { get; set; }
    //----------------------------------------------------------------------------------
    // 첫 서빙이 있을 때부터 시간이 흐르도록 할건지 제어
    //[SerializeField]
    // bool _isOperateTimerAfterFirstServing;
    // bool _isOperateTimer = false;
    bool _isStart = false;
    //----------------------------------------------------------------------------------
    [SerializeField]
    ParticleSystem _servingParticle;
    //----------------------------------------------------------------------------------
    void Start()
    {
        _instance = this;

        _currentStage = DataManager.Instance.SeletedStage;

        _currentTime = _currentStage.limitedTime;
        //Test=============================================================
        _currentTime = 30;
        //=============================================================Test
        InGameUIManager.Instance.SetLimitedTime(_currentStage.limitedTime);

        CurrentProfit = 0;

        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Ready, false);
        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Start, false);
        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.End, false);

        FadeManager.Instance.StartFadeIn(() => StartCoroutine(CRT_Start()));
    }
    IEnumerator CRT_Start()
    {
        //  카메라 효과 추가 필요
        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Ready, true);
        yield return new WaitForSeconds(1);
        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Ready, false);

        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Start, true);
        yield return new WaitForSeconds(0.5f);
        InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.Start, false);

        _isStart = true;
        _startEvent.Invoke();
        //for (int count = 0; count < 2; count++)
        OrderManger.Instance.Order();
        OrderManger.Instance.Invoke("Order", 1f);
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
                InGameUIManager.Instance.OnOffMessage(InGameUIManager.eMESSAGE.End, true);

                int deliveredProfit = CurrentProfit + FailedProfit;
                DataManager.Instance.SetResultInfo(DeliveredCount, deliveredProfit, FailedCount, FailedProfit, CurrentProfit);

                _isStart = false;
                Invoke("LoadResult", 2f);
            }
            else
                _currentTime -= Time.deltaTime;

            InGameUIManager.Instance.UpdateTime(_currentTime);
        }
    }
    void LoadResult() { SceneManager.LoadScene("Result"); }
    //----------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        PlateCtrl plate = other.GetComponent<PlateCtrl>();
        if (plate != null)
        {
            if(OrderManger.Instance.CheckOrder(plate.IncludedIngredientsBit))
                _servingParticle.Play();
            
            InGameUIManager.Instance.UpdateProfits(CurrentProfit);
            plate.OnDisableCustom();
        }
        else
        {
            // 접시가 필요합니다 문구 표시
        }
    }
}
