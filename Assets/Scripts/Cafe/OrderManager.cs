using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : Singleton<OrderManager>
{
    public Order CurrentOrder { get; private set; }


    [SerializeField] private List<Order> availableOrders;

    private void Start()
    {

    }

    public Order GenerateNewOrder()
    {
        
        CurrentOrder = RarityFunctions.GenerateItem(availableOrders);
        return CurrentOrder;
        
    }
    

    public void ServeDish(Dish dish)
    {
        // Debug.Log("SERVE DISH " + CurrentOrder);
        // if (dishOld.CompareWithOrder(CurrentOrder))
        // {
        //     CurrencyManager.Instance.AddCoins(20);
        //     CompleteOrder();
        // }
        // else
        // {
        //     CompleteOrder();
        //     Debug.Log("THE DISH IS WRONG");
        // }
    }

    public void CompleteOrder()
    {
        //GameManager.Instance.DialogueManager.customerImage.gameObject.SetActive(false);
       // GameManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
       // GameManager.Instance.CustomerManager.GetNextCustomer();
        GenerateNewOrder();
    }
}
