using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    int _currentProfit;
    //----------------------------------------------------------------------------------
    //[SerializeField]
    //ù ������ ���� ������ �ð��� �帣���� �Ұ��� ����
    // bool _ = false;
    bool _isStart = false;
    //----------------------------------------------------------------------------------
    void Start()
    {
        _currentStage = DataManager.Instance.GetStage(_chapterValue, _stageValue);
        _currentStageMenus = DataManager.Instance.GetMenus(_currentStage.menusBit);

        _currentTime = _currentStage.limitedTime;
        _uiManager.SetLimitedTime(_currentStage.limitedTime);

        _currentProfit = 0;

        _currentOrderCool = _orderDelay;
        for (int count = 0; count < 2; count++)
            Order();

        StartCoroutine(CRT_Start());
    }
    IEnumerator CRT_Start()
    {
        yield return new WaitForSeconds(1.5f);
        _uiManager.OnOffReadyText(false);
        _uiManager.OnOffStartText(true);
        yield return new WaitForSeconds(1);
        _uiManager.OnOffStartText(false);

        _isStart = true;
        _startEvent.Invoke();
    }
    void Update()
    {
        if (_isStart)
        {
            //  ���� �ð� üũ
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _endEvent.Invoke();
            }
            else
                _currentTime -= Time.deltaTime;

            _uiManager.UpdateTime(_currentTime);

            //  �ֹ��� ����
            if (_currentOrderCool <= 0)
            {
                _currentOrderCool = _orderDelay;
                Order();
            }
            else
                _currentOrderCool -= Time.deltaTime;
        }
    }
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
                    Debug.Log(_orders[index].name +  "�ϼ�! ���� ����!");
                    _currentProfit += 100;  //  ���� �� 100
                    RemoveOrder(index);
                    break;
                }
            }
            if (!isCorrect)
            {
                Debug.Log("�߸��� �丮..���� ����..");
                _currentProfit -= 100;
                RemoveOrder(0);
            }

            //  �ֹ����� �����ϵ��� �ֹ��� ��Ÿ�� ����
            _currentOrderCool = 0;
            _uiManager.UpdateProfits(_currentProfit);

            plate.OnDisableCustom();
        }
        else
        {
            // ���ð� �ʿ��մϴ� ���� ǥ��
        }
    }
}
