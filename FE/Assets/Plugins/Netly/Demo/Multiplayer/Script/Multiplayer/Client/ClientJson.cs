using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ClientJson
{
    [Serializable]
    public class LoginList
    {
        public List<Login> logins;

        public LoginList(List<Login> logins)
        {
            this.logins = logins;
        }
    }


    [Serializable]
    public class Login
    {
        public int id;
        public string username;
        public Vector3 position;
        public Vector3 rotation;

        /// <summary>
        /// Client Version
        /// </summary>
        public Login(int id, string username)
        {
            this.id = id;
            this.username = username;
        }

        /// <summary>
        /// Server Version
        /// </summary>
        public Login(int id, string username, Vector3 position, Vector3 rotation)
        {
            this.id = id;
            this.username = username;
            this.position = position;
            this.rotation = rotation;
        }
    }

    [Serializable]
    public class Logout
    {
        public int id;

        public Logout(int id)
        {
            this.id = id;
        }
    }

    [Serializable]
    public class Transform
    {
        public int id;
        public Vector3 position;
        public Vector3 rotation;

        public Transform(int id, Vector3 position, Vector3 rotation)
        {
            this.id = id;
            this.position = position;
            this.rotation = rotation;
        }
    }

}
