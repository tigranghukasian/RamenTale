using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableDishComponent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta/ GameSceneManager.Instance.CafeCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Image>().raycastTarget = true;

    }
}
