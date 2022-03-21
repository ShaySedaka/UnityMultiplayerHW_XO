using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject[] _boardUISlots;
    [SerializeField] TextMeshProUGUI _symbolIndicator;
    [SerializeField] TextMeshProUGUI _statusLine;
    [SerializeField] MyToggle _portToggle;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] TextMeshProUGUI _winningSignText;
    [SerializeField] GameObject _togglePanel;
    [SerializeField] GameObject _connectButton;

    public MyToggle PortToggle { get => _portToggle;}

    public void PlaceSignInUISlot(int pos, PlayerSign sign)
    {
        foreach (Transform t in _boardUISlots[pos].GetComponentInChildren<Transform>())
        {
            if(t.gameObject.tag == sign.ToString())
            {
                t.gameObject.SetActive(true);
            }
        }
    }

    public void SetPlayerIndicatorText(PlayerSign sign)
    {
        _symbolIndicator.text = "You are playing as: " + sign.ToString();
    }

    public void SetStatusText(string info)
    {
        _statusLine.text = info;
    }

    public void OnGameOver(PlayerSign sign)
    {
        if(sign != PlayerSign.NONE)
        {
            _winningSignText.text = sign.ToString() + " WON!";
        }
        else
        {
            _winningSignText.text = "It's a TIE!";
        }

        _gameOverPanel.SetActive(true);
       
    }

    public void OnConnectPressed()
    {
        _togglePanel.SetActive(false);
        _connectButton.SetActive(false);
        NetworkManager.Instance.StartConnection();
    }
}
