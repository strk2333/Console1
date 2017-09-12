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
    public void ExStart()
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
    private Socket serverSocket;
    public Server()
    {
        Init();
        Start();
    }

    public void Init()
    {
        IPAddress ip = IPAddress.Parse(_address);
        //IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        IPEndPoint ipe = new IPEndPoint(ip, _port);
        sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sSocket.Bind(ipe);
    }

    public void Start()
    {
        sSocket.Listen(0);
        Console.WriteLine("listening...");
        serverSocket = sSocket.Accept();
        Console.WriteLine("Connected");

        StartReciveSrv();
        StartSendSrv();
    }

    private void StartReciveSrv()
    {
        new Thread(delegate ()
        {
            while (true)
            {
                byte[] buf = new byte[20];
                try
                {
                    serverSocket.Receive(buf);
                    Console.Write(Encoding.UTF8.GetString(buf).Trim());
                    Console.WriteLine("");
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                    break;
                }
            }
            Console.Read();
        }).Start();
    }

    private void StartSendSrv()
    {
        new Thread(delegate ()
        {
            int tmp;
            while (true)
            {
                byte[] buf = new byte[1024];
                int len = 0;

                while ((tmp = Console.Read()) != '\r' && len != buf.Length - 1)
                {
                    buf[len] = (byte)tmp;
                    len++;
                }
                serverSocket.Send(buf, len, SocketFlags.None);
            }
        }).Start();
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
        IPAddress ip = IPAddress.Parse(_address);
        //IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        ipe = new IPEndPoint(ip, _port);
        IPEndPoint localIpe = new IPEndPoint(ip, _port + 1);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Bind(localIpe);
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Connecting...");
                clientSocket.Connect(ipe);
                Console.WriteLine("Connected");
            }
            catch (SocketException se)
            {
                Thread.Sleep(100);
                Console.WriteLine("Try to reconnect");
            }
            if (clientSocket.Connected)
                break;
        }

        //clientSocket.BeginConnect(ipe, new AsyncCallback(ConnCallBack), clientSocket);

        StartReciveSrv();
        StartSendSrv();
    }

    private void StartReciveSrv()
    {
        new Thread(delegate ()
        {
            while (true)
            {
                byte[] buf = new byte[20];
                try
                {
                    clientSocket.Receive(buf);
                    Console.Write(Encoding.UTF8.GetString(buf).Trim());
                    Console.WriteLine("");
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                    break;
                }
            }
            Console.Read();
        }).Start();
    }

    private void StartSendSrv()
    {
        new Thread(delegate ()
        {
            int tmp;
            while (true)
            {
                byte[] buf = new byte[1024];
                int len = 0;
                while ((tmp = Console.Read()) != '\r' && len != buf.Length - 1)
                {
                    buf[len] = (byte)tmp;
                    len++;
                }
                try
                {
                    clientSocket.Send(buf, len, SocketFlags.None);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.StackTrace);
                }
            }
        }).Start();
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
