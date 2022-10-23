using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netly.Core;
using Netly.Tcp;
using Netly.Unity;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NetlyHost))]
public class MultiplayerServer : MonoBehaviour
{
    [Header("Try Open Connection On Game Start")]
    public bool connectOnStart = true;
    public TcpServer server;
    private NetlyHost _host;

    #region Flow

    private void Awake()
    {
        _host = GetComponent<NetlyHost>();

        server = new TcpServer();

        //Server event
        server.OnOpen(OnOpen);
        server.OnError(OnError);
        server.OnClose(OnClose);

        //Client event
        server.OnEnter(OnEnter);
        server.OnExit(OnExit);
    }

    private void Start()
    {
        if (connectOnStart)
        {
            Open();
        }
    }

    #endregion

    #region Trigger

    public void Open()
    {
        server.Open(_host.GetHost());
    }

    public void Close()
    {
        server.Close();
    }

    private void OnApplicationQuit()
    {
        Close();
    }

    #endregion

    #region Zenet

    #region Server

    private void OnOpen()
    {
        Debug.Log($"[ Zenet Server ] OnOpen");
    }

    private void OnError(Exception e)
    {
        Debug.LogError($"[ Zenet Server ] OnError/OnClose: {e.Message}");
    }

    private void OnClose()
    {
        Debug.LogWarning($"[ Zenet Server ] OnClose");
    }

    #endregion

    #region Client

    private void OnEnter(TcpClient client)
    {
        Debug.Log($"[ Zenet Server ] OnEnter");
    }

    private void OnExit(object obj)
    {
        Debug.LogWarning($"[ Zenet Server ] OnExit");
    }


    #endregion

    #endregion
}
