using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CurrencyManager : PersistentSingleton<CurrencyManager>
{
    private Currency coins = new Currency("Coins");

    public Action<float> OnCoinsChanged;
    public Action<float> OnCoinIncreased;
    public Action<float> OnCoinDecreased;

    private float _revenue;
    private float _tips;
    private float _suppliesUsed;
    private float _totalProfit;
    

    [Button]
    public void Give100Coins()
    {
        AddCoins(100);
    }

    public void AddCoins(float amount)
    {
        coins.Add(amount);
        OnCoinIncreased?.Invoke(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
        AudioManager.Instance.PlayCashIncomeClip();
    }

    public bool HasCoin(float amount)
    {
        return true;
        return coins.Balance >= amount;
    }

    public void SubtractCoins(float amount)
    {
        coins.Subtract(amount);
        OnCoinDecreased?.Invoke(amount);
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

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        SaveCurrencyInfo();
    }

    private void LoadCurrencyInfo()
    {
        coins.Balance = PlayerPrefs.GetFloat(StringConstants.SAVE_COINS, 100);
        OnCoinsChanged?.Invoke(coins.Balance);
    }

    private void SaveCurrencyInfo()
    {
        PlayerPrefs.SetFloat(StringConstants.SAVE_COINS, coins.Balance);
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

