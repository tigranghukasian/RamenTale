using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartPlacer : DragCreator
{
    [SerializeField] private Part part;

    public Part Part => part;
    

    private void Start()
    {
        moveableGraphic = part.componentToSpawn;
    }
}
