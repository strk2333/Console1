using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.IO;

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
    private int _port = 8568;
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

                while ((tmp = Console.Read()) != '\r' && tmp != '\n' && len != buf.Length - 1)
                {
                    buf[len] = (byte)tmp;
                    len++;
                }

                string[] splits = mTrim(Encoding.UTF8.GetString(buf)).Split(' ');
                if (splits.Length == 2 && splits[0].ToLower() == "sendfile")
                {
                    // send file
                    try
                    {
                        serverSocket.Send(Encoding.UTF8.GetBytes("__COMMAND:sendfile".ToCharArray()));
                        Thread.Sleep(100);
                        serverSocket.SendFile(splits[1]);
                        Thread.Sleep(100);
                        serverSocket.Send(Encoding.UTF8.GetBytes("__EOF".ToCharArray()));
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    continue;
                }
                serverSocket.Send(mTrim(buf), len, SocketFlags.None);
            }
        }).Start();
    }

    private string mTrim(string s)
    {
        int index = -1;
        for (int i = s.Length - 1; i > 0; i--)
        {
            if (s[i] == 0)
                index = i;
            else
                break;
        }

        if (index != -1)
            return s.Substring(0, index);
        else
            return "";
    }

    private byte[] mTrim(byte[] s)
    {
        int index = -1;
        for (int i = s.Length - 1; i > 0; i--)
        {
            if (s[i] == 0)
                index = i;
            else
                break;
        }

        byte[] result;
        if (index != -1)
        {
            result = new byte[index];
            for (int i = 0; i < index; i++)
                result[i] = s[i];
            return result;
        }
        else
            return null;
    }
}

public class Client
{
    private string _address = "127.0.0.1";
    private int _port = 8568;
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
                Thread.Sleep(1000);
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
            StringBuilder fileBuf = new StringBuilder();
            bool readingFile = false;
            while (true)
            {
                byte[] buf = new byte[128];
                try
                {
                    clientSocket.Receive(buf);

                    if (!readingFile && ResolveCommand(mTrim(Encoding.UTF8.GetString(buf))) == "sendfile")
                    {
                        fileBuf.Clear();
                        readingFile = true;
                        Console.WriteLine("File download start.");
                        clientSocket.Receive(buf);
                    }

                    if (readingFile)
                    {
                        string s = mTrim(Encoding.UTF8.GetString(buf));
                        Console.WriteLine(s.Length);

                        if (s.Length != 0 && s != "__EOF")
                        {
                            fileBuf.Append(s);
                        }
                        else
                        {
                            Console.WriteLine("File download complete.");
                            readingFile = false;
                            FileStream f = File.Create("D:/file.txt");
                            byte[] tmp = Encoding.UTF8.GetBytes(fileBuf.ToString());
                            f.Write(tmp, 0, tmp.Length);
                            f.Close();
                        }
                    }
                    else
                    {
                        Console.Write(Encoding.UTF8.GetString(buf).Trim());
                        Console.WriteLine("");
                    }
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
                while ((tmp = Console.Read()) != '\r' && tmp != '\n' && len != buf.Length - 1)
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

    private string ResolveCommand(string s)
    {
        string[] splits = s.Split(':');
        if (splits.Length == 2 && splits[0] == "__COMMAND")
        {
            return splits[1];
        }

        return "";
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

    private string mTrim(string s)
    {
        int index = -1;
        for (int i = s.Length - 1; i > 0; i--)
        {
            if (s[i] == 0)
                index = i;
            else
                break;
        }

        if (index != -1)
            return s.Substring(0, index);
        else
            return "";
    }

}
