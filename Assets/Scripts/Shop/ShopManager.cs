using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : PersistentSingleton<ShopManager>
{

    [SerializeField] private GameObject shopView;
    [SerializeField] private ShopItemsList shopItemsList;

    [SerializeField] private Transform itemsView;

    [SerializeField] private GameObject shopItemComponentPrefab;

    private Dictionary<string, Item> _shopItemComponents = new Dictionary<string, Item>();
    
    [Header("Categories parents")] 
    [SerializeField] private GameObject ingredientCategoryParent;
    [SerializeField] private GameObject cosmeticCategoryParent;
    [SerializeField] private GameObject decorationCategoryParent;
    [SerializeField] private GameObject upgradeCategoryParent;
    
    public bool IsShopOpen { get; private set; }

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
            
            ShopItem shopItem = Instantiate(shopItemsList.ShopItems[i]);
            
            ShopItemComponent shopItemComponent = Instantiate(shopItemComponentPrefab, 
                    GetCategoryViewObjectFromItemCategory(shopItem.ItemCategory).transform)
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

    private GameObject GetCategoryViewObjectFromItemCategory(ShopItem.Category category)
    {
        switch (category)
        {
            case ShopItem.Category.Ingredient:
                return ingredientCategoryParent;
            case ShopItem.Category.Cosmetic:
                return cosmeticCategoryParent;
            case ShopItem.Category.Decoration:
                return decorationCategoryParent;
            case ShopItem.Category.Upgrade:
                return upgradeCategoryParent;
        }

        return cosmeticCategoryParent;
    }

    public void ChangeCategory(int categoryIndex)
    {
        ShopItem.Category category = (ShopItem.Category)categoryIndex;
        ingredientCategoryParent.SetActive(category == ShopItem.Category.Ingredient);
        cosmeticCategoryParent.SetActive(category == ShopItem.Category.Cosmetic);
        decorationCategoryParent.SetActive(category == ShopItem.Category.Decoration);
        upgradeCategoryParent.SetActive(category == ShopItem.Category.Upgrade);
    }

    public void EnableShopView()
    {
        shopView.gameObject.SetActive(true);
        ChangeCategory(0);
        IsShopOpen = true;
    }

    public void DisableShopView()
    {
        shopView.gameObject.SetActive(false);
        TopBarManager.Instance.UnPause();
        IsShopOpen = false;
    }

    private void DisableView()
    {
        itemsView.gameObject.SetActive(false);

    }
    private void EnableView()
    {
        itemsView.gameObject.SetActive(true);
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
