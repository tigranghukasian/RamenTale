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
    private int _dayNumber;
    
    [Header("Scene Management")]
    [SerializeField] private float transitionSpeed = 1f;
    [SerializeField] private Image transitionImage;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
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

    public void EndDay()
    {
        _dayNumber++;
        ChangeScene(StringConstants.DAY_SCENE_NAME, () =>
        {
            DaySceneManager daySceneManager = FindObjectOfType<DaySceneManager>();
            daySceneManager.ShowEndDayCanvas();
        });
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
