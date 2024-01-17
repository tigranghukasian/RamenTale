using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemComponent : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI costCoinText;
    [SerializeField] private TextMeshProUGUI costDiamondText;
    [SerializeField] private Button purchaseButtonCoin;
    [SerializeField] private Button purchaseButtonDiamond;
    [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI unlocksOnDayText;


    [SerializeField] private GameObject buyButtonCoinGameObject;
    [SerializeField] private GameObject buyButtonDiamondGameObject;

    [Header("State Gameobjects")] 

    [SerializeField] private GameObject purchasedAndSelectedStateGameObject;
    [SerializeField] private GameObject purchasedStateGameObject;
    [SerializeField] private GameObject notPurchasedStateGameObject;
    [SerializeField] private GameObject lockedStateGameObject;



    private enum State
    {
        PurchasedAndSelected,
        Purchased,
        NotPurchased,
        Locked,
    };

    private State _state;

    private ShopItem _shopItem;

    private void SetState(State state)
    {
        _state = state;
        purchasedAndSelectedStateGameObject.SetActive(state == State.PurchasedAndSelected);
        purchasedStateGameObject.SetActive(state == State.Purchased);
        notPurchasedStateGameObject.SetActive(state == State.NotPurchased);
        lockedStateGameObject.SetActive(state == State.Locked);
    }

    public void Setup(ShopItem shopItem, Action<ShopItem> onUnlock, Action<ShopItem> onSelect, ShopManager shopManager)
    {
        _shopItem = shopItem;
        purchaseButtonCoin.onClick.AddListener(() =>
        {
            onUnlock?.Invoke(shopItem);
        });
        purchaseButtonDiamond.onClick.AddListener(() =>
        {
            onUnlock?.Invoke(shopItem);
        });
        selectButton.onClick.AddListener(() =>
        {
            onSelect?.Invoke(shopItem);
        });
        shopManager.OnShopUpdated += _ =>
        {
            UpdateCard();
        };
    }

    public void SetupCard()
    {
        SetState(State.PurchasedAndSelected);
        img.sprite = _shopItem.Sprite;
        title.text = _shopItem.ItemName;
        description.text = _shopItem.ItemDescription;
        unlocksOnDayText.text = $"unlocks on day {_shopItem.UnlockDay}";
        costCoinText.text = _shopItem.CoinCost.ToString("F0");
        costDiamondText.text = _shopItem.DiamondCost.ToString("F0");
        UpdateCard();
    }

    public GameObject GetBuyCoinGameObject()
    {
        return buyButtonCoinGameObject;
    }

    public void UpdateCard()
    {
        if (_shopItem.IsPurchased)
        {
            if (_shopItem.Subcategory == String.Empty || _shopItem.IsSelected)
            {
                SetState(State.PurchasedAndSelected);
            }
            else
            {
                SetState(State.Purchased);
            }
        }
        else
        {
            SetState(State.NotPurchased);
            if (_shopItem.DiamondCost != 0)
            {
                buyButtonDiamondGameObject.SetActive(true);
                buyButtonCoinGameObject.SetActive(false);
            }
            else
            {
                buyButtonDiamondGameObject.SetActive(false);
                buyButtonCoinGameObject.SetActive(true);
            }
        }
        if (_shopItem.UnlockDay > GameManager.Instance.DayNumber)
        {
            SetState(State.Locked);
        }
    }
    
    

    public void LockItem()
    {
        
    }
    
    
    
    
}
