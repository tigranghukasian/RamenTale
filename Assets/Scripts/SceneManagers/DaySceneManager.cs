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
    public void NextDay()
    {
        GameManager.Instance.ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    private void Start()
    {
        dayNumberText.text= $"Day {(GameManager.Instance.DayNumber + 1).ToString()}";
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
