using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesRewardSelector : RewardSelector
{
    protected override void SetReward(RewardButton rewardButton)
    {
        MainLogic.Instance.SetGlassesReward(rewardButton.Reward);
    }
}
