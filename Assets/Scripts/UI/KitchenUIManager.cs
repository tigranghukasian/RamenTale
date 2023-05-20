using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KitchenUIManager : MonoBehaviour
{

    [SerializeField] private GameObject orderTicket;
    [SerializeField] private Animator orderTicketAnimator;
    [SerializeField] private TextMeshProUGUI orderText;
    private bool _isOrderTicketOpen;

    
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && orderTicket.activeSelf && _isOrderTicketOpen)
        {
            orderTicketAnimator.Play("ticket_close");
            _isOrderTicketOpen = false;
            //orderTicket.SetActive(false);
        }
    }

    public void SetOrder(Order order)
    {
        var text = string.Empty;
        text += $"{order.Soup.name} soup\n";
        text += $"{order.Noodle.name} noodle\n";
        for (int i = 0; i < order.OrderIngredients.Count; i++)
        {
            text += $"{order.OrderIngredients[i].amount} {order.OrderIngredients[i].ingredient.name} \n";
        }

        orderText.text = text;
    }

    public void OpenOrder()
    {
        orderTicket.SetActive(true);
        _isOrderTicketOpen = true;
        orderTicketAnimator.Play("ticket_open");
    }

   
}
