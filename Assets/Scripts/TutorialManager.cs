using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialHand;
    [SerializeField] private Image overlayImage;
    [SerializeField] List<TutorialStep> tutorialSteps;
    public Action OnTutorialStarted { get; set; }
    public Action OnTutorialEnded { get; set; }
    private int currentStepIndex = 0;
    private int numberOfCompleteActionCalls;
    private Animator tutorialHandAnimator;
    private Animator overlayAnimator;

    private Tween moveTween;
    public void StartTutorial()
    {
        tutorialHandAnimator = tutorialHand.GetComponent<Animator>();
        overlayAnimator = overlayImage.GetComponent<Animator>();
        tutorialHand.SetActive(true);
        ShowStep(tutorialSteps[currentStepIndex]);
        OnTutorialStarted?.Invoke();
        
    }
    void ShowStep(TutorialStep step)
    {

        moveTween.Kill();

        if (step.ShowHandAfterDelay != 0)
        {
            StartCoroutine(ShowHandAfterDelay(step.Target1, step.Target2, step.ShowHandAfterDelay));
        }
        else
        {
            ShowHand(step.Target1, step.Target2);
        }


        StartCoroutine(ChangeOverlayAfterDelay(step, AnimationConstants.TUTORIAL_OVERLAY_FADE_OUT_DURATION));
        
        step.AdditionalEvent?.Invoke();

        // Code to display the overlay and move the hand to the target object
        // Play animations or highlight the object as needed
    }

    IEnumerator ChangeOverlayAfterDelay(TutorialStep step,float delay)
    {
        yield return new WaitForSeconds(delay);
        overlayImage.gameObject.SetActive(true);
        overlayImage.sprite = step.OverlaySprite;
        if (step.OverlaySprite == null)
        {
            overlayImage.gameObject.SetActive(false);
        }
        overlayAnimator.Play("TutorialOverlayFadeIn");
    }

    IEnumerator ShowHandAfterDelay(GameObject target1, GameObject target2, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowHand(target1, target2);
    }

    private void ShowHand(GameObject target1, GameObject target2)
    {
        RestartCurrentHandFadeAnimation();
        tutorialHand.gameObject.SetActive(true);
        tutorialHand.transform.position = target1.transform.position;
        moveTween = tutorialHand.transform.DOMove(target2.transform.position, 1f).SetEase(Ease.Linear);
        
        moveTween.OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                tutorialHand.transform.position = target1.transform.position;
            });
        });
        moveTween.SetLoops(-1, LoopType.Restart);
    }
    
    private void RestartCurrentHandFadeAnimation()
    {
        tutorialHandAnimator.Play("TutorialHandFade", -1, 0);

    }

    public void CompleteAction(string action)
    {
        if (currentStepIndex < tutorialSteps.Count)
        {
            if (tutorialSteps[currentStepIndex].Action == action)
            {
                numberOfCompleteActionCalls++;
                if (numberOfCompleteActionCalls >= tutorialSteps[currentStepIndex].Times)
                {
                    tutorialHand.SetActive(false);
                    overlayAnimator.Play("TutorialOverlayFadeOut");
                    NextStep();
                }
              
            }
        }
    }

    public void AddAdditionalEventToStep(string stepAction, Action eventToAdd)
    {
        var step = tutorialSteps.FirstOrDefault(s => s.Action == stepAction);
        
        if (step != null)
        {
            step.AdditionalEvent += eventToAdd;
        }
    }

    public void AddInteractableObject(string stepAction, GameObject interactable)
    {
        var step = tutorialSteps.FirstOrDefault(s => s.Action == stepAction);
        
        if (step != null)
        {
            step.InteractableObject = interactable;
        }
    }

    public GameObject GetCurrentStepInteractableObject()
    {
        return tutorialSteps[currentStepIndex].InteractableObject;
    }

    public void CompleteActionAfterDelay(string action, float delay)
    {
        StartCoroutine(CompleteActionAfterDelayCoroutine(action, delay));
    }
    private void NextStep()
    {
        currentStepIndex++;
        numberOfCompleteActionCalls = 0;

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
        OnTutorialEnded?.Invoke();
        moveTween.Kill();
    }
    
    IEnumerator CompleteActionAfterDelayCoroutine( string action, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameSceneManager.Instance.TutorialManager.CompleteAction(action);
    }

}
