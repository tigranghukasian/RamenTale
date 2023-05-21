using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsBalanceText;
    [SerializeField] private TextMeshProUGUI coinsIncreaseText;
    [SerializeField] private TextMeshProUGUI coinsDecreaseText;
    [SerializeField] private TextMeshProUGUI satisfactionText;
    [SerializeField] private Image satisfactionFaceImage;
    [SerializeField] private Gradient satisfactionGradient;
    private Animator topBarManagerAnimator;
    
    private void Awake()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        CurrencyManager.Instance.OnCoinIncreased += ShowCoinIncrease;
        CurrencyManager.Instance.OnCoinDecreased += ShowCoinDecrease;
        GameManager.Instance.CustomerManager.OnCustomerSatisfactionChanged += UpdateCustomerSatisfactionText;
        topBarManagerAnimator = GetComponent<Animator>();
        
    }

    private void ShowCoinIncrease(float amount)
    {
        coinsIncreaseText.gameObject.SetActive(true);
        coinsIncreaseText.text = "+" + amount.ToString("F1");
        topBarManagerAnimator.Play("coinIncrease");
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(coinsIncreaseText.gameObject, 0.5f));
    }

    public void UpdateCustomerSatisfactionText(float amount)
    {
        satisfactionText.text = $"{Mathf.RoundToInt(amount * 100f).ToString()}%";
        Color color = satisfactionGradient.Evaluate(amount);
        satisfactionText.color = color;
        satisfactionFaceImage.color = color;
    }

    private void ShowCoinDecrease(float amount)
    {
        coinsDecreaseText.gameObject.SetActive(true);
        coinsDecreaseText.text = "-" + amount.ToString("F1"); 
        topBarManagerAnimator.Play("coinDecrease");
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(coinsDecreaseText.gameObject, 0.5f));
    }

    IEnumerator DisableGameObjectAfterDelay(GameObject gameObjectToDisable, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObjectToDisable.SetActive(false);
    }
    private void UpdateCoinsBalanceText(float amount)
    {
        coinsBalanceText.text = amount.ToString("F1");
    }
}
