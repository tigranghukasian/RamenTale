using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateSpot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Dish dishPrefab;
    private Dish _currentDish;
    public Dish CurrentDish => _currentDish;

    public void AddEmptyPlate()
    {
        _currentDish = Instantiate(dishPrefab, transform).GetComponent<Dish>();
        _currentDish.transform.position = transform.position;
        _currentDish.Init();
        KitchenManager.Instance.SetCompleteButton(true);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // GameObject dropped = eventData.pointerDrag;
        // if (dropped.TryGetComponent(out PlatePlacer platePlacer) && _currentDish == null)
        // {
        //    
        // }
    }
}
