using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private TextMeshProUGUI buttonAText;
    [SerializeField] private TextMeshProUGUI buttonBText;
    [SerializeField] private Image tapArea;
    private bool optionsEnabled = false;

    public void SetText(string message) {
        text.text = message;
    }
    

    public void OnTapAreaPressed()
    {
        if (!optionsEnabled)
        {
            GameSceneManager.Instance.DialogueManager.NextStep();
        }
    }

    public void EnableOptionsButtons()
    {
        optionsEnabled = true;
        buttonsParent.SetActive(true);
    }

    public void DisableOptionsButtons()
    {
        optionsEnabled = false;
        buttonsParent.SetActive(false);
        SetTapAreaActive(true);
    }

    public void SetButtons(string optionAText, string optionBText)
    {
        buttonAText.text = optionAText;
        buttonBText.text = optionBText;
        SetTapAreaActive(false);

    }

    public void SetTapAreaActive(bool active)
    {
        tapArea.gameObject.SetActive(active);
    }

    public void OptionA()
    {
        GameSceneManager.Instance.DialogueManager.NextStep(DialogueChoiceInfo.Choice.OptionA);
    }

    public void OptionB()
    {
        GameSceneManager.Instance.DialogueManager.NextStep(DialogueChoiceInfo.Choice.OptionB);
    }
    
    
}
