using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : PersistentSingleton<ShopManager>
{

    [SerializeField] private GameObject shopView;
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
        DisableView();
        for (int i = 0; i < shopItemsList.ShopItems.Count; i++)
        {
            ShopItem shopItem = shopItemsList.ShopItems[i];
            shopItem.IsPurchased = false;
            ShopItemComponent shopItemComponent = Instantiate(shopItemComponentPrefab, unlockableItemsParent)
                .GetComponent<ShopItemComponent>();

            shopItemComponent.Setup(shopItem, UnlockItem);
            shopItemComponent.SetupCard();
            _shopItemComponents.Add(shopItem.Id, new Item
            {
                ShopItem = shopItem,
                component = shopItemComponent
            });

        }

        GameManager.Instance.FirebaseManager.OnUserSetup += UpdateShop;

    }

    public void EnableShopView()
    {
        shopView.gameObject.SetActive(true);
    }

    public void DisableShopView()
    {
        shopView.gameObject.SetActive(false);
        TopBarManager.Instance.UnPause();
    }

    private void DisableView()
    {
        unlockableItemsParent.gameObject.SetActive(false);

    }
    private void EnableView()
    {
        unlockableItemsParent.gameObject.SetActive(true);
    }

    public void UpdateShop()
    {
        GameManager.Instance.FirebaseManager.GetUnlockedItems(UpdateShopData);
    }

    private void UpdateShopData(List<ShopItemData> unlockedShopItems)
    {
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            if (_shopItemComponents.ContainsKey(unlockedShopItems[i].Id))
            {
                Item item = _shopItemComponents[unlockedShopItems[i].Id];
                ShopItemComponent component = item.component;
                
                item.ShopItem.IsPurchased = true;

                component.SetupCard();
            }
        }

        EnableView();
    }

    public void UnlockItem(ShopItem shopItem)
    {
        if (CurrencyManager.Instance.HasCoin(shopItem.CoinCost))
        {
            GameManager.Instance.FirebaseManager.UnlockItem(shopItem);
            CurrencyManager.Instance.SubtractCoins(shopItem.CoinCost);
        }
        UpdateShop();
    }
}
