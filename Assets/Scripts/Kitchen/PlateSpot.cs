using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateSpot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Dish currentDish;
    [SerializeField] private Dish dishPrefab;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out PlatePlacer platePlacer) && currentDish == null)
        {
            currentDish = Instantiate(dishPrefab, transform).GetComponent<Dish>();
            currentDish.transform.position = transform.position;
            currentDish.Init();
        }
    }
}
