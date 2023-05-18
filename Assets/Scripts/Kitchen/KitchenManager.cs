using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenManager : Singleton<KitchenManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image completeButton;

    [SerializeField] private Color completeButtonEnabledColor;
    [SerializeField] private Color completeButtonDisabledColor;

    public Canvas Canvas => canvas;

    public void SetCompleteButton(bool state)
    {
        completeButton.color = state ? completeButtonEnabledColor : completeButtonDisabledColor;
        completeButton.GetComponent<Button>().interactable = state;
    }

    public void CompleteDish()
    {
        SetCompleteButton(false);
        GameManager.Instance.SetView(GameManager.View.Cafe);
    }
}
