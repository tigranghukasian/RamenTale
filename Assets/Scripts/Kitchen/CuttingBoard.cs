using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out IngredientBox ingredientBox))
        {
            if (ingredientBox.IngredientData.isCuttable && !ingredientBox.IngredientData.isCut)
            {
                ingredientBox.SpawnedComponent.transform.SetParent(transform);
                ingredientBox.SpawnedComponent.Placed = true;
                ingredientBox.SpawnedComponent.IsOnBoard = true;
                ingredientBox.SpawnedComponent.GetComponent<Image>().raycastTarget = true;
                UpdateIngredientsOrder();
            }
        }
    }
    public void UpdateIngredientsOrder()
    {
        Transform[] uiElements = new Transform[transform.childCount];
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i] = transform.GetChild(i);
        }
        Array.Sort(uiElements, (a, b) => b.position.y.CompareTo(a.position.y));
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetSiblingIndex(i);
        }
    }
}
