using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    public int DayNumber { get; set; }
    
    public int ChapterNumber { get; set; }
    public int ChapterPartNumber { get; set; }
    
    public int CustomersServedToday { get; set; }
    public float RevenueToday { get; set; }
    public float TipsToday { get; set; }
    public float InvestmentsToday { get; set; }
    public float RentToday { get; set; }
    public float SuppliesUsedToday { get; set; }
    
    public bool IsTutorialActive { get; set; }
    public bool IsAdsInitialized { get; set; }
    

    [SerializeField] private List<Chapter> chapters = new List<Chapter>();
    
    
    [Header("Scene Management")]
    [SerializeField] private float transitionSpeed = 1f;
    [SerializeField] private Image transitionImage;

    [Header("Managers")] [SerializeField] private FirebaseManager firebaseManager;

    public FirebaseManager FirebaseManager => firebaseManager;

    public Action OnDayEnded;
    public AdsInitializer AdsInitializer { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 0;
        AdsInitializer = GetComponent<AdsInitializer>();
   
        FirebaseManager.OnUserSetup += IncreaseChapterIfLastChapterIsFinished;
        
    }
    

    private void IncreaseChapterIfLastChapterIsFinished()
    {
        if (ChapterNumber < chapters.Count && ChapterPartNumber > chapters[ChapterNumber-1].ChapterParts.Count)
        {
            ChapterNumber++;
            ChapterPartNumber = 1;
            FirebaseManager.UpdateUserData();
        }

 
    }

    public void GoToGameScene()
    {
        ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    public void GoToDayScene(Action onSceneChanged = null)
    {
        ChangeScene(StringConstants.DAY_SCENE_NAME, onSceneChanged);
    }
    
    public void Restart()
    {
        ChangeScene(StringConstants.DAY_SCENE_NAME);
    }

    public void ChangeScene(string sceneName, Action onSceneChanged = null)
    {
        StartCoroutine(ChangeSceneAsync(sceneName, onSceneChanged));
    }
    
    
    public ChapterPart GetCurrentChapter()
    {
        if (Utilities.IsIndexValid(chapters[ChapterNumber-1].ChapterParts, ChapterPartNumber - 1))
        {
            return chapters[ChapterNumber - 1].ChapterParts[ChapterPartNumber - 1];
        }
        return null;

    }
    public void EndDay()
    {
        DayNumber++;
        ChapterPartNumber++;
        IncreaseChapterIfLastChapterIsFinished();

        OnDayEnded?.Invoke();
        ChangeScene(StringConstants.DAY_SCENE_NAME, () =>
        {
            DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
            daySceneManager.ShowEndDayCanvas();
            CurrencyManager.Instance.SubtractCoins(RentToday);
        });
        
    }

    public void ResetEndDayData()
    {
        RevenueToday = 0;
        SuppliesUsedToday = 0;
        CustomersServedToday = 0;
        RentToday = 10f;
        TipsToday = 0;
        InvestmentsToday = 0;

    }
    
    public void ShowShopCanvas()
    {
        ShopManager.Instance.EnableShopView();
        ShopManager.Instance.UpdateShop();
        TopBarManager.Instance.CloseMenu();
        TopBarManager.Instance.Pause();
    }

    public void HideShopCanvas()
    {
        ShopManager.Instance.DisableShopView();
        TopBarManager.Instance.UnPause();
    }

    public float TotalProfitForToday()
    {
        float net = RevenueToday;
        net += TipsToday;
        net += InvestmentsToday;
        net -= SuppliesUsedToday;
        net -= RentToday;
        return net;
    }

    private IEnumerator ChangeSceneAsync(string sceneName, Action onSceneChanged)
    {
        enabled = false;
        transitionImage.gameObject.SetActive(true);
        
        for (float t = 0; t < 1; t += Time.deltaTime * transitionSpeed)
        {
            transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, t);
            yield return null;
        }


        // Load the new scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the new scene is loaded
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        onSceneChanged?.Invoke();
        
        for (float t = 1; t > 0; t -= Time.deltaTime * transitionSpeed)
        {
            transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, t);
            yield return null;
        }
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0);
        transitionImage.gameObject.SetActive(false);
        
    }
    
    
}
