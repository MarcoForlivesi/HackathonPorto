using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRewardSelector : RewardSelector
{
    protected override void SetReward(RewardButton rewardButton)
    {
        MainLogic.Instance.SetHatAnimation(rewardButton.Reward);
    }
}
