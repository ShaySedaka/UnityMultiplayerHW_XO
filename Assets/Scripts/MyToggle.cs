using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyToggle : MonoBehaviour
{

    [SerializeField] Sprite _toggleSateSprite1, _toggleStateSprite2;

    [SerializeField] Image _imageComponent; 

    [SerializeField] bool _toggleState;

    public bool ToggleState { get => _toggleState;}

    void Start()
    {
        _toggleState = false;
        OnToggleChange();
    }

    private void OnToggleChange()
    {
        if(ToggleState == true)
        {
            _imageComponent.sprite = _toggleSateSprite1;

        }
        else
        {
            _imageComponent.sprite = _toggleStateSprite2;
        }
    }

    public void ChangeToggle()
    {
        _toggleState = !_toggleState;

        OnToggleChange();
    }
}
