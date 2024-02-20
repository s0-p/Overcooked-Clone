using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _animator;
    void Awake() {  _animator = GetComponentInChildren<Animator>(); }
    public void MoveAni(bool isOn) 
    {
        if (isOn) CutAni(false);

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
    public void CutAni(bool isOn) { _animator.SetBool("Cut", isOn); }
}
