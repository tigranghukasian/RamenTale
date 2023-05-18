using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerBehaviour : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out Dish dish))
        {
            OrderManager.Instance.ServeDish(dish);
            
            Destroy(dish.gameObject);
        }
    }
}
