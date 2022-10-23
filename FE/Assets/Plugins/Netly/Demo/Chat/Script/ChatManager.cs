using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Netly.Tcp;
using Netly.Core;
using Netly.Unity;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(NetlyHost))]
public class ChatManager : MonoBehaviour
{
    #region Var

    #region Chat

    [Header("Chat Canvas")]
    public Canvas chatCanvas;
    public TMP_InputField messageIF;
    public Button sentMessageBTN;
    public TextMeshProUGUI usernameTXT;
    
    #endregion

    #region Message

    [Header("Message")]
    public GameObject messagePrefab;
    public Transform messageTarget;

    #endregion

    #region Login

    [Header("Login Canvas")]
    public Canvas loginCanvas;
    public TMP_InputField loginIF;
    public Button loginBTN;
    public string username;

    #endregion

    #region Error

    [Header("Error Canvas")]
    public Canvas errorCanvas;
    public TextMeshProUGUI errorTitleTXT;
    public TextMeshProUGUI errorMessageTXT;
    public Button errorCloseBTN;

    #endregion

    #region Load

    [Header("Load Canvas")]
    public Canvas loadCanvas;
    public TextMeshProUGUI loadMessageTXT;

    #endregion

    #region Network

    public TcpClient client;
    public TcpServer server;

    private NetlyHost _host;
    private bool _isServer;
    private bool _isClient;
    private int _loadIndex;
    public bool forceStartServer;
    public const string ADMIN_USERNAME = "***** SERVER *****";

    #endregion

    #endregion

    #region On Init

    public void Init()
    {
        _host = GetComponent<NetlyHost>();
        _isServer = _host.isServer;
        _isClient = !_host.isServer;

        #region Client
        
        if (_isClient)
        {
            // stop chat canvas
            chatCanvas.gameObject.SetActive(false);

            // stop load canvas
            StopLoad();

            // stop error canvas
            StopError();

            // init login canvas
            loginCanvas.gameObject.SetActive(true);
            loginBTN.onClick = new Button.ButtonClickedEvent();
            // login button event
            ButtonEvent(loginBTN, () =>
            {
                username = loginIF.text;

                #region Check length of username

                if (username.Length <= 4)
                {
                    // reset input data
                    loginIF.text = null;

                    // start message error
                    StartError("Invalid username", "Min length is 5 character");

                    // error button close event
                    ButtonEvent(errorCloseBTN, StopError);
                }
                else if (username.Length >= 14)
                {
                    // reset input data
                    loginIF.text = null;

                    // start message error
                    StartError("Invalid username", "Max length is 14 character");

                    // error button close event
                    ButtonEvent(errorCloseBTN, StopError);
                }
                #endregion
                else
                {
                    // start loading canvas
                    StartLoad();

                    // open  connnection
                    client.Open(_host.GetHost());
                }
            });          

            client = new TcpClient();

            client.OnOpen(() =>
            {
                // stop loading canvas
                StopLoad();

                // stop login canvas
                loginCanvas.gameObject.SetActive(false);

                // start chat canvas
                chatCanvas.gameObject.SetActive(true);

                // add display username
                usernameTXT.text = username;

                // config send message button
                ButtonEvent(sentMessageBTN, () =>
                {
                    // return if connection is closed
                    if (!client.Opened) return;

                    messageIF.interactable = false;

                    // verify if is white string
                    if (string.IsNullOrWhiteSpace(messageIF.text)) return;

                    // create message object
                    var messageObj = new MessageSender(username, messageIF.text);

                    // convert message object to json string
                    var messageJson = JsonUtility.ToJson(messageObj);


                    // draw message on screen
                    var chatMessage = GetChatMessage();
                    chatMessage.WriteMessage(username, messageIF.text, true);

                    // reset input value
                    messageIF.text = null;
                    messageIF.interactable = true;

                    // sent json message to event "message"
                    client.ToEvent("message", Encode.GetBytes(messageJson));

                });

                print("[CLIENT] OnOpen");
            });

            client.OnError((e) =>
            {
                // stop loading canvas
                StopLoad();

                // start message error
                StartError("Connection error", e.Message);

                // error button close event
                ButtonEvent(errorCloseBTN, StopError);                

                print($"[CLIENT] OnError: {e.Message}");
            });

            client.OnClose(() =>
            {
                StartError("Connection close", "Connnection with server is closed!");

                ButtonEvent(errorCloseBTN, () =>
                {
                    _chatMessageIndex = 0;

                    // stop chat canvas
                    chatCanvas.gameObject.SetActive(false);

                    // stop load canvas
                    StopLoad();

                    // stop error canvas
                    StopError();

                    // init login canvas
                    loginCanvas.gameObject.SetActive(true);
                });

                print("[CLIENT] OnClose");
            });

            client.OnEvent((name, data) =>
            {
                if(name == "message")
                {
                    try
                    {
                        // try convert data to MessageSender object
                        var messageObj = JsonUtility.FromJson<MessageSender>(Encode.GetString(data)); ;

                        // draw message on screen
                        var chatMessage = GetChatMessage();
                        chatMessage.WriteMessage(messageObj.username, messageObj.message, false);
                    }
                    catch(Exception e)
                    {
                        print(e);
                    }
                }

                print($"[CLIENT] OnEvent ({name}): {Encode.GetString(data)}");
            });
        }

        #endregion

        #region Server

        if (_isServer)
        {
            StartLoad();

            username = ADMIN_USERNAME;

            server = new TcpServer();

            loginCanvas.gameObject.SetActive(false);

            server.OnOpen(() =>
            {
                StopLoad();

                // add display username
                usernameTXT.text = username;

                chatCanvas.gameObject.SetActive(true);

                // config send message button
                ButtonEvent(sentMessageBTN, () =>
                {                    
                    messageIF.interactable = false;

                    // verify if is white string
                    if (string.IsNullOrWhiteSpace(messageIF.text)) return;

                    // create message object
                    var messageObj = new MessageSender(username, messageIF.text);

                    // convert message object to json string
                    var messageJson = JsonUtility.ToJson(messageObj);


                    // draw message on screen
                    var chatMessage = GetChatMessage();
                    chatMessage.WriteMessage(username, messageIF.text, true);

                    // reset input value
                    messageIF.text = null;
                    messageIF.interactable = true;

                    // sent json message to event "message"
                    foreach (var _client in server.Clients)
                    {
                        _client.ToEvent("message", Encode.GetBytes(messageJson));
                    }
                });

                print("[SERVER] OnOpen");
            });

            server.OnError((e) =>
            {
                // stop loading canvas
                StopLoad();

                // start message error
                StartError("Connection error", e.Message);

                // error button close event
                ButtonEvent(errorCloseBTN, StopError);

                print($"[SERVER] OnError: {e.Message}");
            });

            server.OnClose(() =>
            {
                StartError("Connection close", "A connnection is closed!");
                ButtonEvent(errorCloseBTN, null);

                print("[SERVER] OnClose");
            });

            server.OnEnter((client) =>
            {
                print($"[SERVER] OnEnter: {client.Id}");
            });

            server.OnExit((client) =>
            {
                print($"[SERVER] OnExit: {client.Id}");
            });

            server.OnEvent((client, name, data) =>
            {
                if (name == "message")
                {
                    try
                    {
                        // try convert data to MessageSender object
                        var messageObj = JsonUtility.FromJson<MessageSender>(Encode.GetString(data)); ;

                        // draw message on screen
                        var chatMessage = GetChatMessage();
                        chatMessage.WriteMessage(messageObj.username, messageObj.message, false);
                    }
                    catch (Exception e)
                    {
                        print(e);
                    }
                }

                foreach (var _client in server.Clients)
                {
                    if (_client.Id == client.Id) continue;

                    _client.ToEvent(name, data);
                }

                print($"[SERVER] OnEvent ({name}): {Encode.GetString(data)}, id: {client.Id}");
            });
        }

        #endregion
    }

