using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMainPathStep : DialogueStep{

    [SerializeField] private DialogueChoiceInfo _choiceInfo;

    [SerializeField] private List<DialogueBranchStep> pathA = new List<DialogueBranchStep>();
    [SerializeField] private List<DialogueBranchStep> pathB = new List<DialogueBranchStep>();
    
    
    public List<DialogueBranchStep> PathA => pathA;
    public List<DialogueBranchStep> PathB => pathB;

    public string GetOptionAText()
    {
        return _choiceInfo.OptionAText;
    }
    public string GetOptionBText()
    {
        return _choiceInfo.OptionBText;
    }

    public bool Branches()
    {
        return _choiceInfo.Branches;
    }
}

