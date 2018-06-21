using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Socket_Client : MonoBehaviour
{
    public Button m_SendeBtn;
    public Text m_SendText;
    public string m_ServerAddress;
    public int m_ServerPort;
    private Socket m_ClientSocket;

    private void Awake()
    {
        m_SendeBtn.onClick.AddListener(SendMsg);
        InitialSocketClient();
    }
    void OnDisable()
    {
        if (m_ClientSocket!=null)
        {
            m_ClientSocket.Close();
        }
    }

    void InitialSocketClient()
    {
        m_ClientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        m_ClientSocket.Connect(IPAddress.Parse(m_ServerAddress), m_ServerPort);
    }


    void SendMsg()
    {
        byte[]     data = Encoding.UTF8.GetBytes(m_SendText.text);
        m_ClientSocket.Send(data);
        Debug.Log("SendMsg Length:" + data.Length);
    }

   
}
