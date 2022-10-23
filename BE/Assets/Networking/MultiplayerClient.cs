using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netly.Tcp;
using Netly.Core;
using Netly.Unity;

[RequireComponent(typeof(NetlyHost))]
public class MultiplayerClient : MonoBehaviour
{
    [Header("Try Open Connection On Game Start")]
    public bool connectOnStart = true;
    public TcpClient client;
    private NetlyHost _host;

    #region Flow

    private void Awake()
    {
        _host = GetComponent<NetlyHost>();

        client = new TcpClient();

        client.OnOpen(OnOpen);
        client.OnError(OnError);
        client.OnClose(OnClose);
        client.OnEvent(OnEvent);
    }

    private void Start()
    {
        if (connectOnStart)
        {
            Open();
        }
    }

    private void OnApplicationQuit()
    {
        Close();
    }

    #endregion

    #region Trigger

    public void Open()
    {
        client.Open(_host.GetHost());
    }

    public void Close()
    {
        client.Close();
    }

    #endregion

    #region Zenet

    private void OnOpen()
    {
        Debug.LogWarning($"[ Zenet Client ] OnOpen");
    }

    private void OnError(Exception e)
    {
        Debug.LogError($"[ Zenet Client ] OnError/OnClose: {e.Message}");
    }

    private void OnClose()
    {
        Debug.LogWarning($"[ Zenet Client ] OnClose");
    }

    private void OnEvent(string name, byte[] data)
    {
        Debug.Log($"[ Zenet Client ] OnEvent ({name}): {Encode.GetString(data)}");
    }

    #region Server Events

    #endregion

    #endregion
}
