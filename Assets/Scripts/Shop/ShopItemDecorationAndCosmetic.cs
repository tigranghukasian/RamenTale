using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItemDecorationAndCosmetic")]
public class ShopItemDecorationAndCosmetic : ShopItem
{
    [SerializeField] private Sprite decorationAndCosmeticVisual;
    
    public Sprite DecorationAndCosmeticVisual => decorationAndCosmeticVisual;
}
