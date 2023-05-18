using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Currency coins = new Currency("Coins");

    public Action<int> OnCoinsChanged;
    

    public void AddCoins(int amount)
    {
        coins.Add(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    public void SubtractCoins(int amount)
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
        coins.Balance = PlayerPrefs.GetInt("coins", 100);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    private void SaveCurrencyInfo()
    {
        PlayerPrefs.SetInt("coins", coins.Balance);
        PlayerPrefs.Save();
    }
}

public class Currency
{
    public string name;
    public int Balance { get; set; }

    public Currency(string name)
    {
        this.name = name;
        Balance = 0;;
    }

    public void Add(int amount)
    {
        Balance += amount;
    }

    public void Subtract(int amount)
    {
        Balance -= amount;
    }
}

