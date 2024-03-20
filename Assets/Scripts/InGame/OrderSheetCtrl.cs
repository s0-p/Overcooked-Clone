using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSheetCtrl : MonoBehaviour
{
    public enum eORDER_ANIMATION
    {
        Move,
        Arrive,
        RedLight,
        GreenLight
    }
    Animator _animator;
    //-------------------------------------------------------------------------------
    [SerializeField]
    float _limitedTime;
    float _currentTime;
    Slider _timerSlider;
    //-------------------------------------------------------------------------------
    [SerializeField]
    Image _menuImage;
    bool _isSetted;
    //-------------------------------------------------------------------------------
    void Awake() 
    { 
        _animator = GetComponent<Animator>();
        _timerSlider = GetComponentInChildren<Slider>();

        _timerSlider.maxValue = _timerSlider.value = _limitedTime;
    }
    void Update()
    {
        if (_isSetted)
        {
            if (_currentTime > 0)
                _currentTime -= Time.deltaTime;
            else
            {
                OrderManger.Instance.RemoveOrder(transform.GetSiblingIndex(), eORDER_ANIMATION.RedLight); ;
                _isSetted = false;
            }
            _timerSlider.value = _currentTime;
        }
    }
    public void SetInfo(Sprite menuSprite) 
    {
        _currentTime = _limitedTime;
        _menuImage.sprite = menuSprite;
        _isSetted = true;
    }
    public void RunAnimation(eORDER_ANIMATION type) { _animator.SetTrigger(type.ToString()); }
}
