using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientBox : DragCreator
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private bool infinite;

    private IngredientComponent _spawnedComponent;
    public IngredientComponent SpawnedComponent => _spawnedComponent;

    public Action<int> OnCountChanged;

    private bool _isDragging;

    public int Count { get; set; } = 100;

    private void Start()
    {
        if (ingredientData != null)
        {
            moveableGraphic = ingredientData.componentToSpawn;
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
        }

    }
}
