using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewardSelector : MonoBehaviour
{
    [SerializeField] private List<RewardButton> rewardButtonList;

    private void Start()
    {
        for (int i = 0; i < rewardButtonList.Count; i++)
        {
            RewardButton rewardButton = rewardButtonList[i];
            int c = i;
            rewardButton.Button.onClick.AddListener(() => RewardChanged(c));
            rewardButton.SetActive(false);
        }

    }

    abstract protected void SetReward(RewardButton rewardButton);

    private void RewardChanged(int index)
    {
        RewardButton rewardButton = rewardButtonList[index];
        SetReward(rewardButton);
    }
}
