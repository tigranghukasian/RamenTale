using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientComponent : Moveable
{
    [SerializeField] private Ingredient ingredientData;
    public Ingredient Ingredient { get; set; }
    
    public bool IsCut { get; set; }

    public void Init()
    {
        Ingredient = Instantiate(ingredientData);
    }
}
