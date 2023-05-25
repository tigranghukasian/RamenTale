using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IngredientComponent : Moveable, IPointerClickHandler, IDropHandler
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

    private CuttingBoard _cuttingBoard;

    private int _clickedCount;
    private int _amountToClickForCut = 3;
    private float offsetAmount = 20f;
    
    
    public void Init()
    {
        SetSprite();
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
    }

    private void SetSprite()
    {
        image.sprite = IngredientData.sprite;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOnBoard)
        {
            // AudioManager.Instance.PlayKnifeClip();
            // _clickedCount++;
            // CheckIfCut();
        }
        
    }

    private void CheckIfCut()
    {
        if (_clickedCount >= _amountToClickForCut)
        {
            //Cut();
        }
    }

    private void Cut()
    {
        for (int i = 0; i < 2; i++)
        {
            IngredientComponent cutIngredient = Instantiate(ingredientData.CutVersion.componentToSpawn, transform.parent).GetComponent<IngredientComponent>();
            cutIngredient.ingredientData = ingredientData.CutVersion;
            cutIngredient.Init();
            cutIngredient.transform.position = transform.position += new Vector3(-offsetAmount * Mathf.Pow(-1, i), offsetAmount * Mathf.Pow(-1, i), 0);
            cutIngredient.GetComponent<Image>().raycastTarget = true;
        }
        transform.parent.GetComponent<CuttingBoard>().UpdateIngredientsOrder();
        
        Destroy(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!IsOnBoard)
        {
            return;
        }
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out Knife knife))
        {
            knife.IsCutting = true;
            AudioManager.Instance.PlayKnifeClip();
            StartCoroutine(CutAfterDelay(knife.CutAnimationLength));
            return;
        }

        _cuttingBoard.OnDrop(eventData);
    }

    public void SetOnBoard(CuttingBoard cuttingBoard)
    {
        _cuttingBoard = cuttingBoard;
        IsOnBoard = true;
        GetComponent<Image>().raycastTarget = true;
    }
    IEnumerator CutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cut();
    }
}
