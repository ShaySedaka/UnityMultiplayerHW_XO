using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;


public enum PortConfig
{
    Port1,
    Port2
}



public class NetworkManager : Singleton<NetworkManager>
{
    private string LocalIPAddress = "127.0.0.1";
    private const int PORT1 = 40000;
    private const int PORT2 = 40001;

    private PortConfig _myPortConfig = PortConfig.Port1;
    Thread listener;
    static Queue pQueue = Queue.Synchronized(new Queue()); //this is the message queue, it is thread safe
    static UdpClient udp;
    private IPEndPoint endPoint;


    public PortConfig MyPortConfig { get => _myPortConfig; set => _myPortConfig = value; }

    // Start is called before the first frame update
    void Start()
    {
        _myPortConfig = PortConfig.Port1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePortConfig();

        //in the main thread, read the message and update the game manager
        lock (pQueue.SyncRoot)
        {
            if (pQueue.Count > 0)
            {
                object o = pQueue.Dequeue(); //Take the olders message out of the queue
                string parsedMessage = (string)o;
                GameManager.Instance.OnMessageRecieved(Int32.Parse(parsedMessage)); //Send it to the game manager
            }
        }

    }

    private void UpdatePortConfig()
    {
        bool toggleState = UIManager.Instance.PortToggle.ToggleState;

        if(toggleState == false)
        {
            MyPortConfig = PortConfig.Port1;
        }
        else
        {
            MyPortConfig = PortConfig.Port2;
        }
    }

    public void StartUDP()
    {
        endPoint = new IPEndPoint(IPAddress.Any, GetListeningPort()); //this line will listen to all IP addresses in the network
        //endPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress), ListeningPort); //this line will listen to a specific IP address
        udp = new UdpClient(endPoint);
        Debug.Log("Listening for Data on port "+ GetListeningPort() +"...");
        listener = new Thread(new ThreadStart(MessageHandler));
        listener.IsBackground = true;
        listener.Start();
    }

    void MessageHandler()
    {
        Byte[] data = new byte[0];
        while (true)
        {
            try
            {
                //Did we get a new message?
                data = udp.Receive(ref endPoint);
            }
            catch (Exception err)
            {
                //If there's a problem
                Debug.Log("Communication error, recieve data error " + err);
                udp.Close();
                return;
            }
            //Treat the new message
            string msg = Encoding.ASCII.GetString(data);
            Debug.Log("UDP incoming " + msg);
            pQueue.Enqueue(msg);
        }
    }

    private void EndUDP()
    {
        if (udp != null)
        {
            udp.Close();
        }
        if (listener != null)
        {
            listener.Abort();
        }
    }

    private void OnDestroy()
    {
        EndUDP();
    }

    public void SendUDPMessage(string message)
    {
        UdpClient send_client = new UdpClient();
        IPEndPoint send_endPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress), GetSendingPort());
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        send_client.Send(bytes, bytes.Length, send_endPoint);
        send_client.Close();
        Debug.Log("Sent message: " + message);
    }

    public int GetListeningPort()
    {
        if(_myPortConfig == PortConfig.Port1)
        {
            return PORT1;
        }
        else
        {
            return PORT2;
        }
    }

    public int GetSendingPort()
    {
        if (_myPortConfig == PortConfig.Port1)
        {
            return PORT2;
        }
        else
        {
            return PORT1;
        }
    }

    public void SendMove(int pos)
    {
        SendUDPMessage(pos.ToString());
    }

    public void StartConnection()
    {
        StartUDP();
    }
}
