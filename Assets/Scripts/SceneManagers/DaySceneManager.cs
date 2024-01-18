using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaySceneManager : MonoBehaviour
{
    [SerializeField] private Canvas endDayCanvas;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private TextMeshProUGUI dayNumberText;
    [SerializeField] private TextMeshProUGUI endDayNumberText;
    [SerializeField] private TextMeshProUGUI chapterNumberText;

    [SerializeField] private TextMeshProUGUI customersServedTodayText;
    [SerializeField] private TextMeshProUGUI revenueTodayText;
    [SerializeField] private TextMeshProUGUI tipsTodayText;
    [SerializeField] private TextMeshProUGUI investmentsTodayText;
    [SerializeField] private TextMeshProUGUI rentTodayText;
    [SerializeField] private TextMeshProUGUI suppliesUsedTodayText;
    [SerializeField] private TextMeshProUGUI totalProfitTodayText;
    [SerializeField] private Canvas loadingCanvas;

    public void NextDay()
    {
        GameManager.Instance.ResetEndDayData();
        GameManager.Instance.ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    private void Start()
    {
        loadingCanvas.gameObject.SetActive(true);
        if (!GameManager.Instance.FirebaseManager.Authenticated)
        {
            GameManager.Instance.FirebaseManager.OnUserSetup += () =>
            {
                //dayNumberText.text = $"Day {(GameManager.Instance.DayNumber).ToString()} Chapter {GameManager.Instance.ChapterNumber.ToString()} ChapterPart {GameManager.Instance.ChapterPartNumber.ToString()}";
                dayNumberText.text = $"Day {GameManager.Instance.DayNumber}";
                chapterNumberText.text = $"Chapter {GameManager.Instance.ChapterNumber}";
                loadingCanvas.gameObject.SetActive(false);
            };
        }
        else
        {
            //dayNumberText.text = $"Day {(GameManager.Instance.DayNumber).ToString()} Chapter {GameManager.Instance.ChapterNumber.ToString()} ChapterPart {GameManager.Instance.ChapterPartNumber.ToString()}";
            dayNumberText.text = $"Day {GameManager.Instance.DayNumber}";
            chapterNumberText.text = $"Chapter {GameManager.Instance.ChapterNumber}";
            loadingCanvas.gameObject.SetActive(false);
        }
       
    }

    public void ShowShopCanvas()
    {
        GameManager.Instance.ShowShopCanvas();
    }

    public void ShowEndDayCanvas()
    {
        endDayNumberText.text= $"Day {GameManager.Instance.DayNumber.ToString()}";
        customersServedTodayText.text = GameManager.Instance.CustomersServedToday.ToString();
        revenueTodayText.text = GameManager.Instance.RevenueToday.ToString("F1");
        tipsTodayText.text = GameManager.Instance.TipsToday.ToString("F1");
        investmentsTodayText.text = GameManager.Instance.InvestmentsToday.ToString("F1");
        rentTodayText.text = GameManager.Instance.RentToday.ToString("F1");
        suppliesUsedTodayText.text = GameManager.Instance.SuppliesUsedToday.ToString("F1");
        totalProfitTodayText.text = GameManager.Instance.TotalProfitForToday().ToString("F1");
        endDayCanvas.gameObject.SetActive(true);
    }

    public void HideEndDayCanvas()
    {
        endDayCanvas.gameObject.SetActive(false);
    }
    
    
}
