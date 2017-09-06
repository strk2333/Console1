using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

public class NetExample
{
    public void NetEx()
    {
        new Thread(delegate ()
        {
            Server s = new Server();
        }).Start();

        new Thread(delegate ()
        {
            Client c = new Client();
        }).Start();
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
        Console.WriteLine("listening...");
        Socket serverSocket = sSocket.Accept();
        Console.WriteLine("Connected");

        byte[] buf = new byte[1024];
        while (buf[0] == 0)
        {
            serverSocket.ReceiveBufferSize = 1024;
            serverSocket.Receive(buf);
            Console.Write(Encoding.UTF8.GetString(buf));
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
        //clientSocket.BeginConnect(ipe, new AsyncCallback(ConnCallBack), clientSocket);

        string msg = "你好";
        clientSocket.Send(Encoding.UTF8.GetBytes(msg));

        byte[] buf = new byte[1024];
        clientSocket.Receive(buf);
        Console.WriteLine(Encoding.UTF8.GetString(buf));
    }

    private void ConnCallBack(IAsyncResult ar)
    {
        try
        {
            Socket handler = ar.AsyncState as Socket;
            handler.EndSend(ar);
        }
        catch (SocketException se)
        {

        }
    }

    private void boo()
    {

    }

}
