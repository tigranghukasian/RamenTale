using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject, IRarity
{
    [SerializeField] private List<string> steps;

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

    public string GetStep(int step)
    {
        if (step >= 0 && step < steps.Count)
        {
            return steps[step];
        }
        Debug.LogError("Trying to get a step from dialogue that does not exist");
        return "no step found!";
    }
}
