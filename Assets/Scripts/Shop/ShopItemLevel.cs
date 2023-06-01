using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItemLevel 
{
    [SerializeField] private string id;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string itemDescription;
    [SerializeField] private float coinCost;

    public string Id => id;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public Sprite Sprite => sprite;
    public float CoinCost => coinCost;
}
