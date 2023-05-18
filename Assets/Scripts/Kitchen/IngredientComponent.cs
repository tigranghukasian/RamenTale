using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IngredientComponent : Moveable, IPointerClickHandler
{
    [SerializeField] private Ingredient ingredientData;
    [SerializeField] private Image image;

    public Ingredient IngredientData
    {
        get => ingredientData;
        set => ingredientData = value;
    }

    public bool IsCut { get; set; }
    public bool IsOnBoard { get; set; }

    private int _clickedCount;
    private int _amountToClickForCut = 3;
    private float offsetAmount = 30f;
    
    
    public void Init()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        image.sprite = IngredientData.sprite;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOnBoard)
        {
            _clickedCount++;
            CheckIfCut();
        }
        
    }

    private void CheckIfCut()
    {
        if (_clickedCount >= _amountToClickForCut)
        {
            Cut();
        }
    }

    private void Cut()
    {
        for (int i = 0; i < 2; i++)
        {
            IngredientComponent cutIngredient = Instantiate(ingredientData.CutVersion.componentToSpawn, transform.parent).GetComponent<IngredientComponent>();
            cutIngredient.ingredientData = ingredientData.CutVersion;
            cutIngredient.Init();
            cutIngredient.transform.position = transform.position += new Vector3(Random.Range(-offsetAmount, offsetAmount), Random.Range(-offsetAmount, offsetAmount), 0);
            cutIngredient.GetComponent<Image>().raycastTarget = true;
        }
        transform.parent.GetComponent<CuttingBoard>().UpdateIngredientsOrder();
        
        Destroy(gameObject);
    }
}
