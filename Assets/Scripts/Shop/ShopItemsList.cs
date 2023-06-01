using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItemsList")]
public class ShopItemsList : ScriptableObject
{
    [SerializeField] private List<ShopItem> _shopItems;

    public List<ShopItem> ShopItems => _shopItems;
}
