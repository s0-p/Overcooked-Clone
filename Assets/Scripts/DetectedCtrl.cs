using UnityEngine;

public class DetectedCtrl : MonoBehaviour
{
    protected Renderer[] _renderers;
    //-----------------------------
    [SerializeField]
    protected Shader _detectedShader;
    protected Shader[] _originShaders;
    //-----------------------------
    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        _originShaders = new Shader[_renderers.Length];
        for (int index = 0; index < _originShaders.Length; index++)
            _originShaders[index] = _renderers[index].material.shader;
    }
    void OnEnable() { Exit(); }
    //-----------------------------
    public void Enter() 
    {
        for (int index = 0; index < _originShaders.Length; index++)
            _renderers[index].material.shader = _detectedShader;
    }
    public void Exit() 
    {
        for (int index = 0; index < _originShaders.Length; index++)
            _renderers[index].material.shader = _originShaders[index];
    }
}
