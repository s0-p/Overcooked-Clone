using System.Collections.Generic;
using UnityEngine;

public class DetectedCtrl : MonoBehaviour
{
    protected Renderer _renderer;
    //-----------------------------
    [SerializeField]
    protected Shader _detectedShader;
    protected Shader[] _originalShaders;
    //-----------------------------
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalShaders = new Shader[_renderer.materials.Length];
        for (int index = 0; index < _originalShaders.Length; index++)
            _originalShaders[index] = _renderer.materials[index].shader;
    }
    void OnEnable() { Exit(); }
    //-----------------------------
    public void Enter() 
    {
        for (int index = 0; index < _originalShaders.Length; index++)
            _renderer.materials[index].shader = _detectedShader;
    }
    public void Exit() 
    {
        for (int index = 0; index < _originalShaders.Length; index++)
            _renderer.materials[index].shader = _originalShaders[index];
    }
}
