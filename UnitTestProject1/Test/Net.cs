using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

[TestClass]
public class Net
{
    [Fact]
    public void NetEx()
    {
        new Thread(delegate ()
        {
            Client c = new Client();
        }).Start();

        Server s = new Server();
    }
}

public class Server
{
    private string _address = "127.0.0.1";
    private int _port = 6000;
    public Server()
    {
        //IPAddress ip = IPAddress.Parse(_address);
        IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        IPEndPoint ipe = new IPEndPoint(ip, _port);
        Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sSocket.Bind(ipe);
        sSocket.Listen(0);
        Debug.WriteLine("listening...");
        Socket serverSocket = sSocket.Accept();
        Debug.WriteLine("Connected");

        byte[] buf = new byte[1024];
        while (buf[0] == 0)
        {
            serverSocket.ReceiveBufferSize = 1024;
            serverSocket.Receive(buf);
            Debug.Write(Encoding.UTF8.GetString(buf));
        }

        serverSocket.Send(Encoding.UTF8.GetBytes("" + serverSocket.RemoteEndPoint));
        
    }

}

public class Client
{
    private string _address = "127.0.0.1";
    private int _port = 6000;

    public Client()
    {
        //IPAddress ip = IPAddress.Parse(_address);
        IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        IPEndPoint ipe = new IPEndPoint(ip, _port);
        IPEndPoint localIpe = new IPEndPoint(ip, _port + 1);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Bind(localIpe);
        clientSocket.Connect(ipe);

        string msg = "你好";
        clientSocket.Send(Encoding.UTF8.GetBytes(msg));

        byte[] buf = new byte[1024];
        clientSocket.Receive(buf);
        Debug.WriteLine("\n" + Encoding.UTF8.GetString(buf));
    }

}
