using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesRewardSelector : RewardSelector
{
    protected override void SetReward(RewardButton rewardButton)
    {
        Debug.Log(rewardButton.Reward); 
        MainLogic.Instance.SetGlassesReward(rewardButton.Reward);
    }
}
