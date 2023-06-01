using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private ShopItemsList shopItemsList;

    [SerializeField] private Transform unlockableItemsParent;

    [SerializeField] private GameObject shopItemComponentPrefab;

    private Dictionary<string, Item> _shopItemComponents = new Dictionary<string, Item>();

    public struct Item
    {
        public ShopItem ShopItem;
        public ShopItemComponent component;
    };
    private void Start()
    {
        GameManager.Instance.FirebaseManager.OnUnlockedShopItemDatasReceived += UpdateShopData;
        for (int i = 0; i < shopItemsList.ShopItems.Count; i++)
        {
            ShopItem shopItem = shopItemsList.ShopItems[i];
            ShopItemComponent shopItemComponent = Instantiate(shopItemComponentPrefab, unlockableItemsParent)
                .GetComponent<ShopItemComponent>();
            if (shopItem is ShopItemUpgradeable shopItemUpgradeable)
            {
                shopItemUpgradeable.SetLevel(0);
            }
            
            shopItemComponent.Setup(shopItem, UnlockItem);
            shopItemComponent.SetupCard();
            unlockableItemsParent.gameObject.SetActive(false);
            _shopItemComponents.Add(shopItem.Id, new Item
            {
                ShopItem = shopItem,
                component = shopItemComponent
            });

        }
        UpdateShop();
    }

    private void EnableView()
    {
        unlockableItemsParent.gameObject.SetActive(true);
    }

    public void UpdateShop()
    {
        GameManager.Instance.FirebaseManager.GetUnlockedItems(EnableView);
    }

    private void UpdateShopData(List<ShopItemData> unlockedShopItems)
    {
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            if (_shopItemComponents.ContainsKey(unlockedShopItems[i].Id))
            {
                Item item = _shopItemComponents[unlockedShopItems[i].Id];
                ShopItemComponent component = item.component;

                if (unlockedShopItems[i].Level > 0 )
                {
                    if (item.ShopItem is ShopItemUpgradeable shopItemUpgradeable)
                    {
                        shopItemUpgradeable.SetLevel(unlockedShopItems[i].Level);
                    }
                }
                component.SetupCard();
            }
        }
    }

    public void UnlockItem(ShopItem shopItem)
    {
        Debug.Log(shopItem);
        if (shopItem is ShopItemUpgradeable shopItemUpgradeable)
        {
            if (Utilities.IsIndexValid(shopItemUpgradeable.Levels, shopItemUpgradeable.CurrentLevel))
            {
                if (CurrencyManager.Instance.HasCoin(shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].CoinCost))
                {
                    shopItemUpgradeable.LevelUp();
                    Debug.Log("shop manager unlock item " + shopItemUpgradeable.CurrentLevel);
                    GameManager.Instance.FirebaseManager.UnlockItem(shopItemUpgradeable, shopItemUpgradeable.CurrentLevel);
                    CurrencyManager.Instance.SubtractCoins(shopItemUpgradeable.Levels[shopItemUpgradeable.CurrentLevel].CoinCost);
                }
            }
        }
        else 
        {
            if (CurrencyManager.Instance.HasCoin(shopItem.CoinCost))
            {
                GameManager.Instance.FirebaseManager.UnlockItem(shopItem, 0);
                CurrencyManager.Instance.SubtractCoins(shopItem.CoinCost);
            }

        }
        UpdateShop();
    }
}
