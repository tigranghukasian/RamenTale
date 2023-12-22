using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialHand;
    [SerializeField] private Image overlayImage;
    [SerializeField] List<TutorialStep> tutorialSteps;
    private int currentStepIndex = 0;
    private int numberOfCompleteActionCalls;

    private Tween moveTween;
    public void StartTutorial()
    {
        tutorialHand.SetActive(true);
        overlayImage.gameObject.SetActive(true);
        ShowStep(tutorialSteps[currentStepIndex]);
    }
    void ShowStep(TutorialStep step)
    {
        moveTween.Kill();
        tutorialHand.transform.position = step.Target1.transform.position;
        
        moveTween = tutorialHand.transform.DOMove(step.Target2.transform.position, 1f).SetEase(Ease.Linear);
        
        moveTween.OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                tutorialHand.transform.position = step.Target1.transform.position;
            });
        });
        moveTween.SetLoops(-1, LoopType.Restart);
        
        overlayImage.sprite = step.OverlaySprite;

        // Code to display the overlay and move the hand to the target object
        // Play animations or highlight the object as needed
    }

    public void CompleteAction(string action)
    {
        

        if (currentStepIndex < tutorialSteps.Count)
        {
            Debug.Log(tutorialSteps[currentStepIndex].Action + ":" + action);
            if (tutorialSteps[currentStepIndex].Action == action)
            {
                numberOfCompleteActionCalls++;
                if (numberOfCompleteActionCalls >= tutorialSteps[currentStepIndex].Times)
                {
                    NextStep();
                }
              
            }
        }
    }

    public void CompleteActionAfterDelay(string action, float delay)
    {
        StartCoroutine(CompleteActionAfterDelayCoroutine(action, delay));
    }
    private void NextStep()
    {
        currentStepIndex++;
        numberOfCompleteActionCalls = 0;
        Debug.Log("---reset");
       
        if (currentStepIndex < tutorialSteps.Count)
        {
            ShowStep(tutorialSteps[currentStepIndex]);
        }
        else
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        tutorialHand.SetActive(false);
        overlayImage.gameObject.SetActive(false);
        GameManager.Instance.IsTutorialActive = false;
    }
    
    IEnumerator CompleteActionAfterDelayCoroutine( string action, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameSceneManager.Instance.TutorialManager.CompleteAction(action);
    }

}
