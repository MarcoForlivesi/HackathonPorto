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
    public GameObject playerInstancePrefab;
    public List<PlayerInstance> players = new List<PlayerInstance>();


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
        server.OnEvent(OnEvent);
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

    private void OnEvent(TcpClient client, string name, byte[] data)
    {
        Debug.Log($"[ Zenet Server ] OnEvent ({name}): {Encode.GetString(data)}");

        switch (name)
        {
            case "login":
                Client_EventLogin(client, data);
                break;

            case "transform":
                Client_EventTransform(client, data);
                break;
        }
    }

    private void OnExit(object obj)
    {
        Debug.LogWarning($"[ Zenet Server ] OnExit");
    }

    #region Client Events

    private void Client_EventLogin(TcpClient client, byte[] data)
    {
        try
        {
            string loginString = Encode.GetString(data);

            ClientJson.Login loginObject = JsonUtility.FromJson<ClientJson.Login>(loginString);

            // verify if exist
            bool exist = false;

            foreach (PlayerInstance player in players)
            {
                if (player.userid == loginObject.id)
                {
                    exist = true;
                    break;
                }
            }

            // if exist! close this player
            if (exist)
            {
                client.Close();
                return;
            }

            // create this player

            // -- create random position and rotation
            Vector3 randomPos = new Vector3(Random.Range(-7, 7), 2, Random.Range(-7, 7));
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // -- create object
            GameObject newPlayer = Instantiate(playerInstancePrefab, randomPos, randomRot, null);
            newPlayer.name = client.Id;

            // create player instance
            PlayerInstance newPlayerInstance =
                new PlayerInstance(loginObject.id, loginObject.username, newPlayer, client);

            // add player instance on list
            players.Add(newPlayerInstance);

            // get all player
            List<ClientJson.Login> logins = 
                players.Select(player => new ClientJson.Login(player.userid, player.username, player.position, player.rotation)).ToList();

            // convert all client to json
            ClientJson.LoginList loginList = new ClientJson.LoginList(logins);
            string loginsJson = JsonUtility.ToJson(loginList);
            print($"Logins: {loginsJson}, Logins Len: {logins.Count}, players Len: {players.Count}");
            byte[] loginJsonBytes = Encode.GetBytes(loginsJson);

            // add scrit for auto smooth transform
            newPlayer.AddComponent<SmoothInstance>();

            /*
                WARNING: this is not finaly broadcast sample
                    
                FUTURE BROADCAST IS:
                                    server.BroadcastData(byte[] data);
                                    server.BroadcastEvent(string name, byte[] data);
             
            */

            // broadcast new player list
            foreach (var player in players)
            {
                player.m_client.ToEvent("login", loginJsonBytes);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[ Zenet Server ]{nameof(Client_EventLogin)} : {e}");
        }
    }

    private void Client_EventTransform(TcpClient client, byte[] data)
    {
        try
        {
            string transformString = Encode.GetString(data);

            ClientJson.Transform transformObject = JsonUtility.FromJson<ClientJson.Transform>(transformString);

            foreach (PlayerInstance player in players)
            {
                if (player.userid == transformObject.id)
                {
                    // update position of this player
                    SmoothInstance smooth = player.m_object.GetComponent<SmoothInstance>();
                    smooth.position = transformObject.position;
                    smooth.rotation = transformObject.rotation;


                    // multicast this data
                    foreach (var _player in players)
                    {
                        // not send this data for owner of this data
                        if (_player.userid == transformObject.id) continue;

                        // send a data to current client on loop
                        _player.m_client.ToEvent("transform", data);
                    }

                    return;
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"[ Zenet Server ]{nameof(Client_EventTransform)} : {e}");
        }
    }

    #endregion

    #endregion

    #endregion
}
