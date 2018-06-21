using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/// <summary>
/// ****采用多线程机制 一个线程负责监听连接一个消息负责处理
/// </summary>
public class TcpServer : MonoBehaviour
{
    [SerializeField]
    private int m_ServerPort = 60001;
    private int m_MaxListener = 100;  //最大监听数量
    private Socket m_ServerSocket = null;
    [SerializeField]
    private string m_ServerSayHello = "TcpServer Is Ready To Communicate";

    private void Awake()
    {
        InitialServer();
    }

    void OnDisable()
    {
        if (m_ServerSocket == null) return;
     //   m_ServerSocket.Shutdown(SocketShutdown.Receive);
        m_ServerSocket.Close();
    }

    void InitialServer()
    {
        m_ServerSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        IPAddress localAddress = NetHelper.GetInstance().GetLocalIpAddress();
        EndPoint endPoint = new IPEndPoint(localAddress, m_ServerPort);

        m_ServerSocket.Bind(endPoint); //绑定到指定的端口上
        m_ServerSocket.Listen(m_MaxListener);  //设置最大的监听数量
        Thread myThread = new Thread(ListenClientConnect);
        myThread.IsBackground = true;
        myThread.Start();  
    }

    /// <summary>
    /// 监听客户端链接的线程
    /// </summary>
    private void ListenClientConnect()
    {
        while(true)
        {
            if (m_ServerSocket == null) return;
            Socket client = m_ServerSocket.Accept();  //当收到客户端连接时候创建一个Socket对象继续执行
            byte[] data = Encoding.UTF8.GetBytes(m_ServerSayHello);
            client.Send(data);   //服务器发送准备好通信的消息

            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
            receiveThread.Start(client);  //创建并启动一个接受线程
        }
    }
   

    private void ReceiveMessage(object clientSocket)
    {
        if (clientSocket == null) return;
        Debug.Log("<<<<<<<<<<<<<<");
        Socket client = clientSocket as Socket;
        byte[] dataBuffer = new byte[1024];
        while(true)
        {
            try
            {
                int receiveNumber = client.Receive(dataBuffer);
                string msg = Encoding.UTF8.GetString(dataBuffer);
                Debug.Log("ReceiveMessage  From" + client.RemoteEndPoint + "   Msg:" + msg);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("ReceiveMessage Fail: " + ex.Message);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                break;
            }
        }
    }



}
