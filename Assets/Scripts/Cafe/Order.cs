using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Order")]
public class Order : ScriptableObject, IRarity
{
    [SerializeField]
    private float rarity;

    [TextArea(4,10)]
    public string orderText;

    public float Rarity => rarity;

    [SerializeField]
    private Soup soup;

    [SerializeField]
    private Noodle noodle;

    [SerializeField]
    private List<OrderIngredient> _orderIngredients = new List<OrderIngredient>();

    public List<OrderIngredient> OrderIngredients => _orderIngredients;
    public Noodle Noodle => noodle;
    public Soup Soup => soup;
    
    public bool CompareWithDish(Dish dish)
    {
        DishData data = dish.DishDataBuilder.Build();
        if (data.Soup != soup || data.Noodle != noodle)
            return false;
        
        List<Ingredient> dishIngredients = data.Ingredients;
        
        int orderIngredientCount = _orderIngredients.Sum(o => o.amount);
        if (orderIngredientCount != data.Ingredients.Count)
        {
            return false;
        }
        
        foreach (var orderIngredient in _orderIngredients)
        {
            // Get the count of this ingredient in the dish
            int dishIngredientCount = data.Ingredients.Count(ingredient => ingredient == orderIngredient.ingredient);

            // If the counts don't match, the Order doesn't match the Dish
            if (dishIngredientCount != orderIngredient.amount)
            {
                return false;
            }
        }

        // If no differences were found, the Order matches the Dish
        return true;
    }
    
}

[System.Serializable]
public class OrderIngredient
{
    public int amount;
    public Ingredient ingredient;
}
