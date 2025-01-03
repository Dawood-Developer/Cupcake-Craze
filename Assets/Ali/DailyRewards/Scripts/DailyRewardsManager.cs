using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiobiumStudios;
using System;

public class DailyRewardsManager : MonoBehaviour
{
    private void OnEnable()
    {
        DailyRewards.instance.onClaimPrize += OnClaimPrizeDailyRewards;
    }

    private void OnDisable()
    {
        DailyRewards.instance.onClaimPrize -= OnClaimPrizeDailyRewards;
    }

    private void OnClaimPrizeDailyRewards(int day)
    {
        Reward reward = DailyRewards.instance.GetReward(day);

        print(reward.unit);
        print(reward.reward);

        
    }
}
