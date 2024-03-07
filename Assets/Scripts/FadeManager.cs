using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    static FadeManager _instance;
    public static FadeManager Instance => _instance;
    //------------------------------------------------------------------------------------
    [SerializeField]
    float _maxSize;
    [SerializeField]
    RectTransform _blackImageRectTrsf;
    Vector3 _offset = new Vector3(1, 1, 1);
    //------------------------------------------------------------------------------------
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        //Test=============================================================
        StartFadeOut(() => SceneManager.LoadScene("Lobby"));
        //=============================================================Test
    }
    public void StartFadeIn(Action actionAfterFadeIn) 
    { 
        StartCoroutine(CRT_FadeIn(actionAfterFadeIn)); 
    }
    IEnumerator CRT_FadeIn(Action actionAfterFadeIn)
    {
        while (_blackImageRectTrsf.localScale.x < _maxSize)
        {
            yield return null;
            _blackImageRectTrsf.localScale += _offset;
        }
        actionAfterFadeIn?.Invoke();
    }
    public void StartFadeOut(Action actionAfterFadeOut)
    {
        StartCoroutine(CRT_FadeOut(actionAfterFadeOut));
    }
    IEnumerator CRT_FadeOut(Action actionAfterFadeOut)
    {
        while (_blackImageRectTrsf.localScale.x > 1)
        {
            yield return null;
            _blackImageRectTrsf.localScale -= _offset;
        }
        actionAfterFadeOut?.Invoke();
    }
}
