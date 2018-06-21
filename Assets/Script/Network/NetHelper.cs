using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

public class NetHelper
{
    private static NetHelper instance = null;
    public static NetHelper GetInstance()
    {
        if (instance == null)
            instance = new NetHelper();
        return instance;
    }





    #region  获得本机一个可用的端口号  避免使用已经被设置的端口
    /// <summary> 
    /// 获取第一个可用的端口号 
    /// </summary> 
    /// <returns></returns> 
    public int GetFirstAvailablePort()
    {
        int MAX_PORT = 65535; //系统tcp/udp端口数最大是65535 
        int BEGIN_PORT = 5000;//从这个端口开始检测 

        for (int i = BEGIN_PORT; i < MAX_PORT; i++)
        {
            if (PortIsAvailable(i)) return i;
        }

        return -1;
    }

    /// <summary> 
    /// 获取操作系统已用的端口号 
    /// </summary> 
    /// <returns></returns> 
    private IList PortIsUsed()
    {
        //获取本地计算机的网络连接和通信统计数据的信息 
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

        //返回本地计算机上的所有Tcp监听程序 
        IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

        //返回本地计算机上的所有UDP监听程序 
        IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

        //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。 
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        IList allPorts = new ArrayList();
        foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
        foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
        foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

        return allPorts;
    }

    /// <summary> 
    /// 检查指定端口是否已用
    /// </summary> 
    /// <param name="port"></param> 
    /// <returns></returns> 
    private bool PortIsAvailable(int port)
    {
        bool isAvailable = true;

        IList portUsed = PortIsUsed();

        foreach (int p in portUsed)
        {
            if (p == port)
            {
                isAvailable = false; break;
            }
        }

        return isAvailable;
    }
    #endregion

    #region  获得本地的Ipv4 地址
    /// <summary>
    /// 根据本机名获取一个指定的Ipv4地址
    /// </summary>
    /// <returns></returns>
    public IPAddress GetLocalIpAddress()
    {
        string hostName = Dns.GetHostName(); //获取本几名
        IPHostEntry localHost = Dns.GetHostEntry(hostName);   //获取Ip地址

        foreach (var ip in localHost.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)  //获取Ipv4地址
                return ip;
        }
        Debug.LogError("GetLocalIpAddress Fail");
        return null;
    }
    #endregion


}
