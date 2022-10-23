using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netly.Core;
using Netly.Tcp;

public class PlayerMuliplayer : MonoBehaviour
{
    public PlayerInstance player;
    public TcpClient client;
    public static readonly int messagePerSecond = 10;
    public bool running = false;

    private Vector3 _position;
    private Vector3 _rotation;
    private float _delay;
    private float _time;

    private void Start()
    {
        _delay = 1f / (float)messagePerSecond;
    }

    private void Update()
    {
        if (!running) return;

        if (!client.Opened) return;

        _time += Time.deltaTime;
        
        if(_time > _delay)
        {
            if(player.position != _position || player.rotation != _rotation)
            {
                _position = player.position;
                _rotation = player.rotation;

                // create transform package
                ClientJson.Transform _transform = new ClientJson.Transform(player.userid, player.position, player.rotation);
                string _transformJson = JsonUtility.ToJson(_transform);
                byte[] _transformBytes = Encode.GetBytes(_transformJson);

                client.ToEvent("transform", _transformBytes);                
            }
        }
    }
}
