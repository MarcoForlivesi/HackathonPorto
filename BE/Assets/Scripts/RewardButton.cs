using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public Button Button => button;
    public string Reward => rewardName;

    [SerializeField] private Button button;
    [SerializeField] private Image border;
    [SerializeField] private string rewardName;

    public void Start()
    {
        if (button == null)
        {
            button = GetComponentInChildren<Button>();
        }
    }

    public void SetActive(bool active)
    {
        border.gameObject.SetActive(active);
    }
}
