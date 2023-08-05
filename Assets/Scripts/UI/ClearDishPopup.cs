using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDishPopup : MonoBehaviour, IPopup, IConfirmablePopup
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        PopupManager.Instance.ClosePopup();
        gameObject.SetActive(false);
    }

    public void OnYesButtonPressed()
    {
        KitchenManager.Instance.CancelDish();
        Close();
    }

    public void OnNoButtonPressed()
    {
        Close();
    }
}
