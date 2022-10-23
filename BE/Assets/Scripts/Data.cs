using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardRecognitionData
{
    public string PlayerName { get { return playerName; } set { playerName = value; } }
    public string ReplyValue { get { return replyValue; } set { replyValue = value; } }
    public string Description { get { return description; } set { description = value; } }
    public string GlassesReward { get { return glassesReward; } set { glassesReward = value; } }
    public string HatReward { get { return hatReward; } set { hatReward = value; } }
    public string AnimationReward { get { return animationReward; } set { animationReward = value; } }

    private string playerName;
    private string replyValue;
    private string description;
    private string glassesReward;
    private string hatReward;
    private string animationReward;
}
