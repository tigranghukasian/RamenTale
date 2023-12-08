using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateSpot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Dish dishPrefab;
    [SerializeField] private GameObject dishShadow;
    private Dish _currentDish;
    public Dish CurrentDish => _currentDish;

    public void AddEmptyPlate(bool addShadow = false)
    {
        _currentDish = Instantiate(dishPrefab, transform).GetComponent<Dish>();
        _currentDish.transform.position = transform.position;
        _currentDish.Init();
        if (addShadow)
        {
            dishShadow.SetActive(true);
        }
        KitchenManager.Instance.SetCompleteButton(true);
    }

    public void RemovePlate()
    {
        dishShadow.SetActive(false);
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
