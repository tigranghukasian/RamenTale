using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KitchenUIManager : MonoBehaviour
{
    [SerializeField] private float namesFadeDuration;
    [SerializeField] private GameObject orderTicket;
    [SerializeField] private Image orderTicketArea;
    [SerializeField] private Animator kitchenUIAnimator;
    [SerializeField] private Animator orderTicketAnimator;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private CanvasGroup noodleNames;
    [SerializeField] private CanvasGroup soupNames;
    private bool _isOrderTicketOpen;
    
    private static readonly int CLOSE = Animator.StringToHash("Close");
    private static readonly int OPEN = Animator.StringToHash("Open");

    
    
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0) && _isOrderTicketOpen)
        // {
        //     
        //     //orderTicket.SetActive(false);
        // }
    }

   

    public void SetOrder(Order order)
    {
        var text = string.Empty;
        text += $"{order.Soup.displayName}\n";
        text += $"{order.Noodle.displayName}\n";
        for (int i = 0; i < order.OrderIngredients.Count; i++)
        {
            text += $"{order.OrderIngredients[i].amount} {order.OrderIngredients[i].ingredient.displayName} \n";
        }

        orderText.text = text;
    }
 

    public void ToggleOrderTicket()
    {
        if (_isOrderTicketOpen)
        {
            CloseOrderTicket();
        }
        else
        {
            OpenOrderTicket();
        }
    }

    public void OpenOrderTicket()
    {
        if (!_isOrderTicketOpen)
        {
            _isOrderTicketOpen = true;
            orderTicketArea.raycastTarget = true;
            orderTicketAnimator.Play("ticket_open");
            if (GameManager.Instance.IsTutorialActive)
            {
                GameSceneManager.Instance.TutorialManager.CompleteAction("ViewOrder");
            }
        }
        
    }
    public void CloseOrderTicket()
    {
        if (_isOrderTicketOpen)
        {
            orderTicketAnimator.Play("ticket_close");
            _isOrderTicketOpen = false;
            orderTicketArea.raycastTarget = false;
            if (GameManager.Instance.IsTutorialActive)
            {
                GameSceneManager.Instance.TutorialManager.CompleteAction("CloseOrder");
            }
        }
        
    }

    public void StartKitchenView()
    {
        StartCoroutine(PlayAnimationOrderTicketAfterDelay("ticket_appear", 1.0f));
        kitchenUIAnimator.SetTrigger(OPEN);
    }

    public void EndKitchenView()
    {
        if (_isOrderTicketOpen)
        {
            orderTicketAnimator.Play("ticket_disappear_from_opened");
            StartCoroutine(DisableOrderTicketAfterDelay( 0.3f));
        }
        else
        {
            orderTicketAnimator.Play("ticket_disappear");
            StartCoroutine(DisableOrderTicketAfterDelay( 0.15f));
        }
       
        kitchenUIAnimator.SetTrigger(CLOSE);
        
    }
    

    public void ShowSoupAndNoodleNames()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
        

    }
    
    IEnumerator PlayAnimationOrderTicketAfterDelay(string animationName, float delay)
    {
        yield return new WaitForSeconds(delay);
        orderTicket.SetActive(true);
        orderTicketAnimator.Play(animationName);
    }
    IEnumerator DisableOrderTicketAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        orderTicket.SetActive(false);
        _isOrderTicketOpen = false;
    }
    private IEnumerator Fade()
    {
        noodleNames.alpha = 1f;
        soupNames.alpha = 1f;
        
        yield return new WaitForSeconds(1f);

        float elapsed = 0;
        while (elapsed < namesFadeDuration)
        {
            elapsed += Time.deltaTime;
            noodleNames.alpha = Mathf.Lerp(1, 0, elapsed / namesFadeDuration);
            soupNames.alpha = Mathf.Lerp(1, 0, elapsed / namesFadeDuration);
            yield return null;
        }

        noodleNames.alpha = 0;
        soupNames.alpha = 0;
    }


}
