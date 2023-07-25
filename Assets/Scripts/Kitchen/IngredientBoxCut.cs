using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientBoxCut : DragCreator
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private bool infinite;
    [SerializeField] private Transform ingredientParent;
    [SerializeField] private TextMeshProUGUI countText;

    private IngredientComponent _spawnedComponent;
    public IngredientComponent SpawnedComponent => _spawnedComponent;
    public Ingredient IngredientData
    {
        get => ingredientData;
        set => ingredientData = value;
    }

    public Action<int> OnCountChanged;

    public int Count { get; set; } = 0;
    

    public void Init()
    {
        if (ingredientData != null)
        {
            
            moveableGraphic = ingredientData.componentToSpawn;
            IngredientComponent visual = Instantiate(ingredientData.componentToSpawn, ingredientParent).GetComponent<IngredientComponent>();

           
            visual.IngredientData = ingredientData;
            visual.Init();
            countText.text = Count.ToString();

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

        countText.text = Count.ToString();
        OnCountChanged?.Invoke(Count);
    }

    public void IncreaseAfterDelay(int amount, float delay)
    {
        StartCoroutine(IncreaseWithDelay(amount, delay));
    }

    IEnumerator IncreaseWithDelay(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        Increase(amount);
    }

    public void Increase(int amount)
    {
        Count += amount;
        countText.text = Count.ToString();
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

        // if (_spawnedComponent.Placed)
        // {
        //     Decrease(1);
        // }

    }
}
