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
        Client c;
        Server s;
        if (Console.Read() == '1')
            c = new Client();
        else
            s = new Server();
    }
}

public class Server
{
    private string _address = "127.0.0.1";
    private int _port = 6000;
    private Socket sSocket;
    public Server()
    {
        Init();
        Start();
    }

    public void Init()
    {
        //IPAddress ip = IPAddress.Parse(_address);
        IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        IPEndPoint ipe = new IPEndPoint(ip, _port);
        sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sSocket.Bind(ipe);
    }

    public void Start()
    {
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
    private Socket clientSocket;
    private IPEndPoint ipe;

    public Client()
    {
        Init();
        Start();
    }

    public void Init()
    {
        //IPAddress ip = IPAddress.Parse(_address);
        IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        ipe = new IPEndPoint(ip, _port);
        IPEndPoint localIpe = new IPEndPoint(ip, _port + 1);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Bind(localIpe);
    }

    public void Start()
    {
        clientSocket.Connect(ipe);

        string msg = "你好";
        clientSocket.Send(Encoding.UTF8.GetBytes(msg));

        byte[] buf = new byte[1024];
        clientSocket.Receive(buf);
        Debug.WriteLine("\n" + Encoding.UTF8.GetString(buf));
    }

}
