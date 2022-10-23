using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRewardSelector : RewardSelector
{
    protected override void SetReward(RewardButton rewardButton)
    {
        MainLogic.Instance.SetRewardAnimation(rewardButton.Reward);
    }
}
