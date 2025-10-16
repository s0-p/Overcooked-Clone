using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    Image _inGameImage;
    TMP_Text[] _goalText;
    //----------------------------------------------------------------------------------
    void Start()
    {
        //  플래그에 해당되는 스테이지 데이터 로딩
        _stageInfo = DataManager.Instance.GetStage(_chapter, _stage);

        _title.text = $"{_chapter}-{_stage}";
        if(_stageInfo.id < DataManager.Instance.InGameImages.Length)
        _inGameImage.sprite = DataManager.Instance.InGameImages[_stageInfo.id];

        _goalText = GetComponentsInChildren<TMP_Text>();
        _goalText[1].text = _stageInfo.goalProfits1.ToString();
        _goalText[2].text = _stageInfo.goalProfits2.ToString();
        _goalText[3].text = _stageInfo.goalProfits3.ToString();

        _infoWindow.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        //  스테이지 정보 창 활성화
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //  스테이지 정보 창 비활성화
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(false);
        }
    }
    public void LoadInGame() 
    {
        //  데이터 매니저에게 정보 전달
        DataManager.Instance.isSelectedStage = true;
        DataManager.Instance.SelectedStage = _stageInfo;

        //  FadeOut 후 스테이지 씬 로딩
        FadeManager.Instance.StartFadeOut(() => SceneManager.LoadScene("Loading"));
    }
}
