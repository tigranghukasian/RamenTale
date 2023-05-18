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

    private DishData.Builder _dishDataBuilder;

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
            _dishDataBuilder.AddIngredient(ingredientBox.SpawnedComponent.Ingredient);
            ingredientBox.SpawnedComponent.transform.SetParent(ingredientsParent);
            ingredientBox.SpawnedComponent.Placed = true;
            UpdateIngredientsOrder();
        }
        if (pointerDrag.TryGetComponent(out PartPlacer partPlacer))
        {
            if (partPlacer.Part is Soup && !_dishDataBuilder.HasSoup())
            {
                _dishDataBuilder.SetSoup((Soup)partPlacer.Part);
            }
            else if (partPlacer.Part is Noodle && _dishDataBuilder.HasSoup() && !_dishDataBuilder.HasNoodle())
            {
                _dishDataBuilder.SetNoodle((Noodle)partPlacer.Part);
            }
            
        }
    }

    private void SetSoupVisuals(Soup soup)
    {
        soupImage.color = soup.color;
        soupImage.gameObject.SetActive(true);
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
    private Soup _soup;
    private Noodle _noodle;
    private List<Ingredient> _ingredients = new List<Ingredient>();

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
            _dishData._soup = soup;
            OnSoupSet?.Invoke(soup);
            return this;
        }

        public Builder SetNoodle(Noodle noodle)
        {
            _dishData._noodle = noodle;
            OnNoodleSet?.Invoke(noodle);
            return this;
        }

        public Builder AddIngredient(Ingredient ingredient)
        {
            _dishData._ingredients.Add(ingredient);
            OnIngredientAdded?.Invoke();
            return this;
        }

        public DishData Build()
        {
            return _dishData;
        }

        public bool HasSoup()
        {
            return _dishData._soup != null;
        }

        public bool HasNoodle()
        {
            return _dishData._noodle != null;
        }
    }
}
