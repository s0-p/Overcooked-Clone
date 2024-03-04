using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagCtrl : MonoBehaviour
{
    [SerializeField]
    int _chapter;
    [SerializeField]
    int _stage;
    //----------------------------------------------------------------------------------
    [SerializeField]
    TMP_Text _title;
    [SerializeField]
    GameObject _infoWindow;
    //----------------------------------------------------------------------------------
    void Awake()
    {
        _title.text = $"{_chapter}-{_stage}";
    }
    void Start()
    {
        _infoWindow.SetActive(false);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(true);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _infoWindow.SetActive(false);
        }
    }
    public void LoadScene() { SceneManager.LoadScene(_title.text); }
}
