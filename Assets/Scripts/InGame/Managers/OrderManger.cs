using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManger : MonoBehaviour
{
    static OrderManger _instnace;
    public static OrderManger Instance => _instnace;
    //----------------------------------------------------------------------------------
    [SerializeField]
    int _maxCount;
    //----------------------------------------------------------------------------------
    float _orderStartPosX;
    Vector3 _orderOffset = Vector3.left;
    [SerializeField]
    float _spacing = 20;
    [SerializeField]
    float _orderMovingSpeed;
    //----------------------------------------------------------------------------------
    [SerializeField]
    GameObject _orderPrefab;
    [SerializeField]
    Sprite[] _menuSprites;
    //----------------------------------------------------------------------------------
    [SerializeField]
    ObjectPool _orderPool;

    SMenu[] _currentStageMenus;
    List<SMenu> _currentOrders = new List<SMenu>();
    //----------------------------------------------------------------------------------
    void Awake()
    {
        _instnace = this;

        _orderStartPosX = Screen.width;

        _currentStageMenus = DataManager.Instance.GetMenus(DataManager.Instance.SeletedStage.menusBit);
        _orderPool.Init(_maxCount, _orderPrefab);
    }
    //----------------------------------------------------------------------------------
    public void Order()
    {
        if (_currentOrders.Count < _maxCount)
        {
            int randomIndex = Random.Range(0, _currentStageMenus.Length);
            _currentOrders.Add(_currentStageMenus[randomIndex]);

            CreateOrderSheet(_currentStageMenus[randomIndex]);
        }
    }
    public void CreateOrderSheet(SMenu menu)
    {
        OrderSheetCtrl orderSheet = _orderPool.Get().GetComponent<OrderSheetCtrl>();
        orderSheet.transform.SetParent(transform);

        if ((int)Mathf.Log(menu.bitId, 2) < _menuSprites.Length)
            orderSheet.SetInfo(_menuSprites[(int)Mathf.Log(menu.bitId, 2)]);

        StartCoroutine(CRT_OrderSheetEffect(orderSheet));
    }
    IEnumerator CRT_OrderSheetEffect(OrderSheetCtrl orderSheet)
    {
        orderSheet.RunAnimation(OrderSheetCtrl.eORDER_ANIMATION.Move);

        RectTransform rectTransform = (RectTransform)orderSheet.transform;
        rectTransform.anchoredPosition = Vector2.right * _orderStartPosX;
        float finalPosXOffset = (_orderPool.ActivatedGobjs.Count - 1) * (100 + _spacing * 2);
        while (rectTransform.localPosition.x > finalPosXOffset)
        {
            yield return null;
            rectTransform.localPosition += Time.deltaTime * _orderMovingSpeed * _orderOffset;
        }

        orderSheet.RunAnimation(OrderSheetCtrl.eORDER_ANIMATION.Arrive);
    }

    //----------------------------------------------------------------------------------
    public bool CheckOrder(int includedIngredientsBit)
    {
        for (int index = 0; index < _currentOrders.Count; index++)
        {
            if (includedIngredientsBit == _currentOrders[index].ingredientsBit)
            {
                RemoveOrder(index, OrderSheetCtrl.eORDER_ANIMATION.GreenLight);
                return true;
            }
        }
        ++InGameManager.Instance.FailedCount;
        InGameManager.Instance.FailedProfit += 20;
        InGameManager.Instance.CurrentProfit -= 20;
        InGameUIManager.Instance.UpdateProfits(InGameManager.Instance.CurrentProfit);
        foreach (GameObject order in _orderPool.ActivatedGobjs)
            order.GetComponent<OrderSheetCtrl>().RunAnimation(OrderSheetCtrl.eORDER_ANIMATION.RedLight);
        return false;
    }
    //----------------------------------------------------------------------------------
    public void RemoveOrder(int removedIndex, OrderSheetCtrl.eORDER_ANIMATION eOrderAniType)
    {
        if (eOrderAniType == OrderSheetCtrl.eORDER_ANIMATION.GreenLight)
        {
            ++InGameManager.Instance.DeliveredCount;
            InGameManager.Instance.CurrentProfit += 20;
        }
        else if (eOrderAniType == OrderSheetCtrl.eORDER_ANIMATION.RedLight)
        {
            ++InGameManager.Instance.FailedCount;
            InGameManager.Instance.FailedProfit += 20;
            InGameManager.Instance.CurrentProfit -= 20;
        }

        InGameUIManager.Instance.UpdateProfits(InGameManager.Instance.CurrentProfit);
        _currentOrders.RemoveAt(removedIndex);

        GameObject orderSheet = _orderPool.ActivatedGobjs[removedIndex];
        orderSheet.GetComponent<OrderSheetCtrl>().RunAnimation(eOrderAniType);

        StartCoroutine(CRT_RelocateOrders(removedIndex, orderSheet));
    }
    IEnumerator CRT_RelocateOrders(int removedIndex, GameObject orderSheet)
    {
        yield return new WaitForSeconds(0.5f);
        _orderPool.Return(orderSheet);

        for (int index = removedIndex; index < _orderPool.ActivatedGobjs.Count; index++)
            _orderPool.ActivatedGobjs[index].transform.localPosition += _orderOffset * (100 + _spacing * 2);

        yield return null;
        Order();
    }
}
