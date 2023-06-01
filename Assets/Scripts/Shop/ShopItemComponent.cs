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

    private ShopItem _shopItem;

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
        if (!_shopItem.IsUnlocked)
        {
            
        }
        if (_shopItem is ShopItemUpgradeable shopItemUpgradeable)
        {
            img.sprite = shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].Sprite;
            title.text = shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].ItemName;
            description.text = shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].ItemDescription;
            cost.text = shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].CoinCost.ToString("F1");
        }
        else
        {
            img.sprite = _shopItem.Sprite;
            title.text = _shopItem.ItemName;
            description.text = _shopItem.ItemDescription;
            cost.text = _shopItem.CoinCost.ToString("F1");
        }
    }

    public void LockItem()
    {
        
    }
    
    
    
    
}
