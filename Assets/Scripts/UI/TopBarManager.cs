using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBarManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsBalanceText;
    
    private void Awake()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        
    }
    private void UpdateCoinsBalanceText(float amount)
    {
        coinsBalanceText.text = amount.ToString();
    }
}
