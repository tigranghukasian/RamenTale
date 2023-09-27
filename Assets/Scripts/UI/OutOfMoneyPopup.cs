using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfMoneyPopup : MonoBehaviour, IPopup, IAlertPopup
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

    public void OnOkButtonPressed()
    {
        GameManager.Instance.FirebaseManager.DeleteUserData(() =>
        {
            Close();
            //GameManager.Instance.FullRestart();
        });
        
    }
}
