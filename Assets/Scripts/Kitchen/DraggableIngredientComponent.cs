using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIngredientComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;
    private Transform _initialParent;
    public bool Placed { get; set; }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialPosition = transform.position;
        _initialParent = transform.parent;
        transform.SetParent(KitchenManager.Instance.Canvas.transform);
        //GetComponent<Image>().raycastTarget = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //GetComponent<CanvasGroup>().interactable = false;
        Placed = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta/ KitchenManager.Instance.Canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Placed)
        {
            //TODO: UNDERSTAND WHY THIS DOESNT WORK
            //FOR SOME REASON DOESNT WORK, POINTERDRAG IS ALWAYS THE GAMEOBJECT ITSELF
            
            // if (eventData.pointerDrag.TryGetComponent(out CuttingBoard cuttingBoard))
            // {
            //     GetComponent<Image>().raycastTarget = true;
            //     return;
            // }
            transform.SetParent(_initialParent);
            //GetComponent<Image>().raycastTarget = true;
            transform.position = _initialPosition;
            transform.parent.GetComponent<CuttingBoard>().UpdateIngredientsOrder();
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        
    }

   
}
