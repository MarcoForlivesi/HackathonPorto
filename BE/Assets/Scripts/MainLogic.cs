
using TMPro;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public static MainLogic Instance => instance;
    public RewardRecognitionData Data => state;

    [SerializeField] HTTPSender httpSender;
    [SerializeField] TMP_InputField recognitionDescription;

    private RewardRecognitionData state;
    private static MainLogic instance;

    public void Awake()
    {
        instance = this;
        state = new RewardRecognitionData();
    }

    private void Start()
    {
    }

    public void SetPlayerName(string playerName)
    {
        state.PlayerName = playerName;
    }

    public void SetReplyValue(string replyValue)
    {
        state.ReplyValue = replyValue;
    }

    public void SetDescription(string description)
    {
        state.Description = description;
    }

    public void SetGlassesReward(string reward)
    {
        state.GlassesReward = reward;
    }

    public void SetHatAnimation(string reward)
    {
        state.HatReward = reward;
    }

    public void SetRewardAnimation(string reward)
    {
        state.AnimationReward = reward;
    }

    public void SendAvatar()
    {
        SetDescription(recognitionDescription.text);
        httpSender.SendData(state);
    }
}
