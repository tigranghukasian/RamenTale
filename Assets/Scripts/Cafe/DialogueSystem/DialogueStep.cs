using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogueStep
{
    [SerializeField] private DialogueStepType _stepType;

    [TextArea(3,10)]
    [SerializeField] private string stepText;

    public string StepText { get => stepText; set => stepText = value; }
    public DialogueStepType StepType { get => _stepType; set => _stepType = value; }

    public Action OnConfirmStepCustomAction { get; protected set; }

    // public DialogueStep(string text)
    // {
    //     StepText = text;
    // }


}
public enum DialogueStepType { Speech, Order, Feedback}
