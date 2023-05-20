using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
     
     [SerializeField] private Image customerImage;
     public Image CustomerImage => customerImage;
     [SerializeField]
     private SpeechBubble speechBubble;

     public SpeechBubble SpeechBubble => speechBubble;

     [SerializeField]
     private List<Dialogue> availableDialogues;

     private Queue<DialogueStep> _dialogueSteps = new Queue<DialogueStep>();
     
     
     private DialogueStep _currentStep;

     private void Start()
     {
          GameManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
          GameManager.Instance.DialogueManager.CustomerImage.gameObject.SetActive(false);
          GameManager.Instance.CustomerManager.OnCustomerGenerated += GenerateDialogue;
     }

     private void EnqueueStep(DialogueStep step)
     {
          _dialogueSteps.Enqueue(step);
     }

     public void NextStep()
     {
          if (_dialogueSteps.Count > 0)
          {
               ConfirmCurrentStep();
               _currentStep = _dialogueSteps.Dequeue();
               ProcessCurrentStep();
          }
          else
          {
               GameManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
               GameManager.Instance.DialogueManager.CustomerImage.gameObject.SetActive(false);
               GameManager.Instance.CustomerManager.GetNextCustomer();
               Debug.Log("DIALOGUE ENDED");
          }
     }

     public void ProcessCurrentStep()
     {
          speechBubble.gameObject.SetActive(true);
          speechBubble.SetText(_currentStep.StepText);
          if (_currentStep.StepText == string.Empty)
          {
               speechBubble.gameObject.SetActive(false);
          }
          // switch (_currentStep.StepType)
          // {
          //      case DialogueStepType.Speech:
          //           
          //           break;
          //      case DialogueStepType.Order:
          //           
          //           break;
          //      case DialogueStepType.Feedback:
          //           break;
          // }
     }

     public void ConfirmCurrentStep()
     {
          if (_currentStep == null)
          {
               return;
          }
          if (_currentStep.OnConfirmStepCustomAction != null)
          {
               _currentStep.OnConfirmStepCustomAction?.Invoke();
          }

          if (_currentStep is OrderDialogueStep)
          {
               speechBubble.gameObject.SetActive(false);
          }
     }

     public void AddFeedback(string feedbackText)
     {
          DialogueStep feedbackDialogueText = new DialogueStep
          {
               StepText = feedbackText
          };
          EnqueueStep(feedbackDialogueText);
     }
     public void GenerateDialogue(Customer customer)
     {
          Dialogue dialogue = RarityFunctions.GenerateItem(availableDialogues);
          for (int i = 0; i < dialogue.StepCount(); i++)
          {
               EnqueueStep(dialogue.GetStep(i));
          }

          Order newOrder = OrderManager.Instance.GenerateNewOrder();
          
          OrderDialogueStep orderDialogueStep = new OrderDialogueStep(newOrder);
          orderDialogueStep.SetCustomAction(GameManager.Instance.MoveToKitchenToPrepareFood);
          EnqueueStep(orderDialogueStep);

          DialogueStep waitDialogueStep = new DialogueStep
          {
               StepText = string.Empty
          };
          EnqueueStep(waitDialogueStep);
          
          speechBubble.gameObject.SetActive(true);
          customerImage.gameObject.SetActive(true);
          customerImage.sprite = customer.Sprite;
          _currentStep = _dialogueSteps.Dequeue();
          ProcessCurrentStep();
          
     }
     
}