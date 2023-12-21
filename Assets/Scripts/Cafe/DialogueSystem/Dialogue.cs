using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject, IRarity
{
    [SerializeField] private List<DialogueMainPathStep> steps;

    [SerializeField]
    private float rarity;

    public float Rarity => rarity;

    public bool HasStep(int step)
    {
        if (step >= 0 && step < steps.Count)
        {
            return true;
        }

        return false;
    }

    public int StepCount()
    {
        return steps.Count;
    }
    public DialogueStep GetStep(int step)
    {
        if (step >= 0 && step < steps.Count)
        {
            return steps[step];
        }
        Debug.LogError("No step with given index");
        return null;
    }
    
}
