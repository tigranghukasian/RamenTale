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

    public enum BeginStepActionType
    {
        
    }

    public enum ConfirmStepActionType
    {
        
    }

    [SerializeField] private BeginStepActionType beginStepActionFunction;
    [SerializeField] private ConfirmStepActionType confirmStepActionFunction;

    public string StepText { get => stepText; set => stepText = value; }

    public Action OnConfirmStepCustomAction { get; protected set; }
    
    public Action OnBeginStepAction { get; protected set; }

    public BeginStepActionType BeginStepActionFunction => beginStepActionFunction;
    public ConfirmStepActionType ConfirmStepActionFunction => confirmStepActionFunction;
    
    
    public void AddConfirmAction(Action action)
    {
        OnConfirmStepCustomAction += action;
    }
    public void AddBeginStepAction(Action action)
    {
        OnBeginStepAction += action;
    }

}


