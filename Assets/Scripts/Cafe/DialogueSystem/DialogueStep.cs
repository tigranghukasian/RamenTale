using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogueStep
{

    [TextArea(3,10)]
    [SerializeField] private string stepText;

    public string StepText { get => stepText; set => stepText = value; }

    public Action OnConfirmStepCustomAction { get; protected set; }
    
    public Action OnBeginStepAction { get; protected set; }
    
    
    public void AddConfirmAction(Action action)
    {
        OnConfirmStepCustomAction += action;
    }
    public void AddBeginStepAction(Action action)
    {
        OnBeginStepAction += action;
    }

}

