using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Currency coins = new Currency("Coins");

    public Action<float> OnCoinsChanged;
    

    public void AddCoins(float amount)
    {
        coins.Add(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    public bool HasCoin(float amount)
    {
        return coins.Balance >= amount;
    }

    public void SubtractCoins(float amount)
    {
        coins.Subtract(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    private void Start()
    {
        LoadCurrencyInfo();
        
    }

    private void OnDisable()
    {
        SaveCurrencyInfo();
    }

    private void LoadCurrencyInfo()
    {
        coins.Balance = PlayerPrefs.GetFloat("coins", 100);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    private void SaveCurrencyInfo()
    {
        PlayerPrefs.SetFloat("coins", coins.Balance);
        PlayerPrefs.Save();
    }
}

public class Currency
{
    public string name;
    public float Balance { get; set; }

    public Action OnBalanceZero;

    public Currency(string name)
    {
        this.name = name;
        Balance = 0;;
    }
    

    public void Add(float amount)
    {
        Balance += amount;
    }

    public void Subtract(float amount)
    {
        Balance -= amount;
        if (Balance < 0)
        {
            Balance = 0;
            OnBalanceZero();
        }
    }
}

