using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    PlayerCtrl[] _playerCtrls;
    int _currentPlayerIndex;

    void Awake()
    {
        _playerCtrls = GetComponentsInChildren<PlayerCtrl>();
    }
    void Start()
    {
        PauseChracters(true);
    }
    public void StartGame()
    {
        PauseChracters(false);

        _currentPlayerIndex = 0;
        
        _playerCtrls[0].Selected(true);
        _playerCtrls[1].Selected(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerCtrls[_currentPlayerIndex].Selected(false);
            _currentPlayerIndex ^= 1;
            _playerCtrls[_currentPlayerIndex].Selected(true);
        }
    }
    public void PauseChracters(bool isOn)
    {
        foreach (PlayerCtrl playerCtrl in _playerCtrls)
            playerCtrl.Pause(isOn);
    }
}
