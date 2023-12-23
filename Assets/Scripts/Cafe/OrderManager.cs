using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            float profit = CurrentOrder.CalculateProfit();
            CurrencyManager.Instance.AddCoins(profit);
            GameManager.Instance.RevenueToday += profit;
            CompleteOrder(true);
        }
        else
        {
            CompleteOrder(false);
        }
        if (GameManager.Instance.IsTutorialActive)
        {
            GameSceneManager.Instance.TutorialManager.CompleteAction("ServeDish");
        }
        
    }


    private void CompleteOrder(bool isCorrect)
    {
        var feedbackText = isCorrect ? correctOrderText : incorrectOrderText;
        GameSceneManager.Instance.DialogueManager.AddFeedback(feedbackText);
        GameSceneManager.Instance.DialogueManager.NextStep();
    }
}
