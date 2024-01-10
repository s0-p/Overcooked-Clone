using UnityEngine;

public class Table : MonoBehaviour
{
    Renderer _renderer;
    //-----------------------------
    [SerializeField]
    Shader _detectedShader;
    Shader _originShader;
    //-----------------------------
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originShader = _renderer.material.shader;
    }
    //-----------------------------
    public void Enter() { _renderer.material.shader = _detectedShader; }
    public void Exit() { _renderer.material.shader = _originShader; }
}
