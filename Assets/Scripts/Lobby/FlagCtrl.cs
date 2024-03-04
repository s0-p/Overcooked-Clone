using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagCtrl : MonoBehaviour
{
    [SerializeField]
    int _chapter;
    [SerializeField]
    int _stage;
    //----------------------------------------------------------------------------------
    SStage _stageInfo;
    //----------------------------------------------------------------------------------
    [SerializeField]
    TMP_Text _title;
    [SerializeField]
    GameObject _infoWindow;
    TMP_Text[] _goalText;
    //----------------------------------------------------------------------------------
    void Awake()
    {
        _stageInfo = DataManager.Instance.GetStage(_chapter, _stage);

        _title.text = $"{_chapter}-{_stage}";

        _goalText = GetComponentsInChildren<TMP_Text>();
        _goalText[1].text = _stageInfo.goalProfits1.ToString();
        _goalText[2].text = _stageInfo.goalProfits2.ToString();
        _goalText[3].text = _stageInfo.goalProfits3.ToString();
    }
    void Start()
    {
        _infoWindow.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(false);
        }
    }
    public void LoadScene() { SceneManager.LoadScene(_title.text); }
}
