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
    #region User

    [Header("User Info")]
    public bool generateUser = true;
    public int userid = -1;
    public string username = "[ Zenet ]";

    #endregion

    [Header("Try Open Connection On Game Start")]
    public bool connectOnStart = true;
    public TcpClient client;
    private NetlyHost _host;
    public GameObject playerMainPrefab;
    public GameObject playerInstancePrefab;
    public List<PlayerInstance> players = new List<PlayerInstance>();

    #region Flow

    private void Awake()
    {
        _host = GetComponent<NetlyHost>();

        client = new TcpClient();

        client.OnOpen(OnOpen);
        client.OnError(OnError);
        client.OnClose(OnClose);
        client.OnEvent(OnEvent);

        if (generateUser)
        {
            userid = new System.Random().Next(short.MaxValue, int.MaxValue);
            username = $"Player {userid}";
        }
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
        Debug.Log($"[ Zenet Client ] OnOpen");

        // create my json data
        ClientJson.Login loginObject = new ClientJson.Login(userid, username);
        string loginString = JsonUtility.ToJson(loginObject);
        byte[] loginBytes = Encode.GetBytes(loginString);

        // send login data
        client.ToEvent("login", loginBytes);
    }

    private void OnError(Exception e)
    {
        Debug.LogError($"[ Zenet Client ] OnError/OnClose: {e.Message}");
    }

    private void OnClose()
    {
        Debug.LogWarning($"[ Zenet Client ] OnClose");

        // destroy all client instance
        foreach(PlayerInstance player in players)
        {
            if (player != null)
            {
                Destroy(player.m_object);
            }
        }

        // remove all object on list
        players.Clear();
    }

    private void OnEvent(string name, byte[] data)
    {
        Debug.Log($"[ Zenet Client ] OnEvent ({name}): {Encode.GetString(data)}");

        switch (name)
        {
            case "login":
                Client_EventLogin(data);
                break;
            
            case "transform":
                Client_EventTransform(data);
                break;
        }
    }

    #region Server Events

    private void Client_EventLogin(byte[] data)
    {
        try
        {
            string jsonString = Encode.GetString(data);

            // get list of login
            ClientJson.LoginList logins = JsonUtility.FromJson<ClientJson.LoginList>(jsonString);

            // if not exist login, brake!
            if (logins.logins.Count <= 0) return;

            foreach(var login in logins.logins)
            {
                bool jump = false;

                // check, verify if current login "player" exist on world!
                foreach(var player in players)
                {
                    if (login.id == player.userid)
                    {
                        // brake! user exist
                        jump = true;
                        break;
                    }
                }

                if (jump) continue;

                /*
                    if current player is mine: object for instance is [playerMainPrefab]
                    if not is [playerInstancePrefab]
                */
                GameObject prefab = (login.id == userid) ? playerMainPrefab : playerInstancePrefab;


                // current user not exist ( instance current user now! )
                // -- create object
                GameObject newPlayer = Instantiate(prefab, login.position, Quaternion.Euler(login.rotation), null);
                newPlayer.name = client.Id;

                // create player instance
                PlayerInstance newPlayerInstance =
                    new PlayerInstance(login.id, login.username, newPlayer);

                // add player instance on list
                players.Add(newPlayerInstance);

                if (login.id == userid)
                {
                    // add script for auto send position on change location
                    newPlayer.AddComponent<PlayerMuliplayer>();
                    PlayerMuliplayer playerMuliplayer = newPlayer.GetComponent<PlayerMuliplayer>();
                    // -- add value in options
                    playerMuliplayer.client = client;
                    playerMuliplayer.player = newPlayerInstance;
                    playerMuliplayer.running = true;
                }
                else
                {
                    // add scrit for auto smooth transform
                    newPlayer.AddComponent<SmoothInstance>();
                }

            }
            
        }
        catch (Exception e)
        {
            Debug.LogError($"[ Zenet Client ]{nameof(Client_EventLogin)} : {e}");
        }
    }

    private void Client_EventTransform(byte[] data)
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

                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[ Zenet Server ]{nameof(Client_EventTransform)} : {e}");
        }
    }

    #endregion

    #endregion
}
