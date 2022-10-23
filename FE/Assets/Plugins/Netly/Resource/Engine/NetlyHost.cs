using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Netly.Core;

namespace Netly.Unity
{
    public class NetlyHost : MonoBehaviour
    {
        public bool changedObjName = false;
        public string objName = "[ Host ]";

        public enum Status { Connected, Disconnect }

        // endpoint info 
        public int port = 8080;
        public string ip = "127.0.0.1";

        //server info
        public bool isServer;
        public int maxClient;
        public int backlog;
        public int clientConnected;
        public int clientDisconnected;

        public Host GetHost()
        {
            return new Host(ip, port);
        }
    }
}
