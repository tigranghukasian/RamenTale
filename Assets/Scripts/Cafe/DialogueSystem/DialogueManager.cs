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

     private Dialogue _currentDialogue;
     private int _currentStep;

     public void StartDialogue(Customer customer)
     {
          _currentDialogue = RarityFunctions.GenerateItem(availableDialogues);
          _currentStep = 0;
          speechBubble.gameObject.SetActive(true);
          customerImage.gameObject.SetActive(true);
          DisplayStep();
     }
     
     private void DisplayStep()
     {
          string stepText = _currentDialogue.GetStep(_currentStep);
          speechBubble.SetText(stepText);
     }

     private void DisplayCustomStep(string stepText)
     {
          speechBubble.SetText(stepText);
     }

     public void NextStep()
     {
          if (_currentDialogue.HasStep(_currentStep+1))
          {
               _currentStep++;
               DisplayStep();
          }
          else
          {
               TellOrder();
          }
     }

     private void TellOrder()
     {
          Order newOrder = OrderManager.Instance.GenerateNewOrder();
          DisplayCustomStep(newOrder.orderText);
     }
}