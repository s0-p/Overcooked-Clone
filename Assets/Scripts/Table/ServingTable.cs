using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingTable : BasicTable
{
    [SerializeField]
    int _chapterValue;
    [SerializeField]
    int _stageValue;
    //----------------------------------------------------------------------------------
    SStage _stage;
    SMenu[] _menus;
    //----------------------------------------------------------------------------------
    void Start()
    {
        _stage = DataManager.Instance.GetStage(_chapterValue, _stageValue);
        _menus = DataManager.Instance.GetMenus(_stage.menusBit);
    }
    void OnTriggerEnter(Collider other)
    {
        PlateCtrl plate = other.GetComponent<PlateCtrl>();
        if (plate != null)
        {
            for (int index = 0; index < _menus.Length; index++)
            {
                if (plate.IncludedIngredientsBit == _menus[index].ingredientsBit)
                {
                    Debug.Log(_menus[index].name +  "�ϼ�! ���� ����!");
                    plate.gameObject.SetActive(false);
                    return;
                }
            }
            Debug.Log("�߸��� �丮..���� ����..");
            plate.gameObject.SetActive(false);
        }
    }
}
