using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Sirenix.OdinInspector;
using UnityEngine;

public class CurrencyManager : PersistentSingleton<CurrencyManager>
{
    private Currency coins = new Currency("Coins");
    private Currency diamonds = new Currency("Diamonds");

    public Action<float> OnCoinsChanged;
    public Action<float> OnCoinIncreased;
    public Action<float> OnCoinDecreased;
    
    public Action<float> OnDiamondsChanged;
    public Action<float> OnDiamondsIncreased;
    public Action<float> OnDiamondsDecreased;

    private float _revenue;
    private float _tips;
    private float _suppliesUsed;
    private float _totalProfit;

    private bool _hasShownNegativeMoneyPopup;

    public float CoinBalance
    {
        get => coins.Balance;
        set
        {
            coins.Balance = value;
            OnCoinsChanged?.Invoke(coins.Balance);
        }
    }
    
    public float DiamondsBalance
    {
        get => diamonds.Balance;
        set
        {
            diamonds.Balance = value;
            OnDiamondsChanged?.Invoke(diamonds.Balance);
        }
    }


    [Button]
    public void Give100Coins()
    {
        AddCoins(100);
    }

    //COINS ----------------------------------------------------------------
    
    public void AddCoins(float amount)
    {
        coins.Add(amount);
        OnCoinIncreased?.Invoke(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
        AudioManager.Instance.PlayCashIncomeClip();
    }

    public bool HasCoin(float amount, bool canHaveDebt = false)
    {
        if (canHaveDebt)
        {
            return coins.Balance - amount >= -50f;
        }
        return coins.Balance - amount >= 0;

    }
    public void SubtractCoins(float amount)
    {
        coins.Subtract(amount);
        if (coins.Balance <= 0 && !_hasShownNegativeMoneyPopup)
        {
            PopupManager.Instance.NegativeMoneyPopup();
            _hasShownNegativeMoneyPopup = true;
        }
        if (coins.Balance <= -50)
        {
            PopupManager.Instance.OutOfMoneyPopup();
        }
        
        OnCoinDecreased?.Invoke(amount);
        OnCoinsChanged?.Invoke(coins.Balance);
        
    }
    
    //DIAMONDS ----------------------------------------------------------------
    
    public void AddDiamonds(float amount)
    {
        diamonds.Add(amount);
        OnDiamondsIncreased?.Invoke(amount);
        OnDiamondsChanged?.Invoke(diamonds.Balance);
        //AudioManager.Instance.PlayCashIncomeClip();
    }

    public bool HasDiamonds(float amount, bool canHaveDebt = false)
    {
        return diamonds.Balance - amount >= 0;

    }
    public void SubtractDiamonds(float amount)
    {
        diamonds.Subtract(amount);
        if (diamonds.Balance <= 0)
        {
            Debug.LogError("DIAMONDS NEGATIVE");
        }
        
        OnDiamondsDecreased?.Invoke(amount);
        OnDiamondsChanged?.Invoke(diamonds.Balance);
        
    }
    

    private void Start()
    {
        LoadCurrencyInfo();
        GameManager.Instance.FirebaseManager.AuthManager.OnUserAuthenticated += UpdateCurrencyInfo;
    }

    private void UpdateCurrencyInfo(FirebaseUser user)
    {
        
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
            OnBalanceZero?.Invoke();
        }
    }
}

