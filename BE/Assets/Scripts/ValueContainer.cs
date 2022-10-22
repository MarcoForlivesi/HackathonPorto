using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ValueContainer : MonoBehaviour
{

    [SerializeField] private Button back;
    [SerializeField] private Button next;

    [SerializeField] private int visibleValues = 3;
    [SerializeField] private List<Button> valueList;

    private int position = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateButtons();
        back.onClick.AddListener(Back);
        next.onClick.AddListener(Next);
    }

    private void Back()
    {
        if (position == 0)
        {
            return;
        }

        position--;
        if (position == 0)
        {
            back.enabled = false;
        }
        UpdateButtons();
    }

    private void Next()
    {
        if (position + visibleValues > valueList.Count - 1)
        {
            return;
        }

        position++;
        if (position + visibleValues > valueList.Count - 1)
        {
            next.enabled = false;
        }
        UpdateButtons();
    }

    // Update is called once per frame
    private void UpdateButtons()
    {
        for (int i = 0; i < valueList.Count; i++)
        {
            bool visible = i >= position && i < position + visibleValues;
            valueList[i].gameObject.SetActive(visible);
        }
    }
}
