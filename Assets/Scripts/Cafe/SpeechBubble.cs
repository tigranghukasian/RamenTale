using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string message) {
        text.text = message;
    }

    public void OnTap()
    {
        GameManager.Instance.DialogueManager.NextStep();
    }
    
}
