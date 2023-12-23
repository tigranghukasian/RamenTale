using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCreator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Moveable _spawnedItem;
    private RectTransform _spawnedItemRectTransform;
    
    public GameObject moveableGraphic;

    public Moveable SpawnedItem => _spawnedItem;
    
    private bool _isDragging;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (moveableGraphic == null || _isDragging)
        {
            return;
        }

        if (GameManager.Instance.IsTutorialActive)
        {
            if (GameSceneManager.Instance.TutorialManager.GetCurrentStepInteractableObject() != gameObject)
            {
                return;
            }
        }

        _isDragging = true;
        _spawnedItem = Instantiate(moveableGraphic, KitchenManager.Instance.DragCanvas.transform).GetComponent<Moveable>();

        _spawnedItemRectTransform = _spawnedItem.GetComponent<RectTransform>();
        _spawnedItem.transform.position = eventData.position;
        _spawnedItem.GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_spawnedItem == null)
        {
            return;
        }
        _spawnedItemRectTransform.anchoredPosition += eventData.delta/ KitchenManager.Instance.DragCanvas.scaleFactor;

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_spawnedItem == null)
        {
            return;
        }
        _isDragging = false;
        _spawnedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (!_spawnedItem.Placed)
        {
            Destroy(_spawnedItem.gameObject);
        }

    }
}
