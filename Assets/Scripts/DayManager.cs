using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    private float gameTime;
    [SerializeField]
    private float timeSpeed = 1/300f; //this is speed of time, it can be adjusted according to the game. 

    public bool Enabled { get; set; }
    
    [SerializeField] private TextMeshProUGUI timeOfDayText;

    private void Update()
    {

        float oldGameTime = gameTime;
        if (Enabled)
        {
            gameTime += Time.deltaTime * timeSpeed;
        }
        
        gameTime %= 1; // keeps gameTime between 0-1
        
        
        if (oldGameTime > gameTime) 
        {
            GameSceneManager.Instance.OpenCafe();
            GameSceneManager.Instance.CustomerManager.DepartCustomer();
            Enabled = false;
            StartCoroutine(EndDayAfterDelay(2f));
            return;
        }
        UpdateTimeDisplay();
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
        
        if (totalMinutes >= (23f - 8f) * 60f)
        {
            totalMinutes = (23f - 8f) * 60f - 15f;
        }
        
        int hours = 8 + (int)(totalMinutes / 60f); // calculate hours
        int minutes = (int)(totalMinutes % 60f); // calculate minutes

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        
        TopBarManager.Instance.SetTimeDisplay(timeString);
    }
}
