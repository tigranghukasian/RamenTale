using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager>
{

    [Title("Canvases")] 
    [SerializeField] private Canvas kitchenCanvas;
    [SerializeField] private Canvas cafeCanvas;
    
    [Title("Managers")] 
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private CustomerManager customerManager;
    
    [SerializeField] private Transform cafeDishSpot;

    [SerializeField] private KitchenUIManager kitchenUIManager;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private OpeningUIManager _openingUIManager;
    
   
    public DialogueManager DialogueManager => dialogueManager;
    public CustomerManager CustomerManager => customerManager;
    public Canvas CafeCanvas => cafeCanvas;

    public TutorialManager TutorialManager => tutorialManager;
    
    

    private void Start()
    {
        _openingUIManager.gameObject.SetActive(true);
      
    }

    public void StartDay()
    {
        kitchenCanvas.gameObject.SetActive(false);
        customerManager.GetNextCustomer();
        DayCycleManager.Instance.ResetGameTime();
    }
    public void OpenKitchen()
    {
        if (GameManager.Instance.IsTutorialActive)
        {
            StartCoroutine(StartTutorialAfterDelay(AnimationConstants.KITCHEN_VIEW_OPEN_ANIMATION_DURATION));
        }
        kitchenCanvas.gameObject.SetActive(true);
        kitchenUIManager.StartKitchenView();
        StartCoroutine(DisableCanvasAfterDelay(cafeCanvas, AnimationConstants.KITCHEN_VIEW_OPEN_ANIMATION_DURATION)); // Assumes the animation takes 1 second
        KitchenManager.Instance.SetOrder();
        KitchenManager.Instance.PlateSpot.AddEmptyPlate(true);
        DayCycleManager.Instance.Enabled = true;
        CustomerManager.StartSatisfactionTimer();
    }

    public void OpenCafe()
    {
        KitchenManager.Instance.Knife.ResetKnife();
        cafeCanvas.gameObject.SetActive(true);
        kitchenUIManager.EndKitchenView();
        StartCoroutine(DisableCanvasAfterDelay(kitchenCanvas, AnimationConstants.KITCHEN_VIEW_CLOSE_ANIMATION_DURATION)); // Assumes the animation takes 1 second
        customerManager.StopSatisfactionTimer();
        DestroyDragCanvasObjects();
    }

    private void DestroyDragCanvasObjects()
    {
        Canvas dragCanvas = KitchenManager.Instance.DragCanvas;
        for (int i = 0; i < dragCanvas.transform.childCount; i++)
        {
            Destroy(dragCanvas.transform.GetChild(i).gameObject);
        }
    }
    public void CompleteDish(Dish dish)
    {
        dish.transform.SetParent(cafeDishSpot);
        dish.SetChildrenAnchorsToCorners();
        dish.AddComponent<DraggableDishComponent>();
        dish.GetComponent<RectTransform>().sizeDelta = new Vector2(130f, 130f);
        dish.transform.position = cafeDishSpot.transform.position;
        CustomerManager.StopSatisfactionTimer();
    }
    public void MoveToKitchenToPrepareFood()
    {
        OpenKitchen();
    }

    IEnumerator StartTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialManager.StartTutorial();
    }
    IEnumerator DisableCanvasAfterDelay(Canvas canvas, float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.gameObject.SetActive(false);
    }
    

    public void EndDay()
    {
        DayCycleManager.Instance.Enabled = false;
    }
}
