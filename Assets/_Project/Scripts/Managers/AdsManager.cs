using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class AdsManager : MonoBehaviour
{
    public event Action onHireYetiRequested;

    RewardedAdGameObject rewardedAdGO;

    void Start()
    {
        rewardedAdGO = MobileAds.Instance.GetAd<RewardedAdGameObject>("Rewarded Ad");

        MobileAds.Initialize(initStatus => { rewardedAdGO.LoadAd(); });
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo()
    {
        rewardedAdGO.ShowIfLoaded();
    }

    public void ResquetReward()
    {
        onHireYetiRequested?.Invoke();

        rewardedAdGO.LoadAd();
    }
}
