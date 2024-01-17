using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogueStep
{

    [TextArea(3,20)]
    [SerializeField] private string stepText;

    public enum BeginStepActionType
    {
        None,
    }

    public enum ConfirmStepActionType
    {
        None,
        GetNextCustomer,
        EnableTutorial,
        SelectHanaPoster
    }

    [SerializeField] private BeginStepActionType beginStepActionFunction = BeginStepActionType.None;
    [SerializeField] private ConfirmStepActionType confirmStepActionFunction = ConfirmStepActionType.None;

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


