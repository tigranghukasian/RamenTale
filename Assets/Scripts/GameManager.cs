using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

    [SerializeField] private Transform cafeDishSpot;

    [SerializeField] private Animator kitchenAnimator;
    private static readonly int CLOSE = Animator.StringToHash("Close");
    private static readonly int OPEN = Animator.StringToHash("Open");

    public DialogueManager DialogueManager => dialogueManager;
    public CustomerManager CustomerManager => customerManager;

    public Canvas CafeCanvas => cafeCanvas;


    public enum View { Cafe, Kitchen}
    
    public View CurrentView { get; private set; }

    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;
    }

    private void Start()
    {
        customerManager.GetNextCustomer();
    }
    public void OpenKitchen()
    {
        kitchenCanvas.gameObject.SetActive(true);
        kitchenAnimator.SetTrigger(OPEN);
        StartCoroutine(DisableCanvasAfterDelay(cafeCanvas, 1.0f)); // Assumes the animation takes 1 second
    }

    public void OpenCafe()
    {
        cafeCanvas.gameObject.SetActive(true);
        kitchenAnimator.SetTrigger(CLOSE);
        StartCoroutine(DisableCanvasAfterDelay(kitchenCanvas, 1.0f)); // Assumes the animation takes 1 second
    }

    public void CompleteDish(Dish dish)
    {
        dish.transform.SetParent(cafeDishSpot);
        dish.SetChildrenAnchorsToCorners();
        dish.AddComponent<DraggableDishComponent>();
        dish.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 150f);
        dish.transform.position = cafeDishSpot.transform.position;
    }

    IEnumerator DisableCanvasAfterDelay(Canvas canvas, float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.gameObject.SetActive(false);
    }
    
    public void MoveToKitchenToPrepareFood()
    {
        OpenKitchen();
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
