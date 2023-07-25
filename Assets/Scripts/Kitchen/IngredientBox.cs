    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientBox : DragCreator
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private IngredientBoxCut ingredientBoxCut;
    [SerializeField] private Transform ingredientParent;

    private IngredientComponent _spawnedComponent;
    public IngredientComponent SpawnedComponent => _spawnedComponent;

    public Ingredient IngredientData
    {
        get => ingredientData;
        set => ingredientData = value;
    }

    public IngredientBoxCut IngredientBoxCut => ingredientBoxCut;

    private void Start()
    {
        if (ingredientData != null)
        {
            moveableGraphic = ingredientData.componentToSpawn;
            IngredientComponent visual = Instantiate(ingredientData.componentToSpawn, ingredientParent).GetComponent<IngredientComponent>();

           
            visual.IngredientData = ingredientData;
            visual.Init();

            if (ingredientData.CutVersion)
            {
                ingredientBoxCut.IngredientData = ingredientData.CutVersion;
                ingredientBoxCut.Init();
            }
            else
            {
                ingredientBoxCut.gameObject.SetActive(false);
            }
            
        }

    }
    
    

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        _spawnedComponent = (IngredientComponent)SpawnedItem;
        _spawnedComponent.IngredientData = ingredientData;
        _spawnedComponent.Init();

    }
    

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (_spawnedComponent.Placed)
        {
            if(CurrencyManager.Instance.HasCoin(ingredientData.price))
            {
                CurrencyManager.Instance.SubtractCoins(ingredientData.price);
            }
        }

    }
}
