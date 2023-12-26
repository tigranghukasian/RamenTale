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
    [SerializeField] private TextMeshProUGUI diamondsBalanceText;
    [SerializeField] private TextMeshProUGUI satisfactionText;
    [SerializeField] private GameObject satisfactionBar;
    [SerializeField] private GameObject timeOfDayBar;
    [SerializeField] private Image satisfactionFaceImage;
    [SerializeField] private Gradient satisfactionGradient;
    [SerializeField] private TextMeshProUGUI timeOfDayText;
    [SerializeField] private GameObject menuBar;
    [SerializeField] private GameObject pauseBg;
    [SerializeField] private CanvasGroup canvasGroup;

    private Animator topBarManagerAnimator;
    
    private IPopup _currentPopup;

    private bool isMenuBarOpen = false;
    private bool shouldOpenShop = false;
    
    private void Start()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        CurrencyManager.Instance.OnCoinIncreased += ShowCoinIncrease;
        CurrencyManager.Instance.OnCoinDecreased += ShowCoinDecrease;
        CurrencyManager.Instance.OnDiamondsChanged += UpdateDiamondsBalanceText;
        topBarManagerAnimator = GetComponent<Animator>();
    }
    public void SetTimeDisplay(string timeString)
    {
        timeOfDayText.text = timeString;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DisableMenuBar();
        if (scene.name == StringConstants.GAME_SCENE_NAME)
        {
            GameSceneManager.Instance.CustomerManager.OnCustomerSatisfactionChanged += UpdateCustomerSatisfactionText;
            satisfactionBar.gameObject.SetActive(true);
            timeOfDayBar.gameObject.SetActive(true);
            GameSceneManager.Instance.TutorialManager.OnTutorialStarted += DisableCanvas;
            GameSceneManager.Instance.TutorialManager.OnTutorialEnded += EnableCanvas;
        }

        if (scene.name == StringConstants.DAY_SCENE_NAME)
        {
            
            satisfactionBar.gameObject.SetActive(false);
            timeOfDayBar.gameObject.SetActive(false);
        }
    }

    private void DisableCanvas()
    {
        canvasGroup.interactable = false;
    }

    private void EnableCanvas()
    {
        canvasGroup.interactable = true;
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
    private void UpdateDiamondsBalanceText(float amount)
    {
        diamondsBalanceText.text = amount.ToString("F0");
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void OpenMenu()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (isMenuBarOpen)
        {
            DisableMenuBar();
        }
        else
        {
            if (scene.name == StringConstants.GAME_SCENE_NAME)
            {
                pauseBg.SetActive(true);
                Time.timeScale = 0;
            }
            menuBar.SetActive(true);
            isMenuBarOpen = true;
        }
        
    }

    private void DisableMenuBar()
    {
        menuBar.SetActive(false);
        pauseBg.SetActive(false);
        isMenuBarOpen = false;
        Time.timeScale = 1;
    }

    public void GoToHome()
    {
        DisableMenuBar();
        SceneManager.LoadScene(StringConstants.DAY_SCENE_NAME);
    }
    
    public void GoToSettings()
    {
        DisableMenuBar();
        SceneManager.LoadScene(StringConstants.DAY_SCENE_NAME);
    }

    public void GoToShop()
    {
        Scene scene = SceneManager.GetActiveScene();
        DisableMenuBar();
        if (scene.name == StringConstants.GAME_SCENE_NAME)
        {
            GameManager.Instance.GoToDayScene(() =>
            {
                DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
                daySceneManager.ShowShopCanvas();
            });
        }
        else if (scene.name == StringConstants.DAY_SCENE_NAME)
        {
            DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
            daySceneManager.ShowShopCanvas();
        }

        
    }
    
}