    #endregion

    #region On Start

    private void Start()
    {
        server?.Open(_host.GetHost());
        return;
        /*
        client?.Open(_host.GetHost());
        */
    }

    #endregion

    #region On App Quit

    private void OnApplicationQuit()
    {
        client?.Close();
        server?.Close();
    }

    #endregion

    #region Button Event

    public void ButtonEvent(Button button, UnityAction callback)
    {
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(callback);
    }

    #endregion

    #region Start And Stop Load Canvas

    public void StartLoad()
    {
        _loadIndex++;
        loadCanvas.gameObject.SetActive(true);
    }

    public void StopLoad()
    {
        _loadIndex--;
        
        if (_loadIndex <= 0)
        {
            _loadIndex = 0;
            loadCanvas.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Start And Stop Error Canvas

    public void StartError(string title, string message)
    {
        errorTitleTXT.text = title;
        errorMessageTXT.text = message;
        errorCanvas.gameObject.SetActive(true);
    }

    public void StopError()
    {
        errorTitleTXT.text = "Error!";
        errorMessageTXT.text = "Message!";
        errorCanvas.gameObject.SetActive(false);
    }

    #endregion

    #region Get Chat Message
    int _chatMessageIndex = 0;
    public ChatMessage GetChatMessage()
    {
        _chatMessageIndex++;
        GameObject game = Instantiate(messagePrefab, messageTarget);
        game.name = $"message - {_chatMessageIndex}";
        return game.GetComponent<ChatMessage>();
    }

    #endregion

    #region Set Is Server
    
    public void SetZenetTarget(bool isServer)
    {
        GetComponent<NetlyHost>().isServer = isServer;
    }

    #endregion
}
