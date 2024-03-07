using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    //  In Game -> Lobby UI
    [Space, SerializeField]
    Image _tipImage;
    [SerializeField]
    TMP_Text _tipText;
    //------------------------------------------------------------------
    //  Lobby -> In Game UI
    [Space, SerializeField]
    GameObject _stageInfoBG;
    Image _inGameImage;
    TMP_Text _stageTitle;

    [SerializeField]
    GameObject _goalProfits;
    TMP_Text[] _goalProfitsText = new TMP_Text[3];
    //------------------------------------------------------------------
    [Space, SerializeField]
    float _sliderSpeed;
    [SerializeField]
    Slider _loadingSlider;
    //------------------------------------------------------------------
    AsyncOperation _asyncOp;
    //------------------------------------------------------------------
    void Awake()
    {
        _inGameImage = _stageInfoBG.GetComponentInChildren<Image>();
        _stageTitle = _stageInfoBG.GetComponentInChildren<TMP_Text>();

        _goalProfitsText = _goalProfits.GetComponentsInChildren<TMP_Text>();
    }
    void Start()
    {
        if(DataManager.Instance.isSeletedStage)
        {
            _tipImage.gameObject.SetActive(false);
            _tipText.gameObject.SetActive(false);

            _stageInfoBG.SetActive(true);
            _goalProfits.SetActive(true);

            SStage currentStage = DataManager.Instance.SeletedStage;

            //_inGameImage.sprite = DataManager.Instance.InGameImages[currentStage.id];
            _stageTitle.text = $"{currentStage.chapter}-{currentStage.stage}";

            _goalProfitsText[0].text = currentStage.goalProfits1.ToString();
            _goalProfitsText[1].text = currentStage.goalProfits2.ToString();
            _goalProfitsText[2].text = currentStage.goalProfits3.ToString();
        }
        //  선택된 스테이지가 없음
        else
        {
            _stageInfoBG.SetActive(false);
            _goalProfits.SetActive(false);

            _tipImage.gameObject.SetActive(true);
            _tipText.gameObject.SetActive(true);

            //_tipImage.sprite = 
            //_tipText.text = 
        }
        _loadingSlider.value = 0;

        FadeManager.Instance.StartFadeIn(Loading);
    }
    public void Loading()
    {
        if (DataManager.Instance.isSeletedStage)
            StartCoroutine(CRT_Loading(_stageTitle.text));
        else
            StartCoroutine(CRT_Loading("Lobby"));
    }
    IEnumerator CRT_Loading(string sceneName)
    {
        _asyncOp = SceneManager.LoadSceneAsync(sceneName);
        _asyncOp.allowSceneActivation = false;
        float timer = 0;
        while(!_asyncOp.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (_asyncOp.progress < 0.9f)
            {
                _loadingSlider.value = Mathf.Lerp(_loadingSlider.value, _asyncOp.progress, timer * _sliderSpped);
                if (_loadingSlider.value >= _asyncOp.progress)
                    timer = 0;
            }
            else
            {
                _loadingSlider.value = Mathf.Lerp(_loadingSlider.value, 1, timer);
                if (_loadingSlider.value == 1)
                {

                    FadeManager.Instance.StartFadeOut(() => _asyncOp.allowSceneActivation = true);
                    yield break;
                }
            }
        }
    }
}
