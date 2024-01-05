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
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Button unlockButton;

    [Header("State Gameobjects")] [SerializeField]
    private GameObject purchaseStateGameObject;

    [SerializeField] private GameObject purchasedStateGameObject;
    [SerializeField] private GameObject lockedStateGameObject;
    

    private enum State
    {
        Purchase,
        Purchased,
        Locked
    };

    private State _state;

    private ShopItem _shopItem;

    private void SetState(State state)
    {
        _state = state;
        purchaseStateGameObject.SetActive(state == State.Purchase);
        purchasedStateGameObject.SetActive(state == State.Purchased);
        lockedStateGameObject.SetActive(state == State.Locked);
    }

    public void Setup(ShopItem shopItem, Action<ShopItem> onUnlock)
    {
        _shopItem = shopItem;
        unlockButton.onClick.AddListener(() =>
        {
            onUnlock?.Invoke(shopItem);
        });
    }

    public void SetupCard()
    {
        SetState(State.Purchase);
        img.sprite = _shopItem.Sprite;
        title.text = _shopItem.ItemName;
        description.text = _shopItem.ItemDescription;
        cost.text = _shopItem.CoinCost.ToString("F1");
        Debug.Log("SHOP ITEM STATE " + _shopItem.ItemName + " : " + _shopItem.IsPurchased);
        if (_shopItem.IsPurchased)
        {
            SetState(State.Purchased);
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
