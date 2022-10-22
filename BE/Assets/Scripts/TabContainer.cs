using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabContainer : MonoBehaviour
{
    [SerializeField] private List<Button> tabButton;
    [SerializeField] private List<Transform> tabContent;
    [SerializeField] private Color neutral;
    [SerializeField] private Color selected;

    private List<Image> imageTabButtons;

    // Start is called before the first frame update
    void Start()
    {
        imageTabButtons = new List<Image>();
        for (int i = 0; i < tabButton.Count; i++)
        {
            Button button = tabButton[i];
            Image image = button.GetComponentInChildren<Image>();
            imageTabButtons.Add(image);
            int c = i;
            button.onClick.AddListener(() => SelectTab(c) );
        }

        SelectTab(0);
    }

    private void SelectTab(int index)
    {
        for (int i = 0; i < tabButton.Count && i< tabContent.Count; i++)
        {
            if (i == index)
            {
                imageTabButtons[i].color = selected;
            }
            else
            {
                imageTabButtons[i].color = neutral;
            }
            
            tabContent[i].gameObject.SetActive(i == index);
        }
    }
}
