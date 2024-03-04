using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class DetectedCtrl : MonoBehaviour
{
    protected Renderer[] _renderer;
    //-----------------------------
    [SerializeField]
    protected Shader _originalShader;
    [SerializeField]
    protected Shader _detectedShader;
    //-----------------------------
    private void Awake()
    {
        _renderer = GetComponentsInChildren<Renderer>();
    }
    //-----------------------------
    void OnEnable() { Exit(); }
    //-----------------------------
    public void Enter() 
    {
        foreach (var renderer in _renderer)
            foreach (var material in renderer.materials)
                material.shader = _detectedShader;
    }
    public void Exit() 
    {
        foreach (var renderer in _renderer)
            foreach (var material in renderer.materials)
                material.shader = _originalShader;
    }
}
