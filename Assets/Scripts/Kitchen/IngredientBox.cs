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
    [SerializeField] private GameObject ingredientUnCutHolder;
    [SerializeField] private GameObject ingredientCutHolder;
    [SerializeField] private Transform ingredientUnCutParent;
    [SerializeField] private Transform ingredientNonCuttableParent;
    [SerializeField] private GameObject ingredientNonCuttableHolder;

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
            IngredientComponent visual = Instantiate(ingredientData.componentToSpawn, ingredientUnCutParent).GetComponent<IngredientComponent>();
            AddSpecificIngredientBoxToTutorial();
           
           
            visual.IngredientData = ingredientData;
            visual.Init();

            if (ingredientData.isCuttable)
            {
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
            else
            {
                ingredientCutHolder.SetActive(false);
                ingredientUnCutHolder.SetActive(false);
                ingredientNonCuttableHolder.gameObject.SetActive(true);
                visual.transform.SetParent(ingredientNonCuttableParent);
                visual.transform.position = ingredientNonCuttableParent.transform.position;
            }

            
            
        }

    }

    private void AddSpecificIngredientBoxToTutorial()
    {
        if (ingredientData.name == "egg")
        {
            GameSceneManager.Instance.TutorialManager.AddInteractableObject("CuttingBoardPlaceEgg", gameObject);
        }

        if (ingredientData.name == "pork")
        {
            GameSceneManager.Instance.TutorialManager.AddInteractableObject("CuttingBoardPlacePork", gameObject);
        }

        if (ingredientData.name == "seaweed")
        {
            GameSceneManager.Instance.TutorialManager.AddInteractableObject("AddSeaweed", gameObject);
        }
    }
    
    
    

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        
        if (GameManager.Instance.IsTutorialActive)
        {
            if (GameSceneManager.Instance.TutorialManager.GetCurrentStepInteractableObject() != gameObject)
            {
                return;
            }
        }

        _spawnedComponent = (IngredientComponent)SpawnedItem;
        _spawnedComponent.IngredientData = ingredientData;
        _spawnedComponent.Init();

    }
    

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (_spawnedComponent == null)
            return;
        
        if (_spawnedComponent.Placed)
        {
            if(CurrencyManager.Instance.HasCoin(ingredientData.price, true))
            {
                GameManager.Instance.SuppliesUsedToday += ingredientData.price;
                CurrencyManager.Instance.SubtractCoins(ingredientData.price);
            }
        }

    }
}
