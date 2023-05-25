using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : Singleton<OrderManager>
{
    public Order CurrentOrder { get; set; }


    [SerializeField] private List<Order> availableOrders;
    [SerializeField] private string correctOrderText;
    [SerializeField] private string incorrectOrderText;
    

    public Order GenerateNewOrder()
    {
        CurrentOrder = RarityFunctions.GenerateItem(availableOrders);
        return CurrentOrder;
        
    }
    

    public void ServeDish(Dish dish)
    {
        if(CurrentOrder.CompareWithDish(dish))
        {
            CurrencyManager.Instance.AddCoins(CurrentOrder.CalculateProfit());
            CompleteOrder(true);
        }
        else
        {
            CompleteOrder(false);
        }
    }


    private void CompleteOrder(bool isCorrect)
    {
        var feedbackText = isCorrect ? correctOrderText : incorrectOrderText;
        GameSceneManager.Instance.DialogueManager.AddFeedback(feedbackText);
        GameSceneManager.Instance.DialogueManager.NextStep();
    }
}
