using UnityEngine;

public class DetectedCtrl : MonoBehaviour
{
    protected Renderer _renderer;
    //-----------------------------
    [SerializeField]
    protected Shader _detectedShader;
    protected Shader _originShader;
    //-----------------------------
    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originShader = _renderer.material.shader;
    }
    //-----------------------------
    public void Enter() { _renderer.material.shader = _detectedShader; }
    public void Exit() { _renderer.material.shader = _originShader; }
}
