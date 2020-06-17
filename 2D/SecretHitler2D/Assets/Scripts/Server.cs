using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.PackageManager;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    const int Users = 5;
    const int Port = 26000;
    int HostId;
    int WebHostID;
    const int Web = 26001;
    const int BYTE_SIZE = 1024;
    byte error;
    bool severStarted;
    private byte ReliableChannel;
    int connectionid;
    #region Monobehaviour
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    public void Update()
    {
        UpdateMEssagePump();
    }
    #endregion
    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology top = new HostTopology(cc, Users);
        HostId = NetworkTransport.AddHost(top, Port, null);
        WebHostID = NetworkTransport.AddWebsocketHost(top, Web, null);
        Debug.Log(string.Format("Puerto{0} y webport{1}",Web,Port));
        severStarted = true;


    }
    public void Shutdown()
    {
        severStarted = false;
        NetworkTransport.Shutdown();
        
    }
    public void UpdateMEssagePump()
    {
        if (!severStarted)
            return;
        int recHostId;
        int connectionId;
        int channelID;

        byte[] recBuffer = new byte[BYTE_SIZE];
        int DataSize;
       NetworkEventType type=NetworkTransport.Receive(out recHostId,out connectionId,out channelID,recBuffer,BYTE_SIZE,out DataSize,out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("User has connected", connectionId));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User has dissconected", connectionId));
                break;
            case NetworkEventType.DataEvent:
                Debug.Log(string.Format("Data", connectionId));
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMeg msg = (NetMeg)formatter.Deserialize(ms);
                OnData(connectionId,channelID,recHostId,msg);
                break;
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log(string.Format("Lmao", connectionId));
                break;

        }

    }
    public void OnData(int conectionID, int ChannelID, int recHost,NetMeg msg)
    {
        switch (msg.OP)
        {
            case NetOP.None:
                break;
            case NetOP.AnyMesage:
                ReadMessageAny(conectionID, ChannelID, recHost, (Net_MessageTest)msg);
                break;



        }
    }
    public void ReadMessageAny(int connectionId, int channelID, int recHostId, Net_MessageTest msg)
    {
        Debug.Log(msg.Test);
    }
    public void SendClient(int connwctioID,NetMeg msg)
    {

        byte[] buffer = new byte[BYTE_SIZE];
        buffer[0] = 255;
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);
        NetworkTransport.Send(HostId, connwctioID, ReliableChannel, buffer, BYTE_SIZE, out error);

    }
    public void FormatTestDataS()
    {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "Hola soy el server";
        Debug.Log(connectionid);
        SendClient(1, nt);


    }
}


