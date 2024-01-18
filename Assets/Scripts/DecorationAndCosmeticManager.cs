using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DecorationAndCosmeticManager : MonoBehaviour
{
    [SerializeField] private List<DecorationAndCosmeticGameObject> decorationAndCosmeticGameObjects;

    private void OnEnable()
    {
        if (!GameManager.Instance.FirebaseManager.Authenticated)
        {
            GameManager.Instance.FirebaseManager.OnUserSetup += () =>
            {

                ShopManager.Instance.UpdateShop(UpdateVisuals);
            };
        }
        else
        {
            ShopManager.Instance.UpdateShop(UpdateVisuals);
        }
        
        ShopManager.Instance.OnShopUpdated += UpdateVisuals;
        ShopManager.Instance.OnItemSelected += UpdateVisuals;
        
    }

    private void OnDisable()
    {
        ShopManager.Instance.OnShopUpdated -= UpdateVisuals;
        ShopManager.Instance.OnItemSelected -= UpdateVisuals;
    }

    private void UpdateVisuals(List<ShopItem> shopItems)
    {
        var selectedItems = shopItems
            .Where(item => (item.ItemCategory == ShopItem.Category.Cosmetic || 
                            item.ItemCategory == ShopItem.Category.Decoration) &&
                           item.IsSelected)
            .ToList();
        
        for (int i = 0; i < selectedItems.Count; i++)
        {
            if (selectedItems[i] is ShopItemDecorationAndCosmetic)
            {
                ShopItemDecorationAndCosmetic shopItemDecorationAndCosmetic =
                    (ShopItemDecorationAndCosmetic)selectedItems[i];
                
                var selectedDecorationAndCosmeticGameObject = decorationAndCosmeticGameObjects
                    .FirstOrDefault(item => item.SubcategoryItemName == shopItemDecorationAndCosmetic.Subcategory);
                if (shopItemDecorationAndCosmetic != null && selectedDecorationAndCosmeticGameObject != null)
                {
                    selectedDecorationAndCosmeticGameObject.SetVisual(shopItemDecorationAndCosmetic.DecorationAndCosmeticVisual);
                }
                else
                {
                    Debug.LogError($"DecorationAndCosmeticItem or DecorationAndCosmeticGameObject not found by the subcategory {selectedItems[i].Subcategory}");
                }
            }
            else
            {
                Debug.LogError("Selected Item is just a normal shopItem, not a shopItemDecorationAndCosmetic");
            }
            
            
            
        }
    }
}

[System.Serializable]
public class DecorationAndCosmeticGameObject
{
    [SerializeField] private List<Image> itemVisuals;
    [SerializeField] private string subcategoryItemName;

    public string SubcategoryItemName => subcategoryItemName;

    public void SetVisual(Sprite sprite)
    {
        for (int i = 0; i < itemVisuals.Count; i++)
        {
            if (sprite == null)
            {
                var color = itemVisuals[i].color;
                color.a = 0f;
                itemVisuals[i].color = color;
            }
            else
            {
                var color = itemVisuals[i].color;
                color.a = 1f;
                itemVisuals[i].color = color;
                itemVisuals[i].sprite = sprite;
            }
        }
    }
}
