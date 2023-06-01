using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : ScriptableObject
{
   [SerializeField] private string id;
   [SerializeField] private string itemName;
   [SerializeField] private Sprite sprite;
   [SerializeField] private string itemDescription;
   [SerializeField] private float coinCost;
   [SerializeField] private bool isUnlocked;

   public String Id => id;
   public string ItemName => itemName;
   public String ItemDescription => itemDescription;
   public Sprite Sprite => sprite;
   public float CoinCost => coinCost;

   public bool IsUnlocked => isUnlocked;
   

   // public ShopItemData CreateItemData()
   // {
   //    ShopItemData itemData = new ShopItemData();
   //    itemData.level = 0;
   //    return itemData;
   // }
}


