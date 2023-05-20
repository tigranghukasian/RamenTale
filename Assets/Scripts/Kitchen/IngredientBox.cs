using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientBox : DragCreator
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private bool infinite;
    [SerializeField] private Image cuttableKnife;
    [SerializeField] private Transform ingredientParent;

    private IngredientComponent _spawnedComponent;
    public IngredientComponent SpawnedComponent => _spawnedComponent;
    public Ingredient IngredientData => ingredientData;

    public Action<int> OnCountChanged;

    public int Count { get; set; } = 100;

    private void Start()
    {
        if (ingredientData != null)
        {
            moveableGraphic = ingredientData.componentToSpawn;
            IngredientComponent visual = Instantiate(ingredientData.componentToSpawn, ingredientParent).GetComponent<IngredientComponent>();
            visual.IngredientData = ingredientData;
            visual.Init();
            cuttableKnife.gameObject.SetActive(ingredientData.isCuttable);
        }

    }

    public bool HasAmount(int amount)
    {
        return Count >= amount;
    }

    public void Decrease(int amount)
    {
        Count -= amount;
        if (Count == 0)
        {
            Count = 0;
        }

        OnCountChanged?.Invoke(Count);
    }

    public void Increase(int amount)
    {
        Count += amount;
        OnCountChanged?.Invoke(Count);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        _spawnedComponent = (IngredientComponent)SpawnedItem;
        _spawnedComponent.IngredientData = ingredientData;
        _spawnedComponent.Init();

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (_spawnedComponent.Placed)
        {
            Decrease(1);
            if(CurrencyManager.Instance.HasCoin(ingredientData.price))
            {
                CurrencyManager.Instance.SubtractCoins(ingredientData.price);
            }
        }

    }
}
