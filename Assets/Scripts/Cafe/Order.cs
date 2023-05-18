using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Order")]
public class Order : ScriptableObject, IRarity
{
    [SerializeField]
    private float rarity;

    public float Rarity => rarity;
    
    // public SingleIngredientOld soup;
    // public SingleIngredientOld noodle;
    // public List<MultipleIngredientOld> ingredients;
    [TextArea(4,10)]
    public string orderText;
}
