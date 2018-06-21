using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TcpClient : MonoBehaviour
{
    [SerializeField]
    private string m_ServerAddress="192.168.1.96";
    [SerializeField]
    private int m_ServerPort= 60001;
    private Socket m_ClientSocket = null;
    private byte[] dataReceive = new byte[1024];
    [SerializeField]
    private string m_SendMsg = "";

    private void Start  ()
    {
        InitialClient();

    }
    void OnDisable()
    {
        if (m_ClientSocket == null) return;
        m_ClientSocket.Shutdown(SocketShutdown.Both);
        m_ClientSocket.Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendMsg();
        }
    }

    void InitialClient()
    {
        m_ClientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        IPAddress ip =  IPAddress.Parse(m_ServerAddress);
        EndPoint endPoint = new IPEndPoint(ip,m_ServerPort);
        try
        {
            m_ClientSocket.Connect(endPoint);  //链接服务器
            Debug.Log("Connect To Server...");
            Thread clientReceive = new Thread(ReceiveMessage);
            clientReceive.Start();
        }
        catch (System.Exception ex)
        {
            Debug.Log("InitialClient Fail");
            m_ClientSocket.Shutdown(SocketShutdown.Both);
          //  m_ClientSocket.Dispose();
            m_ClientSocket.Close();
        }
    }

    void ReceiveMessage() {
        while (true)
        {
            if (m_ClientSocket == null) return;
            int receiveNumber = m_ClientSocket.Receive(dataReceive);
            string msg = Encoding.UTF8.GetString(dataReceive);
            Debug.Log("ReceiveMessage::: " + msg);
        }
    }

    void SendMsg()
    {
        if (m_ClientSocket == null) return;
        byte[] data = Encoding.UTF8.GetBytes(m_SendMsg);
        m_ClientSocket.Send(data);
        Debug.Log("SendMsg");
    }
}
