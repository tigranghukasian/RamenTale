using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopBarManager : PersistentSingleton<TopBarManager>
{
    [SerializeField] private TextMeshProUGUI coinsBalanceText;
    [SerializeField] private TextMeshProUGUI coinsIncreaseText;
    [SerializeField] private TextMeshProUGUI coinsDecreaseText;
    [SerializeField] private TextMeshProUGUI satisfactionText;
    [SerializeField] private GameObject satisfactionBar;
    [SerializeField] private Image satisfactionFaceImage;
    [SerializeField] private Gradient satisfactionGradient;
    [SerializeField] private TextMeshProUGUI timeOfDayText;
    private Animator topBarManagerAnimator;
    
    private void Start()
    {
        base.Awake();
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        CurrencyManager.Instance.OnCoinIncreased += ShowCoinIncrease;
        CurrencyManager.Instance.OnCoinDecreased += ShowCoinDecrease;
        topBarManagerAnimator = GetComponent<Animator>();
    }
    public void SetTimeDisplay(string timeString)
    {
        timeOfDayText.text = timeString;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == StringConstants.GAME_SCENE_NAME)
        {
            GameSceneManager.Instance.CustomerManager.OnCustomerSatisfactionChanged += UpdateCustomerSatisfactionText;
            satisfactionBar.gameObject.SetActive(true);
        }

        if (scene.name == StringConstants.DAY_SCENE_NAME)
        {
            timeOfDayText.text = "08:00";
            satisfactionBar.gameObject.SetActive(false);
        }
    }
    

    private void ShowCoinIncrease(float amount)
    {
        coinsIncreaseText.transform.parent.gameObject.SetActive(true);
        coinsIncreaseText.text = "+" + amount.ToString("F1");
        topBarManagerAnimator.Play("coinIncrease");
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(coinsIncreaseText.transform.parent.gameObject, 1f));
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

    public void Restart()
    {
        GameManager.Instance.Restart();
    }
}
