using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : PersistentSingleton<ShopManager>
{

    [SerializeField] private GameObject shopView;
    [SerializeField] private ShopItemsList shopItemsList;

    [SerializeField] private Transform itemsView;

    [SerializeField] private GameObject shopItemComponentPrefab;

    private Dictionary<string, Item> _shopItemComponents = new Dictionary<string, Item>();
    private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField] private ScrollRect cardsScrollView;
    
    [Header("Categories parents")] 
    [SerializeField] private GameObject ingredientCategoryParent;
    [SerializeField] private GameObject cosmeticCategoryParent;
    [SerializeField] private GameObject decorationCategoryParent;
    [SerializeField] private GameObject upgradeCategoryParent;

    [Header("Categories buttons")] 
    [SerializeField] private Image ingredientCategoryButtonImage;
    [SerializeField] private Image cosmeticCategoryButtonImage;
    [SerializeField] private Image decorationCategoryButtonImage;
    [SerializeField] private Image upgradeCategoryButtonImage;

    [Header("Button Colors")] 
    [SerializeField] private Color selectedCategoryButtonColor;
    [SerializeField] private Color unSelectedCategoryButtonColor;
    
    
    public Action OnShopUpdated { get; set; }
    public bool IsShopOpen { get; private set; }
    
    private int _callbacksCompleted = 0;

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
            shopItems.Add(shopItem);
            
            ShopItemComponent shopItemComponent = Instantiate(shopItemComponentPrefab, 
                    GetCategoryViewObjectFromItemCategory(shopItem.ItemCategory).transform)
                .GetComponent<ShopItemComponent>();

            shopItemComponent.Setup(shopItem, UnlockItem, SelectItem,this);
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
        RectTransform activeCategoryParent = null;

        ingredientCategoryButtonImage.color = unSelectedCategoryButtonColor;
        cosmeticCategoryButtonImage.color = unSelectedCategoryButtonColor;
        decorationCategoryButtonImage.color = unSelectedCategoryButtonColor;
        upgradeCategoryButtonImage.color = unSelectedCategoryButtonColor;
        
        ingredientCategoryParent.SetActive(false);
        cosmeticCategoryParent.SetActive(false);
        decorationCategoryParent.SetActive(false);
        upgradeCategoryParent.SetActive(false);

        switch (category)
        {
            case ShopItem.Category.Ingredient:
                activeCategoryParent = ingredientCategoryParent.GetComponent<RectTransform>();
                ingredientCategoryButtonImage.color = selectedCategoryButtonColor;
                break;
            case ShopItem.Category.Cosmetic:
                activeCategoryParent = cosmeticCategoryParent.GetComponent<RectTransform>();
                cosmeticCategoryButtonImage.color = selectedCategoryButtonColor;
                break;
            case ShopItem.Category.Decoration:
                activeCategoryParent = decorationCategoryParent.GetComponent<RectTransform>();
                decorationCategoryButtonImage.color = selectedCategoryButtonColor;
                break;
            case ShopItem.Category.Upgrade:
                activeCategoryParent = upgradeCategoryParent.GetComponent<RectTransform>();
                upgradeCategoryButtonImage.color = selectedCategoryButtonColor;
                break;
        }
        
        if (activeCategoryParent != null)
        {
            activeCategoryParent.gameObject.SetActive(true);
            cardsScrollView.content = activeCategoryParent;
        }
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
        _callbacksCompleted = 0;
        GameManager.Instance.FirebaseManager.GetUnlockedItems((unlockedItems) =>
        {
            UpdateUnlockedItems(unlockedItems);
            CheckAndInvokeOnShopUpdated();
        });
        GameManager.Instance.FirebaseManager.GetShopItemSubcategorySelected((subcategorySelected) =>
        {
            GetShopItemSubCategorySelected(subcategorySelected);
            CheckAndInvokeOnShopUpdated();
        });
    }
    private void CheckAndInvokeOnShopUpdated()
    {
        _callbacksCompleted++;

        if (_callbacksCompleted == 2) // Check if both callbacks have been completed
        {
            OnShopUpdated?.Invoke();
            EnableView();
        }
    }

    private void UpdateUnlockedItems(List<ShopItemData> unlockedShopItems)
    {
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            if (_shopItemComponents.ContainsKey(unlockedShopItems[i].Id))
            {
                Item item = _shopItemComponents[unlockedShopItems[i].Id];
                ShopItemComponent component = item.component;
                
                item.ShopItem.IsPurchased = true;

                component.UpdateCard();
            }
        }
    }

    private void GetShopItemSubCategorySelected(List<ShopItemSubcategorySelectedData> unlockedShopItems)
    {
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            var matchingShopItem = shopItems.FirstOrDefault(si => si.Id == unlockedShopItems[i].Id);

            if (matchingShopItem != null)
            {   
                SelectItem(matchingShopItem);
            }
            else
            {
                Debug.LogError($"An item that is selected in the subcategory {unlockedShopItems[i]} was not found");
            }
        }

        
    }

    public void UnlockItem(ShopItem shopItem)
    {
        if (CurrencyManager.Instance.HasCoin(shopItem.CoinCost))
        {
            GameManager.Instance.FirebaseManager.UnlockItem(shopItem);
            CurrencyManager.Instance.SubtractCoins(shopItem.CoinCost);
        }
        
        //TODO: CHECK IF THIS CALL IS NEEDED
        //MAYBE THIS CALL IS NOT NEEDED
        UpdateShop();
    }

    public void SelectItem(ShopItem shopItem)
    {
        foreach (var item in shopItems.Where(si => si.Subcategory == shopItem.Subcategory))
        {
            item.IsSelected = false;
            _shopItemComponents[item.Id].component.UpdateCard();
        }
        shopItem.IsSelected = true;
        _shopItemComponents[shopItem.Id].component.UpdateCard();
        
        GameManager.Instance.FirebaseManager.SelectItem(shopItem);
    }
}
