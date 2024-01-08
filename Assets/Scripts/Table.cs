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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _renderer.material.shader = _detectedShader;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _renderer.material.shader = _originShader;
        }
    }
}
