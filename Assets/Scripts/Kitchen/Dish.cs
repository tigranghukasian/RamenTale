using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dish : Moveable, IDropHandler
{
    [SerializeField] private Image soupImage;
    [SerializeField] private GameObject soupDropChecker;
    [SerializeField] private Image noodleImage;
    [SerializeField] private Transform ingredientsParent;

    [SerializeField] private Animator soupAnimator;

    private DishData.Builder _dishDataBuilder;
    public DishData.Builder DishDataBuilder => _dishDataBuilder;

    public void Init()
    {
        _dishDataBuilder = new DishData.Builder();
        _dishDataBuilder.OnSoupSet += SetSoupVisuals;
        _dishDataBuilder.OnNoodleSet += SetNoodleVisuals;
        _dishDataBuilder.OnIngredientAdded += UpdateIngredientsOrder;
    }

    public void OnPartDropped(GameObject pointerDrag, bool onSoup = false)
    {
        if (pointerDrag.TryGetComponent(out IngredientBox ingredientBox) && onSoup)
        {
            if (!ingredientBox.IngredientData.isCuttable || ingredientBox.IngredientData.isCut)
            {
                AddIngredient(ingredientBox.SpawnedComponent);
            }
        }
        if (pointerDrag.TryGetComponent(out IngredientBoxCut ingredientBoxCut) && onSoup)
        {
            if (ingredientBoxCut.HasAmount(1))
            {
                AddIngredient(ingredientBoxCut.SpawnedComponent);
                ingredientBoxCut.Decrease(1);
            }
        }
        if (pointerDrag.TryGetComponent(out IngredientComponent ingredientComponent) && onSoup)
        {
            AddIngredient(ingredientComponent);
            ingredientComponent.GetComponent<Image>().raycastTarget = false;
            ingredientComponent.GetComponent<DraggableIngredientComponent>().Placed = true;
        }
        if (pointerDrag.TryGetComponent(out PartPlacer partPlacer))
        {
            if (partPlacer.Part is Soup && !_dishDataBuilder.HasSoup())
            {
                Soup soup = (Soup)partPlacer.Part;
                if (CurrencyManager.Instance.HasCoin(soup.price))
                {
                    _dishDataBuilder.SetSoup(soup);
                    AudioManager.Instance.PlayDishSoupPourClip();
                    GameManager.Instance.SuppliesUsedToday += soup.price;
                    CurrencyManager.Instance.SubtractCoins(soup.price);
                }
                
            }
            else if (partPlacer.Part is Noodle && _dishDataBuilder.HasSoup() && !_dishDataBuilder.HasNoodle())
            {
                Noodle noodle = (Noodle)partPlacer.Part;
                if (CurrencyManager.Instance.HasCoin(noodle.price))
                {
                    _dishDataBuilder.SetNoodle(noodle);
                    GameManager.Instance.SuppliesUsedToday += noodle.price;
                    CurrencyManager.Instance.SubtractCoins(noodle.price);
                }

                
            }
            
        }
    }

    private void AddIngredient(IngredientComponent ingredientComponent)
    {
        _dishDataBuilder.AddIngredient(ingredientComponent.IngredientData);
        ingredientComponent.transform.SetParent(ingredientsParent);
        ingredientComponent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ingredientComponent.Placed = true;
        UpdateIngredientsOrder();
        AudioManager.Instance.PlayDishIngredientAddClip();
    }

    public void SetChildrenAnchorsToCorners()
    {
        RectTransform parentRect = transform as RectTransform;

        foreach (Transform child in ingredientsParent)
        {
            RectTransform childRect = child as RectTransform;
            
            if (childRect != null)
            {

                // Vector2 newAnchorsMin = new Vector2(childRect.localPosition.x / parentRect.rect.width + .5f - childRect.rect.width / parentRect.rect.width * childRect.pivot.x, 
                //     childRect.localPosition.y / parentRect.rect.height + .5f - childRect.rect.height / parentRect.rect.height * childRect.pivot.y);
                // Vector2 newAnchorsMax = new Vector2(childRect.localPosition.x / parentRect.rect.width + .5f + childRect.rect.width / parentRect.rect.width * (1 - childRect.pivot.x), 
                //     childRect.localPosition.y / parentRect.rect.height + .5f + childRect.rect.height / parentRect.rect.height * (1 - childRect.pivot.y));
                //
                // childRect.anchorMin = newAnchorsMin;
                // childRect.anchorMax = newAnchorsMax;
                // childRect.anchoredPosition = Vector2.zero;
                // childRect.sizeDelta = Vector2.zero;
                

                Vector2 min = new Vector2(
                    (childRect.localPosition.x - childRect.rect.width * childRect.pivot.x) / parentRect.rect.width + 0.5f,
                    (childRect.localPosition.y - childRect.rect.height * childRect.pivot.y) / parentRect.rect.height + 0.5f);
                Vector2 max = new Vector2(
                    (childRect.localPosition.x + childRect.rect.width * (1 - childRect.pivot.x)) / parentRect.rect.width + 0.5f,
                    (childRect.localPosition.y + childRect.rect.height * (1 - childRect.pivot.y)) / parentRect.rect.height + 0.5f);

                childRect.anchorMin = min;
                childRect.anchorMax = max;

                childRect.offsetMin = new Vector2(
                    (childRect.offsetMin.x - childRect.offsetMin.y) / parentRect.rect.width,
                    (childRect.offsetMin.y - childRect.offsetMin.y) / parentRect.rect.height);
                childRect.offsetMax = new Vector2(
                    (childRect.offsetMax.x - childRect.offsetMax.y) / parentRect.rect.width,
                    (childRect.offsetMax.y - childRect.offsetMax.y) / parentRect.rect.height);

                childRect.pivot = new Vector2(0.5f, 0.5f);
                childRect.anchoredPosition = Vector2.zero;
            }
        }
    }

    private void SetSoupVisuals(Soup soup)
    {
        soupImage.color = soup.color;
        soupImage.gameObject.SetActive(true);
        soupAnimator.Play("soupFill");
        soupDropChecker.SetActive(true);
    }
    private void SetNoodleVisuals(Noodle noodle)
    {
        noodleImage.color = noodle.color;
        noodleImage.gameObject.SetActive(true);
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        OnPartDropped(dropped);
    }
    
    private void UpdateIngredientsOrder()
    {
        Transform[] uiElements = new Transform[ingredientsParent.childCount];
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i] = ingredientsParent.GetChild(i);
        }
        Array.Sort(uiElements, (a, b) => b.position.y.CompareTo(a.position.y));
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetSiblingIndex(i);
        }
    }
}

public class DishData
{
    public Soup Soup { get; private set; }

    public Noodle Noodle { get; private set; }

    public List<Ingredient> Ingredients { get; } = new List<Ingredient>();

    public class Builder
    {
        private DishData _dishData;

        public Action<Soup> OnSoupSet;
        public Action<Noodle> OnNoodleSet;
        public Action OnIngredientAdded;
        
        public Builder()
        {
            _dishData = new DishData();
        }

        public Builder SetSoup(Soup soup)
        {
            _dishData.Soup = soup;
            OnSoupSet?.Invoke(soup);
            return this;
        }

        public Builder SetNoodle(Noodle noodle)
        {
            _dishData.Noodle = noodle;
            OnNoodleSet?.Invoke(noodle);
            return this;
        }

        public Builder AddIngredient(Ingredient ingredient)
        {
            _dishData.Ingredients.Add(ingredient);
            OnIngredientAdded?.Invoke();
            return this;
        }

        public DishData Build()
        {
            return _dishData;
        }

        public bool HasSoup()
        {
            return _dishData.Soup != null;
        }

        public bool HasNoodle()
        {
            return _dishData.Noodle != null;
        }
    }
}
