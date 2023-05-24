using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaySceneManager : MonoBehaviour
{
    [SerializeField] private Canvas endDayCanvas;
    public void NextDay()
    {
        GameManager.Instance.ChangeScene(StringConstants.GAME_SCENE_NAME);
    }

    public void ShowEndDayCanvas()
    {
        endDayCanvas.gameObject.SetActive(true);
    }

    public void HideEndDayCanvas()
    {
        endDayCanvas.gameObject.SetActive(false);
    }
}
