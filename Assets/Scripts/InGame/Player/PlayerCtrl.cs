using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Animator _animator;
    PlayerMove _move;
    PlayerAction _action;
    PlayerAnimation _animation;
    //-------------------------------------------------------------
    [SerializeField]
    GameObject _seletedMark;
    [SerializeField]
    ParticleSystem _switchParticle;
    //-------------------------------------------------------------
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _move = GetComponent<PlayerMove>();
        _action = GetComponent<PlayerAction>();
        _animation = GetComponent<PlayerAnimation>();
    }
    //-------------------------------------------------------------
    public void Selected(bool isOn)
    {
        if(isOn) _switchParticle.Play();
        _move.enabled = isOn;
        _action.enabled = isOn;
        _seletedMark.SetActive(isOn);
        _animation.MoveAni(false);
    }
    public void Pause(bool isOn)
    {
        _move.enabled = !isOn;
        _action.enabled = !isOn;
        _animation.enabled = !isOn;
        _animator.enabled = !isOn;
    }
}
