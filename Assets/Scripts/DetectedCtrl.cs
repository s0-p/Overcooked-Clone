using UnityEngine;

public class DetectedCtrl : MonoBehaviour
{
    protected Renderer _renderer;
    //-----------------------------
    [SerializeField]
    protected Shader _detectedShader;
    protected Shader _originShader;
    //-----------------------------
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originShader = _renderer.material.shader;
    }
    void OnEnable() { Exit(); }
    //-----------------------------
    public void Enter() { _renderer.material.shader = _detectedShader; }
    public void Exit() { _renderer.material.shader = _originShader; }
}
