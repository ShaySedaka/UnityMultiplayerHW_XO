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

    public MyToggle PortToggle { get => _portToggle;}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
