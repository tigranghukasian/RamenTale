using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Title("Managers")] 
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private CustomerManager customerManager;

    [Title("Canvases")] 
    [SerializeField] private Canvas kitchenCanvas;
    [SerializeField] private Canvas cafeCanvas;

    public DialogueManager DialogueManager => dialogueManager;


    public enum View { Cafe, Kitchen}
    
    public View CurrentView { get; private set; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
    }


    public void SetView(View view)
    {
        CurrentView = view;
        kitchenCanvas.gameObject.SetActive(view == View.Kitchen);
        cafeCanvas.gameObject.SetActive(view == View.Cafe);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
