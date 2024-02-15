using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _animator;
    void Awake() {  _animator = GetComponentInChildren<Animator>(); }
    public void MoveAni(bool isOn) { _animator.SetBool("Move", isOn); }
    public void PickUpAni() { _animator.SetBool("PickUp", true); }
    public void PutDownAni() 
    {
        _animator.SetTrigger("PutDown"); 
        _animator.SetBool("PickUp", false);
    }
}
