using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dishIngredientAddClip;
    [SerializeField] private AudioClip uiButtonPressClip;
    [SerializeField] private AudioClip dishSoupPourClip;
    [SerializeField] private AudioClip knifeClip;
    [SerializeField] private AudioClip cashIncomeClip;


    public void PlayDishIngredientAddClip()
    {
        audioSource.PlayOneShot(dishIngredientAddClip);
    }
    public void PlayDishSoupPourClip()
    {
        audioSource.PlayOneShot(dishSoupPourClip);
    }
    public void PlayKnifeClip()
    {
        audioSource.PlayOneShot(knifeClip);
    }
    public void PlayCashIncomeClip()
    {
        audioSource.PlayOneShot(cashIncomeClip);
    }

    public void PlayButtonPressClip()
    {
        audioSource.PlayOneShot(uiButtonPressClip);
    }


}
