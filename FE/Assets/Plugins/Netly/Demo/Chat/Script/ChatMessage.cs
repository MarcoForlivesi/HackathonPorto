using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ChatMessage : MonoBehaviour
{
    public MessageInfo myMessage;
    public MessageInfo otherMessage;
    public Color adminTextColor = Color.green;
    public Color adminBackgroundColor = Color.cyan;


    public string Username { get; private set; }
    public string Message { get; private set; }
    public bool IsMine { get; set; }
    public bool ShowUsername { get; set; }

    private void Awake()
    {
        myMessage.messageObj.SetActive(false);
        otherMessage.messageObj.SetActive(false);
    }

    public void WriteMessage(string username, string message, bool isMine)
    {
        Username = username;
        Message = message;
        IsMine = isMine;

        ShowUsername = !isMine;

        Write();
    }

    private void Write()
    {
        if (IsMine)
        {
            myMessage.messageObj.SetActive(true);
            myMessage.messageTxt.text = $"{((ShowUsername) ? $"@{Username}\n" : "")}{Message}";
        }
        else
        {
            otherMessage.messageObj.SetActive(true);
            otherMessage.messageTxt.text = $"{((ShowUsername) ? $"@{Username}\n" : "")}{Message}";
        }

        if(Username == ChatManager.ADMIN_USERNAME)
        {
            myMessage.messageTxt.color = adminTextColor;
            otherMessage.messageTxt.color = adminTextColor;

            foreach(var img in myMessage.backgrounds)
            {
                img.color = adminBackgroundColor;
            }
            
            foreach(var img in otherMessage.backgrounds)
            {
                img.color = adminBackgroundColor;
            }
        }
    }
}

[Serializable]
public class MessageInfo
{
    public GameObject messageObj;
    public TextMeshProUGUI messageTxt;
    public List<Image> backgrounds;
}

[Serializable]
public class LoginSender
{
    public string username;

    public LoginSender(string username)
    {
        this.username = username;
    }
}

[Serializable]
public class MessageSender
{
    public string username;
    public string message;

    public MessageSender(string username, string message)
    {
        this.username = username;
        this.message = message;
    }
}