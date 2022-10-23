using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        if (dropdown == null)
        {
            dropdown = GetComponentInChildren<TMP_Dropdown>();
        }

        dropdown.onValueChanged.AddListener(PlayerChanged);

        string playerName = dropdown.options[dropdown.value].text;
        MainLogic.Instance.Data.PlayerName = playerName;
    }

    private void PlayerChanged(int index)
    {
        MainLogic.Instance.SetPlayerName(dropdown.options[index].text);
    }
}
