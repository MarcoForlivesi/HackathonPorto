using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabContainer : MonoBehaviour
{
    [SerializeField] private List<Button> tabButton;
    [SerializeField] private List<Transform> tabContent;
    [SerializeField] private Sprite neutralImage;
    [SerializeField] private Sprite selectedImaage;

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
                imageTabButtons[i].sprite = selectedImaage;
            }
            else
            {
                imageTabButtons[i].sprite = neutralImage;
            }
            
            tabContent[i].gameObject.SetActive(i == index);
        }
    }
}
