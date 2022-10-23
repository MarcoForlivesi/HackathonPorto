
using Netly.Core;
using TMPro;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public static MainLogic Instance => instance;
    public RewardRecognitionData Data => state;

    //[SerializeField] HTTPSender httpSender;
    [SerializeField] private MultiplayerServer multiplayer_server;
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
        state.playerName = playerName;
    }

    public void SetReplyValue(string replyValue)
    {
        state.replyValue = replyValue;
    }

    public void SetDescription(string description)
    {
        state.description = description;
    }

    public void SetGlassesReward(string reward)
    {
        state.reward = reward;
    }

    public void SetHatAnimation(string reward)
    {
        state.reward = reward;
    }

    public void SetRewardAnimation(string reward)
    {
        state.reward = reward;
    }

    public void SendAvatar()
    {
        SetDescription(recognitionDescription.text);
        string id = "DATA";
        string loginString = JsonUtility.ToJson(state);
        byte[] loginBytes = Encode.GetBytes(loginString);
        multiplayer_server.server.BroadcastToEvent(id, loginBytes);

        Debug.Log($"OnSendAvatar ({name}): {Encode.GetString(loginBytes)}");
    }
}
