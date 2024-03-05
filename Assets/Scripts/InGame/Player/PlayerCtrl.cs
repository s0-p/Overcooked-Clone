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
        _move.enabled = isOn;
        _action.enabled = isOn;
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
