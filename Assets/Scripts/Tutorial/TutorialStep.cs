using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private string action;
    [SerializeField] private int times = 1;
    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    [SerializeField] private Sprite overlaySprite;
    [SerializeField] private float showHandAfterDelay;
    public Action AdditionalEvent { get; set; }
    public GameObject InteractableObject
        {
            get => interactableObject;
            set => interactableObject = value;
        }
    public string Action { 
        get => action;
        set => action = value;
    }
    public GameObject Target1 => target1;
    public GameObject Target2 => target2;
    public Sprite OverlaySprite => overlaySprite;
    public int Times => times;
    public float ShowHandAfterDelay => showHandAfterDelay;
}
