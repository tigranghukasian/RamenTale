using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : PersistentSingleton<PopupManager>
{
    [SerializeField] private GameObject blackBg;

    [SerializeField] private GameObject clearDishPopup;

    public void ClearDishPopup()
    {
        clearDishPopup.gameObject.SetActive(true);
        blackBg.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        blackBg.gameObject.SetActive(false);
    }
}
