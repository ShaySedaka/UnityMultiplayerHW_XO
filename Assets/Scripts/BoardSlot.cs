using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    [SerializeField] int _slotNumber;

    public void OnSlotClick()
    {

        GameManager.Instance.OnSlotClicked(_slotNumber);
    }

    private void OnMouseDown()
    {
        Debug.Log("Click!");
        OnSlotClick();
    }
}

