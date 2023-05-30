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
    [SerializeField] private Canvas loadingCanvas;
    public void NextDay()
    {
        GameManager.Instance.ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    private void Start()
    {
        loadingCanvas.gameObject.SetActive(true);
        if (!GameManager.Instance.FirebaseManager.Authenticated)
        {
            GameManager.Instance.FirebaseManager.OnUserSetup += () =>
            {
                dayNumberText.text = $"Day {(GameManager.Instance.DayNumber + 1).ToString()}";
                loadingCanvas.gameObject.SetActive(false);
            };
        }
        else
        {
            dayNumberText.text = $"Day {(GameManager.Instance.DayNumber + 1).ToString()}";
            loadingCanvas.gameObject.SetActive(false);
        }
       
    }

    public void ShowEndDayCanvas()
    {
        endDayNumberText.text= $"Day {GameManager.Instance.DayNumber.ToString()}";
        endDayCanvas.gameObject.SetActive(true);
    }

    public void HideEndDayCanvas()
    {
        endDayCanvas.gameObject.SetActive(false);
    }
    
    public void ShowShopCanvas()
    {
        shopCanvas.gameObject.SetActive(true);
    }

    public void HideShopCanvas()
    {
        shopCanvas.gameObject.SetActive(false);
    }
}
