using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplyValueButton : MonoBehaviour
{
    public Button Button => button;
    public ReplyValues ReplyValue => replyValue;

    [SerializeField] private ReplyValues replyValue;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;
    [SerializeField] private Image border;

    private void Start()
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
