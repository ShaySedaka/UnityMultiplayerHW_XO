using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortConfig
{
    Port1,
    Port2
}

public class NetworkManager : Singleton<NetworkManager>
{

    private PortConfig _myPortConfig = PortConfig.Port1;

    public PortConfig MyPortConfig { get => _myPortConfig; set => _myPortConfig = value; }

    // Start is called before the first frame update
    void Start()
    {
        _myPortConfig = PortConfig.Port1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePortConfig();
    }

    private void UpdatePortConfig()
    {
        bool toggleState = UIManager.Instance.PortToggle.ToggleState;

        if(toggleState == false)
        {
            MyPortConfig = PortConfig.Port1;
        }
        else
        {
            MyPortConfig = PortConfig.Port2;
        }
    }
}
