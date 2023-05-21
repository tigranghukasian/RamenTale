using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartPlacer : DragCreator, IPointerClickHandler
{
    [SerializeField] private Part part;

    public Part Part => part;
    

    private void Start()
    {
        moveableGraphic = part.componentToSpawn;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        KitchenManager.Instance.UIManager.ShowSoupAndNoodleNames();
    }
}
