using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenManager : Singleton<KitchenManager>
{
    [SerializeField] private Canvas dragCanvas;
    [SerializeField] private Button completeButton;
    [SerializeField] private PlateSpot plateSpot;

    [SerializeField] private Color completeButtonEnabledColor;
    [SerializeField] private Color completeButtonDisabledColor;
    [SerializeField] private KitchenUIManager kitchenUIManager;
    
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private Transform ingredientsUnlockableParent;
    [SerializeField] private GameObject ingredientBoxPrefab;
    [SerializeField] private Transform defaultIngredientBoxesParent;
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private Transform pagesParent;
    [SerializeField] private Button leftPageButton;
    [SerializeField] private Button rightPageButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Knife knife;
    private List<GameObject> _pages = new List<GameObject>();
    private bool _unlockedItemsIsSetup = false;
    
    private int _currentPageIndex = 0;

    private Image completeButtonImage;

    private Dictionary<Ingredient, IngredientBox> ingredientBoxes = new Dictionary<Ingredient, IngredientBox>();

    private List<IngredientBox> ingredientBoxesList = new List<IngredientBox>();

    public Dictionary<Ingredient, IngredientBox> IngredientBoxes => ingredientBoxes;

    public KitchenUIManager UIManager => kitchenUIManager;

    public PlateSpot PlateSpot => plateSpot;

    public Canvas DragCanvas => dragCanvas;
    public Knife Knife => knife;

    private void Start()
    {
        completeButtonImage = completeButton.GetComponent<Image>();
        GetDefaultIngredientBoxes();
        if (!GameManager.Instance.FirebaseManager.Authenticated)
        {
            GameManager.Instance.FirebaseManager.OnUserSetup += SetupUnlockedItems;
        }
        else
        {
            SetupUnlockedItems();
        }

        cancelButton.onClick.AddListener(() =>
        {
            PopupManager.Instance.ClearDishPopup();
        });
        GameSceneManager.Instance.TutorialManager.OnTutorialStarted += DisableButtons;
        GameSceneManager.Instance.TutorialManager.OnTutorialEnded += EnableButtons;
        GameSceneManager.Instance.TutorialManager.AddAdditionalEventToStep("Complete", EnableCompleteButton);
    }

    private void GetDefaultIngredientBoxes()
    {
        IngredientBox[] defaultIngredientBoxes =
            defaultIngredientBoxesParent.GetComponentsInChildren<IngredientBox>();
        for (int i = 0; i < defaultIngredientBoxes.Length; i++)
        {
            ingredientBoxes.Add(defaultIngredientBoxes[i].IngredientData, defaultIngredientBoxes[i]);
            ingredientBoxesList.Add(defaultIngredientBoxes[i]);
        }
    }

    public void DisableButtons()
    {
        leftPageButton.interactable = false;
        rightPageButton.interactable = false;
        completeButton.interactable = false;
        cancelButton.interactable = false;
    }

    public void EnableButtons()
    {
        leftPageButton.interactable = _currentPageIndex > 0;
        rightPageButton.interactable = _currentPageIndex < _pages.Count - 1;
        cancelButton.interactable = true;
        completeButton.interactable = true;
    }

    public void EnableCompleteButton()
    {
        completeButton.interactable = true;
    }

    public void CancelDish()
    {
        Destroy(PlateSpot.CurrentDish.gameObject);
        PlateSpot.AddEmptyPlate(true);
    }
    public void SetCompleteButton(bool state)
    {
        completeButtonImage.color = state ? completeButtonEnabledColor : completeButtonDisabledColor;
        completeButton.interactable = state;
    }

    public void SetupUnlockedItems()
    {
        if (!_unlockedItemsIsSetup)
        {
            GameManager.Instance.FirebaseManager.GetUnlockedItems(OnUnlockedItemsReceived);
        }
           
    }

    private void OnUnlockedItemsReceived(List<ShopItemData> unlockedShopItems)
    {
        List<Ingredient> unlockedIngredients = new List<Ingredient>();
        Debug.Log("Update Kitchen Items");
        for (int i = 0; i < unlockedShopItems.Count; i++)
        {
            Ingredient ingredient = ingredients.Find(ing => ing.name == unlockedShopItems[i].Id);
            unlockedIngredients.Add(ingredient);
        }
        
        for (int i = 0; i < unlockedIngredients.Count; i++)
        {
            IngredientBox box = Instantiate(ingredientBoxPrefab).GetComponent<IngredientBox>();
            box.IngredientData = unlockedIngredients[i];
            ingredientBoxes.Add(unlockedIngredients[i], box);
            ingredientBoxesList.Add(box);
        }

        _unlockedItemsIsSetup = true;

        SetupPages();
    }
    private void SetupPages()
    {
        // First, clear existing pages if any
        foreach (var page in _pages)
        {
            Destroy(page);
        }
        _pages.Clear();

        // Calculate the total number of pages needed
        int totalIngredientsCount = ingredientBoxes.Count;
        int totalPages = Mathf.CeilToInt(totalIngredientsCount / 4.0f);

        // Create and populate pages
        for (int i = 0; i < totalPages; i++)
        {
            GameObject page = Instantiate(pagePrefab, pagesParent);
            _pages.Add(page);

            // Determine the range of ingredients for this page
            int startIdx = i * 4;
            int endIdx = Mathf.Min(startIdx + 4, totalIngredientsCount);

            for (int j = startIdx; j < endIdx; j++)
            {
                ingredientBoxesList[j].transform.SetParent(page.transform);
                ingredientBoxesList[j].transform.localScale = Vector3.one;
            }
        }

        defaultIngredientBoxesParent.gameObject.SetActive(false);
        UpdatePageVisibility();
    }
    
    public void OnLeftPageButtonPressed()
    {
        if (_currentPageIndex > 0)
        {
            _currentPageIndex--;
            UpdatePageVisibility();
        }
    }

    public void OnRightPageButtonPressed()
    {
        if (_currentPageIndex < _pages.Count - 1)
        {
            _currentPageIndex++;
            UpdatePageVisibility();
        }
    }
    
    private void UpdatePageVisibility()
    {
        for (int i = 0; i < _pages.Count; i++)
        {
            _pages[i].SetActive(i == _currentPageIndex);
        }
        
        //Change button color
        leftPageButton.interactable = _currentPageIndex > 0;
        rightPageButton.interactable = _currentPageIndex < _pages.Count - 1;
        
    }

    public void CompleteDish()
    {
        SetCompleteButton(false);
        plateSpot.RemovePlate();
        GameSceneManager.Instance.CompleteDish(plateSpot.CurrentDish);
        GameSceneManager.Instance.OpenCafe();
        if (GameManager.Instance.IsTutorialActive)
        {
            GameSceneManager.Instance.TutorialManager.CompleteAction("Complete");
        }
        
    }

    public void SetOrder()
    {
        kitchenUIManager.SetOrder(OrderManager.Instance.CurrentOrder);
    }
}
