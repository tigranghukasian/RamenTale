using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoiceInfo
{

    public enum Choice
    {
        NoChoice,
        OptionA,
        OptionB
    }

    [SerializeField] private bool branches;
    [SerializeField] private string optionAText;
    [SerializeField] private string optionBText;

    public string OptionAText => optionAText;
    public string OptionBText => optionBText;
    public bool Branches => branches;
}
