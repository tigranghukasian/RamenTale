using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [SerializeField] private string action;
    [SerializeField] private int times = 1;
    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    [SerializeField] private Sprite overlaySprite;

    public string Action { 
        get => action;
        set => action = value;
    }
    public GameObject Target1 => target1;
    public GameObject Target2 => target2;
    public Sprite OverlaySprite => overlaySprite;
    public int Times => times;
}
