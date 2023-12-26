using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RewardedAdsButton))]
public class RewardGiverCoins : RewardGiver
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
        CurrencyManager.Instance.AddCoins(10);
    }
}
