using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    TMP_Text _stageTitle;
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    Image[] _emptyStars;
    [SerializeField]
    Image[] _filledStars;
    [SerializeField]
    TMP_Text[] _goalProfitsText;

    [SerializeField]
    Vector3 _minScale;
    [SerializeField]
    float _maxFilledStarSize;
    [SerializeField]
    float _finalFilledStarSize;
    Vector3 _offset = new Vector3(0.1f, 0.1f, 0.1f);
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    TMP_Text _deliveredCount;
    [SerializeField]
    TMP_Text _deliveredProfit;

    [SerializeField]
    TMP_Text _failedCount;
    [SerializeField]
    TMP_Text _failedProfit;
    //-----------------------------------------------------------------------------------
    [Space, SerializeField]
    TMP_Text _totalProfit;
    //-----------------------------------------------------------------------------------
    bool _isCompleted = false;
    //-----------------------------------------------------------------------------------
    void Start()
    {
        enabled = true;
        _isCompleted = false;

        SStage stageInfo = DataManager.Instance.SelectedStage;
        _stageTitle.text = $"{stageInfo.chapter}-{stageInfo.stage}";

        _goalProfitsText[0].text = stageInfo.goalProfits1.ToString();
        _goalProfitsText[1].text = stageInfo.goalProfits2.ToString();
        _goalProfitsText[2].text = stageInfo.goalProfits3.ToString();

        _deliveredCount.transform.parent.gameObject.SetActive(false);
        _failedCount.transform.parent.gameObject.SetActive(false);
        _totalProfit.transform.parent.gameObject.SetActive(false);

        DataManager.SResult resultInfo = DataManager.Instance.ResultInfo;
        _deliveredCount.text = resultInfo.deliveredCount.ToString();
        _deliveredProfit.text = resultInfo.deliveredProfit.ToString();

        _failedCount.text = resultInfo.failedCount.ToString();
        _failedProfit.text = (-resultInfo.failedProfit).ToString();

        _totalProfit.text = resultInfo.totalProfit.ToString();

        StartCoroutine(CRT_ShowResult());
        StartCoroutine(CRT_ShowStars(resultInfo.totalProfit));
    }
    //-----------------------------------------------------------------------------------
    IEnumerator CRT_ShowStars(int totalProfit)
    {
        for (int index = 0; index < _filledStars.Length; index++)
            _filledStars[index].gameObject.SetActive(false);

        for (int index = 0; index < _goalProfitsText.Length; index++)
        {
            if (totalProfit >= int.Parse(_goalProfitsText[index].text))
                yield return StartCoroutine(CRT_StarEffect(_filledStars[index].transform));
            else
                break;
        }
    }
    IEnumerator CRT_StarEffect(Transform start)
    {
        yield return new WaitForSeconds(1f);
        start.localScale = _minScale;
        start.gameObject.SetActive(true);
        while (start.localScale.x < _maxFilledStarSize)
        {
            yield return new WaitForSeconds(0.01f);
            start.localScale += _offset;
        }
        while (start.localScale.x > _finalFilledStarSize)
        {
            yield return new WaitForSeconds(0.01f);
            start.localScale -= _offset;
        }
    }
    //-----------------------------------------------------------------------------------
    IEnumerator CRT_ShowResult()
    {
        yield return new WaitForSeconds(1f);

        _deliveredProfit.gameObject.SetActive(false);
        _deliveredCount.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _deliveredProfit.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        _failedProfit.gameObject.SetActive(false);
        _failedCount.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _failedProfit.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        _totalProfit.transform.parent.gameObject.SetActive(true);

        _isCompleted = true;
    }
    //-----------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && _isCompleted)
        {
            DataManager.Instance.isSelectedStage = false;
            FadeManager.Instance.StartFadeOut(() => SceneManager.LoadScene("Lobby"));
            enabled = false;
        }
    }
}
