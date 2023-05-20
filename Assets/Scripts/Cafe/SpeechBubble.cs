using System;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.DialogueManager.NextStep();
        }
    }
    
    
}
