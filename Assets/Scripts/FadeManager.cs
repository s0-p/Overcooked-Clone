using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    static FadeManager _instance;
    public static FadeManager Instance => _instance;
    //------------------------------------------------------------------------------------
    Canvas _canvas;
    //------------------------------------------------------------------------------------
    [SerializeField]
    float _closeupPosZ;
    [SerializeField]
    float _fadeSpeed;
    [SerializeField]
    RectTransform _blackImageRectTrsf;
    //------------------------------------------------------------------------------------
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _canvas = GetComponent<Canvas>();
    }
    public void StartFadeIn(Action actionAfterFadeIn) 
    { 
        _canvas.worldCamera = Camera.main;
        StartCoroutine(CRT_FadeIn(actionAfterFadeIn)); 
    }
    IEnumerator CRT_FadeIn(Action actionAfterFadeIn)
    {
        while (_blackImageRectTrsf.localPosition.z > _closeupPosZ)
        {
            _blackImageRectTrsf.localPosition -= Vector3.forward * _fadeSpeed;
            yield return null;
        }
        actionAfterFadeIn?.Invoke();
    }
    public void StartFadeOut(Action actionAfterFadeOut)
    {
        StartCoroutine(CRT_FadeOut(actionAfterFadeOut));
    }
    IEnumerator CRT_FadeOut(Action actionAfterFadeOut)
    {
        while (_blackImageRectTrsf.localPosition.z < 0)
        {
            _blackImageRectTrsf.localPosition += Vector3.forward * _fadeSpeed;
            yield return null;
        }
        actionAfterFadeOut?.Invoke();
    }
}
