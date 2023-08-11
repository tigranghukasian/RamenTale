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
    
    public int CustomersServedToday { get; set; }
    public float RevenueToday { get; set; }
    public float TipsToday { get; set; }
    public float InvestmentsToday { get; set; }
    public float RentToday { get; set; }
    public float SuppliesUsedToday { get; set; }
    

    [SerializeField] private List<Day> days = new List<Day>();
    
    
    [Header("Scene Management")]
    [SerializeField] private float transitionSpeed = 1f;
    [SerializeField] private Image transitionImage;

    [Header("Managers")] [SerializeField] private FirebaseManager firebaseManager;

    public FirebaseManager FirebaseManager => firebaseManager;
    

    public Action OnDayEnded;

    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 0;
        //LoadDayInfo();
    }

    public void GoToGameScene()
    {
        ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    public void GoToDayScene()
    {
        ChangeScene(StringConstants.DAY_SCENE_NAME);
    }
    
    public void Restart()
    {
        ChangeScene(StringConstants.DAY_SCENE_NAME);
    }
    
    public void ChangeScene(string sceneName, Action onSceneChanged = null)
    {
        StartCoroutine(ChangeSceneAsync(sceneName, onSceneChanged));
    }
    
    
    public Day CurrentDay()
    {
        if (Utilities.IsIndexValid(days, DayNumber))
        {
            return days[DayNumber];
        }
        return null;

    }
    public void EndDay()
    {
        DayNumber++;
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
