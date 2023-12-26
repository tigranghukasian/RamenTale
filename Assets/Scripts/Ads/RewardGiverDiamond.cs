using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RewardedAdsButton))]
public class RewardGiverDiamond : RewardGiver
{

    [SerializeField] private RewardedAdsButton _rewardedAdsButton;

    private void Start()
    {
        _rewardedAdsButton = GetComponent<RewardedAdsButton>();
        if (GameManager.Instance.IsAdsInitialized)
        {
            _rewardedAdsButton.LoadAd();
        }
        else
        {
            GameManager.Instance.AdsInitializer.OnInitializationCompleted += () => _rewardedAdsButton.LoadAd();
        }
    }

    public override void GiveReward()
    {
        CurrencyManager.Instance.AddDiamonds(1);
    }
}
