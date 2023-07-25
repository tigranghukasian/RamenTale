using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Ingredient")]
public class Ingredient : Part
{
    public Sprite sprite;
    public bool isCut;
    public bool isCuttable;
    [ShowIf("isCuttable")]
    public Ingredient CutVersion;
    [HideIf("isCut")]
    public int cutParts;
}
