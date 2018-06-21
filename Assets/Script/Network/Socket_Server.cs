using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Text;

public class Socket_Server : MonoBehaviour
{
    public Text m_ReceiveText;
    public int m_ListenerPoint;

    private Socket m_ServerSocket;

    void Awake()
    {
        InitialSocketServer();
    }

    void OnDisable()
    {
        if (m_ServerSocket != null)
            m_ServerSocket.Close();
    }

    void InitialSocketServer()
    {
        m_ServerSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        //**绑定监听的客户端请求
        IPAddress ip = IPAddress.Parse("192.168.1.96");
        EndPoint endPoint = new IPEndPoint(ip, m_ListenerPoint);
        m_ServerSocket.Bind(endPoint); //绑定剪影这个端口
        Debug.Log("Tcp Server 启动完成");

        m_ServerSocket.Listen(100); //指定最多接收100个客户端请求
        Socket connectSocket = m_ServerSocket.Accept();  //当与客户端建立连接时候返回一个Socket通信连接
        Debug.Log("有客户端连接进来");

        byte[] connectData = Encoding.UTF8.GetBytes("我是TCP服务器，有什么需要");
        connectSocket.Send(connectData);


        byte[] receiveData = new byte[1024];
        int messageLength = connectSocket.Receive(receiveData);
        m_ReceiveText.text = Encoding.UTF8.GetString(receiveData);
        Debug.Log("接收消息长度 " + messageLength);
        if (connectSocket != null)
            connectSocket.Close();
    }

}
