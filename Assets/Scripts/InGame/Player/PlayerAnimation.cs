using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _animator;
    PlayerAction _playerAction;
    void Awake() 
    {  
        _animator = GetComponent<Animator>(); 
        _playerAction = GetComponent<PlayerAction>();
    }
    public void MoveAni(bool isOn) 
    {
        if (isOn && _animator.GetBool("Cut")) CuttingAni(false);

        _animator.SetBool("Move", isOn); 
    }
    public void PickUpAni() 
    { 
        _animator.SetBool("PickUp", true);
        _animator.SetBool("Move", false);
    }
    public void PutDownAni() 
    {
        _animator.SetBool("PickUp", false);
        _animator.SetTrigger("PutDown"); 
    }
    public void CuttingAni(bool isOn) 
    {
        _playerAction.PauseCutting(isOn);
        _animator.SetBool("Cut", isOn); 
    }
}
