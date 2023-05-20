using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsBalanceText;
    [SerializeField] private GameObject orderTicket;
    [SerializeField] private Animator orderTicketAnimator;
    private bool _isOrderTicketOpen;

    private void Awake()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && orderTicket && orderTicket.activeSelf)
        {
            orderTicketAnimator.Play("ticket_close");
            _isOrderTicketOpen = false;
            //orderTicket.SetActive(false);
        }
    }

    public void OpenOrder()
    {
        orderTicket.SetActive(true);
        _isOrderTicketOpen = true;
        orderTicketAnimator.Play("ticket_open");
    }

    private void UpdateCoinsBalanceText(float amount)
    {
        coinsBalanceText.text = amount.ToString();
    }
}
