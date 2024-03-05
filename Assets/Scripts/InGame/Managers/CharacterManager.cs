using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    PlayerCtrl[] _playerCtrls;
    PlayerCtrl _currentPlayer;

    void Awake()
    {
        _playerCtrls = GetComponentsInChildren<PlayerCtrl>();

        _playerCtrls[1].Selected(false);
        _currentPlayer = _playerCtrls[0];
        _currentPlayer.Selected(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _currentPlayer.Selected(false);
            _currentPlayer = (_currentPlayer == _playerCtrls[0]) ? _playerCtrls[1] : _playerCtrls[0];
            _currentPlayer.Selected(true);
        }
    }
}
