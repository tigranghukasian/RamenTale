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

     private Queue<DialogueStep> _currentDialogueQueue = new Queue<DialogueStep>();

     private List<DialogueStep> _currentMainPath = new List<DialogueStep>();
     
     
     private DialogueStep _currentStep;
     private int _currentMainPathStepIndex;

     private void Start()
     {
          GameSceneManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
          GameSceneManager.Instance.DialogueManager.CustomerImage.gameObject.SetActive(false);
          GameSceneManager.Instance.CustomerManager.OnCustomerGenerated += GenerateDialogue;
          
     }

     private void AddStepToMainPath(DialogueStep step)
     {
          _currentMainPath.Add(step);
     }

     public void NextStep(DialogueChoiceInfo.Choice choice = DialogueChoiceInfo.Choice.NoChoice)
     {
          if (_currentDialogueQueue.Count == 0)
          {
               EnqueueNextStep(_currentStep, choice);
          }
          ConfirmCurrentStep();
          if (_currentDialogueQueue.Count != 0)
          {
               _currentStep = _currentDialogueQueue.Dequeue();
               BeginCurrentStep();
          }
          else
          {
               Debug.Log("Dialogue Has No More Steps");
          }
          
     }
     
     public void BeginCurrentStep()
     {
          
          speechBubble.gameObject.SetActive(true);
          speechBubble.SetText(_currentStep.StepText);
          if (_currentStep.StepText == string.Empty)
          {
               speechBubble.gameObject.SetActive(false);
          }
          else
          {
               DayCycleManager.Instance.Enabled = false;
          }

          if (_currentStep is DialogueMainPathStep)
          {
               DialogueMainPathStep mainPathStep = (DialogueMainPathStep)_currentStep;
               if (mainPathStep.Branches())
               {
                    speechBubble.EnableOptionsButtons();
                    speechBubble.SetButtons(mainPathStep.GetOptionAText(), mainPathStep.GetOptionBText());
               }
               
          }
          else
          {
               speechBubble.DisableOptionsButtons();
          }
          
          
          if (_currentStep.OnBeginStepAction != null)
          {
               _currentStep.OnBeginStepAction?.Invoke();
          }
          
     }

     private void OnDialogueFinished()
     {
          if (DayCycleManager.Instance.DayEnded)
          {
               DayCycleManager.Instance.EndDay();
          }
          StartCoroutine(GetNextCustomerAfterDelay(2f));
     }

     private IEnumerator GetNextCustomerAfterDelay(float seconds)
     {
          yield return new WaitForSeconds(seconds);
          GetNextCustomer();

     }

     private void GetNextCustomer()
     {
          _currentDialogueQueue.Clear();
          GameSceneManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
          GameSceneManager.Instance.CustomerManager.DepartCustomer();
          GameSceneManager.Instance.CustomerManager.GetNextCustomer();
     }
     private void EnqueueNextStep(DialogueStep currentStep, DialogueChoiceInfo.Choice choice)
     {
          if (choice == DialogueChoiceInfo.Choice.OptionA && currentStep is DialogueMainPathStep)
          {
               DialogueMainPathStep currStep = (DialogueMainPathStep)currentStep;
               for (int i = 0; i < currStep.PathA.Count; i++)
               {
                    _currentDialogueQueue.Enqueue(currStep.PathA[i]);
                    AddActions(currStep.PathA[i]);
               }
          }
          else if (choice == DialogueChoiceInfo.Choice.OptionB && currentStep is DialogueMainPathStep)
          {
               DialogueMainPathStep currStep = (DialogueMainPathStep)currentStep;
               for (int i = 0; i < currStep.PathB.Count; i++)
               {
                    _currentDialogueQueue.Enqueue(currStep.PathB[i]);
                    AddActions(currStep.PathB[i]);
               }
          }
          else if (choice == DialogueChoiceInfo.Choice.NoChoice && Utilities.IsIndexValid(_currentMainPath, _currentMainPathStepIndex+1))
          {
               _currentMainPathStepIndex++;
               AddActions(_currentMainPath[_currentMainPathStepIndex]);
               _currentDialogueQueue.Enqueue(_currentMainPath[_currentMainPathStepIndex]);
          }
     }

     public void AddActions(DialogueStep step)
     {
          if (step.ConfirmStepActionFunction == DialogueStep.ConfirmStepActionType.GetNextCustomer)
          {
               step.AddConfirmAction(GetNextCustomer);
          }

          if (step.ConfirmStepActionFunction == DialogueStep.ConfirmStepActionType.EnableTutorial)
          {
               GameManager.Instance.IsTutorialActive = true;
          }
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
          DialogueStep feedbackDialogueStep = new DialogueStep
          {
               StepText = feedbackText
          };
          feedbackDialogueStep.AddBeginStepAction(OnDialogueFinished);
          AddStepToMainPath(feedbackDialogueStep);
     }
     public void GenerateDialogue(Customer customer)
     {
          _currentMainPath.Clear();
          _currentMainPathStepIndex = 0;
          Dialogue dialogue = customer.Dialogue;
          if (customer.Dialogue == null)
          {
               dialogue = RarityFunctions.GenerateItem(availableDialogues);
          }

          for (int i = 0; i < dialogue.StepCount(); i++)
          {
               _currentMainPath.Add(dialogue.GetStep(i));
          }

          if (customer.Order != null)
          {
               Order order = customer.Order;
               OrderManager.Instance.CurrentOrder = order;
               if (order == null)
               {
                    order = OrderManager.Instance.GenerateNewOrder();
               }

               OrderDialogueStep orderDialogueStep = new OrderDialogueStep(order);
               orderDialogueStep.AddConfirmAction(GameSceneManager.Instance.MoveToKitchenToPrepareFood);
               orderDialogueStep.AddConfirmAction(GameSceneManager.Instance.CustomerManager.StartSatisfactionTimer);
               _currentMainPath.Add(orderDialogueStep);

               DialogueStep waitDialogueStep = new DialogueStep
               {
                    StepText = string.Empty
               };
               _currentMainPath.Add(waitDialogueStep);
          }
          
          speechBubble.gameObject.SetActive(true);
          customerImage.gameObject.SetActive(true);
          GameSceneManager.Instance.CustomerManager.SetCustomerImageAnimation(true);
          customerImage.sprite = customer.Sprite;
          _currentStep = _currentMainPath[0];
          BeginCurrentStep();
     }
     
}