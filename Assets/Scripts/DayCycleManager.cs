using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCycleManager : Singleton<DayCycleManager>
{
    private float gameTime;
    [SerializeField]
    private float dayDuration = 60; //this is speed of time, it can be adjusted according to the game. 

    public bool Enabled { get; set; }
    public bool DayEnded { get; set; }
    
    [SerializeField] private TextMeshProUGUI timeOfDayText;

    public void ResetGameTime()
    {
        gameTime = 0;
    }
    private void Update()
    {
        if (!Enabled)
        {
            return;
           
        }
        float oldGameTime = gameTime;
        gameTime += Time.deltaTime * 1f/dayDuration;
        
        gameTime %= 1; // keeps gameTime between 0-1
        
        
        if (oldGameTime > gameTime)
        {
            DayEnded = true;
            Enabled = false;
            return;
            // GameSceneManager.Instance.OpenCafe();
            // GameSceneManager.Instance.CustomerManager.DepartCustomer();
            // Enabled = false;
            // StartCoroutine(EndDayAfterDelay(2f));
            // return;
        }
        UpdateTimeDisplay();
    }

    public void EndDay()
    {
        StartCoroutine(EndDayAfterDelay(2f));
        DayEnded = false;
    }

    IEnumerator EndDayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.EndDay();
    }

    private void UpdateTimeDisplay()
    {
        float totalMinutes = gameTime * (23f - 8f) * 60f; // Convert gameTime to total minutes within shop working hours.
        totalMinutes = Mathf.Round(totalMinutes / 15f) * 15f;
        
        if (totalMinutes > (23f - 8f) * 60f)
        {
            totalMinutes = (23f - 8f) * 60f;
        }
        
        int hours = 8 + (int)(totalMinutes / 60f); // calculate hours
        int minutes = (int)(totalMinutes % 60f); // calculate minutes

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        
        TopBarManager.Instance.SetTimeDisplay(timeString);
    }
}
