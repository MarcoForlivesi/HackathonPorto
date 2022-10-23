using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Netly.Tcp;
using Netly.Core;
using System;

public class DataInspector : MonoBehaviour
{
    public TMP_InputField ipaddressIF, portIF;
    public Toggle echoTG, broadcastTG;
    public Button connectBTN;
    public GameObject loginObj;
    public TcpServer server;
    [Space]
    public Transform messageTarget;
    public GameObject messagePrefab;

    private void Awake()
    {
        loginObj.gameObject.SetActive(true);
        server = new TcpServer();

        connectBTN.onClick = new Button.ButtonClickedEvent();
        connectBTN.onClick.AddListener(() =>
        {
            try
            {
                server.Open(new Host(ipaddressIF.text, int.Parse(portIF.text)));

            }
            catch(Exception e)
            {
                DrawLog($"[SERVER] error on parse port: {e.Message}").color = Color.red;
            }
        });

        server.OnOpen(() =>
        {
            print("OnOpen");
            DrawLog($"[SERVER] connection opened at {server.Host}").color = Color.green;
            loginObj.gameObject.SetActive(false);
        });

        server.OnClose(() =>
        {
            print("OnClose");
            DrawLog($"[SERVER] connection closed at {server.Host}").color = Color.yellow;
            loginObj.gameObject.SetActive(true);
            connectBTN.interactable = true;
        });

        server.OnError((e) =>
        {
            print("OnError: " + e.Message);
            DrawLog($"[SERVER] error on connect: {e.Message}").color = Color.red;
            loginObj.gameObject.SetActive(true);
            connectBTN.interactable = true;
        });

        server.OnEnter((client) =>
        {
            DrawLog($"[CLIENT] client connected: {client.Host} | {client.Id}");
        });

        server.OnExit((client) =>
        {
            DrawLog($"[CLIENT] client closed: {client.Host} | {client.Id}");
        });

        server.OnData((client, data) =>
        {
            DrawLog($"[CLIENT] client data (host: {client.Host} & data: {Encode.GetString(data)})");

            if (broadcastTG.isOn)
            {
                foreach (var _client in server.Clients) _client.ToData(data);
            }
            else if (echoTG.isOn)
            {
                client.ToData(data);
            }
        });

        server.OnEvent((client, name, data) =>
        {

            if (broadcastTG.isOn)
            {
                foreach (var _client in server.Clients) _client.ToEvent(name, data);
            }
            else if (echoTG.isOn)
            {
                client.ToEvent(name, data);
            }
        });
    }


    public TextMeshProUGUI DrawLog(string log)
    {
        GameObject game = Instantiate(messagePrefab, messageTarget);
        TextMeshProUGUI text = game.gameObject.GetComponent<TextMeshProUGUI>();
        text.text = log;
        return text;
    }

    private void OnApplicationQuit()
    {
        server?.Close();
    }
}
