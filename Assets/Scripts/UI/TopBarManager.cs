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
    [SerializeField] private TextMeshProUGUI diamondsIncreaseText;
    [SerializeField] private TextMeshProUGUI diamondsDecreaseText;
    [SerializeField] private TextMeshProUGUI satisfactionText;
    [SerializeField] private GameObject satisfactionBar;
    [SerializeField] private GameObject timeOfDayBar;
    [SerializeField] private Image satisfactionFaceImage;
    [SerializeField] private Gradient satisfactionGradient;
    [SerializeField] private TextMeshProUGUI timeOfDayText;
    [SerializeField] private GameObject menuBar;
    [SerializeField] private GameObject pauseBg;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject offersView;
    [SerializeField] private Button menuButton;

    private Animator topBarManagerAnimator;
    
    private IPopup _currentPopup;

    private bool isMenuBarOpen = false;
    private bool shouldOpenShop = false;

    private Canvas canvas;
    private int sortingOrder;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        sortingOrder = canvas.sortingOrder;
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsBalanceText;
        CurrencyManager.Instance.OnCoinIncreased += ShowCoinIncrease;
        CurrencyManager.Instance.OnCoinDecreased += ShowCoinDecrease;
        CurrencyManager.Instance.OnDiamondsChanged += UpdateDiamondsBalanceText;
        CurrencyManager.Instance.OnDiamondsIncreased += ShowDiamondsIncrease;
        CurrencyManager.Instance.OnDiamondsDecreased += ShowDiamondsDecrease;
        topBarManagerAnimator = GetComponent<Animator>();
        topBarManagerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
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

    private void Update()
    {
    }

    
    public void UpdateCustomerSatisfactionText(float amount)
    {
        satisfactionText.text = $"{Mathf.RoundToInt(amount * 100f).ToString()}%";
        Color color = satisfactionGradient.Evaluate(amount);
        satisfactionText.color = color;
        satisfactionFaceImage.color = color;
    }

    private void ShowCoinIncrease(float amount)
    {
        coinsIncreaseText.transform.parent.gameObject.SetActive(true);
        coinsIncreaseText.text = "+" + amount.ToString("F1");
        topBarManagerAnimator.Play("coinIncrease");
        
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(coinsIncreaseText.transform.parent.gameObject, 1f));
    }

    private void ShowCoinDecrease(float amount)
    {
        coinsDecreaseText.transform.parent.gameObject.SetActive(true);
        coinsDecreaseText.text = "-" + amount.ToString("F1"); 
        topBarManagerAnimator.Play("coinDecrease");
        
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(coinsDecreaseText.transform.parent.gameObject, 1f));
    }

    
    private void ShowDiamondsIncrease(float amount)
    {
        diamondsIncreaseText.transform.parent.gameObject.SetActive(true);
        diamondsIncreaseText.text = "+" + amount.ToString("F0");
        topBarManagerAnimator.Play("diamondIncrease");
        
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(diamondsIncreaseText.transform.parent.gameObject, 1f));
    }

    private void ShowDiamondsDecrease(float amount)
    {
        diamondsDecreaseText.transform.parent.gameObject.SetActive(true);
        diamondsDecreaseText.text = "-" + amount.ToString("F0"); 
        topBarManagerAnimator.Play("diamondDecrease");
        
        StopAllCoroutines();
        StartCoroutine(DisableGameObjectAfterDelay(diamondsDecreaseText.transform.parent.gameObject, 1f));
    }
    IEnumerator DisableGameObjectAfterDelay(GameObject gameObjectToDisable, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
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
                Pause(true);
            }
            menuBar.SetActive(true);
            isMenuBarOpen = true;
        }
        
    }

    public void Pause(bool menuButtonActive = false)
    {
        pauseBg.SetActive(true);
        Time.timeScale = 0;
        if (!menuButtonActive)
        {
            menuButton.interactable = false;
        }
    }

    public void UnPause()
    {
        pauseBg.SetActive(false);
        Time.timeScale = 1;
        menuButton.interactable = true;
    }

    public void CloseMenu()
    {
        menuBar.SetActive(false);
        isMenuBarOpen = false;
    }
    private void DisableMenuBar()
    {
        menuBar.SetActive(false);
        isMenuBarOpen = false;
        UnPause();
    }

    public void EnableOffersView()
    {
        canvas.sortingOrder = sortingOrder + 10;
        offersView.SetActive(true);
        CloseMenu();
        Pause();
    }

    public void DisableOffersView()
    {
        canvas.sortingOrder = sortingOrder;
        offersView.SetActive(false);
        if (!ShopManager.Instance.IsShopOpen)
        {
            UnPause();
        }
     
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
        //Scene scene = SceneManager.GetActiveScene();
        DisableMenuBar();
        GameManager.Instance.ShowShopCanvas();
        // if (scene.name == StringConstants.GAME_SCENE_NAME)
        // {
        //     GameManager.Instance.GoToDayScene(() =>
        //     {
        //         DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
        //         daySceneManager.ShowShopCanvas();
        //     });
        // }
        // else if (scene.name == StringConstants.DAY_SCENE_NAME)
        // {
        //     DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
        //     daySceneManager.ShowShopCanvas();
        // }

        
    }
    
}
