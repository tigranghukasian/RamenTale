using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
   [SerializeField] private string id;
   [SerializeField] private Category category;
   [SerializeField] private string itemName;
   [SerializeField] private Sprite sprite;
   [SerializeField] private string itemDescription;
   [SerializeField] private float coinCost;
   [SerializeField] private float diamondCost;
   [SerializeField] private bool isPurchased;
   [SerializeField] private int unlockDay;
   [SerializeField] private string subCategory;
   [SerializeField] private bool isSelected;

   public enum Category
   {
      Ingredient,
      Cosmetic,
      Decoration,
      Upgrade
   };

   public String Id => id;
   public string ItemName => itemName;
   public String ItemDescription => itemDescription;
   public Sprite Sprite => sprite;
   public float CoinCost => coinCost;

   public float DiamondCost => diamondCost;

   public Category ItemCategory => category;

   public int UnlockDay => unlockDay;

   public bool IsPurchased
   {
      get => isPurchased;
      set => isPurchased = value;
   }
   public bool IsSelected
   {
      get => isSelected;
      set => isSelected = value;
   }


   public string Subcategory => subCategory;
   
   // public bool IsUnlocked
   // {
   //    get => isUnlocked;
   //    set => isUnlocked = value;
   // }


   // public ShopItemData CreateItemData()
   // {
   //    ShopItemData itemData = new ShopItemData();
   //    itemData.level = 0;
   //    return itemData;
   // }
}


