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

    [SerializeField] private float profitMargin;
    [SerializeField] private float timeToMake;

    public float TimeToMake => timeToMake;
    
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

    public float CalculateProfit()
    {
        float cost = 0;
        cost += soup.price;
        cost += noodle.price;
        for (int i = 0; i < _orderIngredients.Count; i++)
        {
            cost += _orderIngredients[i].amount * _orderIngredients[i].ingredient.price;
        }

        float profit = cost + profitMargin;
        Debug.Log("Profit " + profit);
        return profit;
    }
    
}

[System.Serializable]
public class OrderIngredient
{
    public int amount;
    public Ingredient ingredient;
}
