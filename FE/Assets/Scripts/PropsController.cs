using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    enum PropType { None, Glass, Hat }

    [SerializeField] private Transform[] rewards;
    [SerializeField] private string[] reward_keys;
    [SerializeField] private PropType currentPropType = PropType.None;
    [SerializeField] private PropType newPropType;
    [SerializeField] private Transform currentProp;
    Dictionary<string, Transform> rewardMap;

    private void Awake()
    {
        rewardMap = new Dictionary<string, Transform>();
    }

    private void Start()
    {
        for (int i = 0; i < reward_keys.Length; i++)
        {
            rewardMap.Add(reward_keys[i], rewards[i]);
        }
    }

    public void RecognitionDataDispatcher(RewardRecognitionData data)
    {
        if (data.reward == "dance")
        {
            DanceAnimation();
            return;
        }
        newPropType = GetPropTypeByName(data.reward);
        if (newPropType == currentPropType && currentPropType != PropType.None)
        {
            DespawnProp(currentProp);
        }
        currentPropType = newPropType;
        SpawnProp(rewardMap[data.reward]);
    }

    private void DanceAnimation()
    {
        GetComponent<Animator>().SetTrigger("Dance");
    }

    private void DespawnProp(Transform currentProp)
    {
        currentProp.gameObject.SetActive(false);
    }

    private void SpawnProp(Transform prop)
    {
        prop.gameObject.SetActive(true);
        currentProp = prop;

    }

    private PropType GetPropTypeByName(string name)
    {
        if (name == "black_glass" || name == "3d_glass")
        {
            return PropType.Glass;
        }
        if (name == "red_hat" || name == "brown_hat")
        {
            return PropType.Hat;
        }
        return PropType.None;
    }


}
