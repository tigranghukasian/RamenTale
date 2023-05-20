using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenManager : Singleton<KitchenManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image completeButton;
    [SerializeField] private PlateSpot plateSpot;

    [SerializeField] private Color completeButtonEnabledColor;
    [SerializeField] private Color completeButtonDisabledColor;
    [SerializeField] private KitchenUIManager kitchenUIManager;

    public Canvas Canvas => canvas;

    public void SetCompleteButton(bool state)
    {
        completeButton.color = state ? completeButtonEnabledColor : completeButtonDisabledColor;
        completeButton.GetComponent<Button>().interactable = state;
    }

    public void CompleteDish()
    {
        SetCompleteButton(false);
        GameManager.Instance.CompleteDish(plateSpot.CurrentDish);
        GameManager.Instance.OpenCafe();
    }

    public void SetOrder()
    {
        kitchenUIManager.SetOrder(OrderManager.Instance.CurrentOrder);
    }
}
