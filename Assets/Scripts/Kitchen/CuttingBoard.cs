using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour, IDropHandler, IDragHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        
        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);

        if (rectTransform.rect.Contains(localPoint)) {
            GameObject dropped = eventData.pointerDrag;
            if (dropped.TryGetComponent(out IngredientBox ingredientBox))
            {
                if (ingredientBox.IngredientData.isCuttable && !ingredientBox.IngredientData.isCut)
                {
                    ingredientBox.SpawnedComponent.transform.SetParent(transform);
                    ingredientBox.SpawnedComponent.Placed = true;
                    ingredientBox.SpawnedComponent.SetOnBoard(this);
                    ingredientBox.SpawnedComponent.GetComponent<Image>().raycastTarget = true;
                    UpdateIngredientsOrder();
                }
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

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
