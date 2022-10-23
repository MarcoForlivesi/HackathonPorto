using UnityEngine;
using Netly.Tcp;

public class PlayerInstance
{
    public int userid;
    public string username;
    public TcpClient m_client;
    public GameObject m_object;
    public Vector3 position => m_object.transform.position;
    public Vector3 rotation => m_object.transform.eulerAngles;

    /// <summary>
    /// Client Version
    /// </summary>
    public PlayerInstance(int userid, string username, GameObject m_object)
    {
        this.userid = userid;
        this.username = username;
        this.m_object = m_object;
    }
    
    /// <summary>
    /// Server Version
    /// </summary>
    public PlayerInstance(int userid, string username, GameObject m_object, TcpClient m_client)
    {
        this.userid = userid;
        this.username = username;
        this.m_object = m_object;
        this.m_client = m_client;
    }
}
