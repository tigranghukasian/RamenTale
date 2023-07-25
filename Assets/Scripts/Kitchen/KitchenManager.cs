using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenManager : Singleton<KitchenManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image completeButton;
    [SerializeField] private PlateSpot plateSpot;

    [SerializeField] private Color completeButtonEnabledColor;
    [SerializeField] private Color completeButtonDisabledColor;
    [SerializeField] private KitchenUIManager kitchenUIManager;

    [SerializeField] private RectTransform middleTable;
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private Transform ingredientsUnlockableParent;
    [SerializeField] private GameObject ingredientBoxPrefab;
    [SerializeField] private Transform defaultIngredientBoxesParent;

    private Dictionary<Ingredient, IngredientBox> ingredientBoxes = new Dictionary<Ingredient, IngredientBox>();

    public Dictionary<Ingredient, IngredientBox> IngredientBoxes => ingredientBoxes;

    public KitchenUIManager UIManager => kitchenUIManager;

    public PlateSpot PlateSpot => plateSpot;

    public Canvas Canvas => canvas;

    private void Start()
    {
        GetUnlockedItems();
        GetDefaultIngredientBoxes();
    }

    private void GetDefaultIngredientBoxes()
    {
        IngredientBox[] defaultIngredientBoxes =
            defaultIngredientBoxesParent.GetComponentsInChildren<IngredientBox>();
        for (int i = 0; i < defaultIngredientBoxes.Length; i++)
        {
            ingredientBoxes.Add(defaultIngredientBoxes[i].IngredientData, defaultIngredientBoxes[i]);
        }
    }
    public void SetCompleteButton(bool state)
    {
        completeButton.color = state ? completeButtonEnabledColor : completeButtonDisabledColor;
        completeButton.GetComponent<Button>().interactable = state;
    }

    private void GetUnlockedItems()
    {
        GameManager.Instance.FirebaseManager.GetUnlockedItems(OnUnlockedItemsReceived);
    }

    private void OnUnlockedItemsReceived(List<ShopItemData> unlockedShopItems)
    {
        List<Ingredient> unlockedIngredients = new List<Ingredient>();
        Debug.Log("///Update kitchen Data" + unlockedShopItems.Count);
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            Ingredient ingredient = ingredients.Find(ing => ing.name == unlockedShopItems[i].Id);
            unlockedIngredients.Add(ingredient);
        }

        if (unlockedIngredients.Count <= 3)
        {
            middleTable.sizeDelta = new Vector2(692f, middleTable.sizeDelta.y);
        }
        if (unlockedIngredients.Count > 3 && unlockedIngredients.Count <= 6)
        {
            middleTable.sizeDelta = new Vector2(804f, middleTable.sizeDelta.y);
        }

        for (int i = 0; i < unlockedIngredients.Count; i++)
        {
            IngredientBox box = Instantiate(ingredientBoxPrefab, ingredientsUnlockableParent).GetComponent<IngredientBox>();
            box.IngredientData = unlockedIngredients[i];
            ingredientBoxes.Add(unlockedIngredients[i], box);
        }
    }

    public void CompleteDish()
    {
        SetCompleteButton(false);
        GameSceneManager.Instance.CompleteDish(plateSpot.CurrentDish);
        GameSceneManager.Instance.OpenCafe();
    }

    public void SetOrder()
    {
        kitchenUIManager.SetOrder(OrderManager.Instance.CurrentOrder);
    }
}
