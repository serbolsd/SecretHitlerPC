using System.Collections;
using System.Collections.Generic;
using System.Data;
//using System.Diagnostics;
using UnityEngine.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
 
    const int Users = 5;
    const int Port = 26000;
    int HostId;
    string ServerIP = "127.0.0.1";
    const int Web = 26001;
    bool severStarted;
    private byte ReliableChannel;
    byte error;
    const int BYTE_SIZE = 1024;
    int connectionid;

    #region Monobehaviour
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    public void Update()
    {
        UpdateMEssagePump();
    }
    #endregion
    public void ChangeServerIP(InputField InputField)
    {
       
        Debug.Log(ServerIP);
        ServerIP = InputField.text;
        Debug.Log(ServerIP);
    }

    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology top = new HostTopology(cc, Users);

        HostId = NetworkTransport.AddHost(top, 0);
#if UNITY_WEBGL && !UNITY_EDITOR
        NetworkTransport.Connect(HostId, ServerIP, Web, 0, out error);
#else
        connectionid = NetworkTransport.Connect(HostId, ServerIP, Port, 0, out error);
#endif
        Debug.Log(string.Format("Puerto{0}", ServerIP));
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
        NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelID, recBuffer, BYTE_SIZE, out DataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("connected", connectionId));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("dissconected", connectionId));
                break;
            case NetworkEventType.DataEvent:
                Debug.Log(string.Format("Data", connectionId));
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMeg msg = (NetMeg)formatter.Deserialize(ms);
                OnData(connectionId, channelID, recHostId, msg);
                break;
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log(string.Format("Lmao", connectionId));
                break;





        }

    }
    #region Send
    public void SendeServer(NetMeg msg)
    {

        byte[] buffer = new byte[BYTE_SIZE];
        buffer[0] = 255;
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);
        NetworkTransport.Send(HostId, connectionid, ReliableChannel, buffer, BYTE_SIZE, out error);

    }
    public void OnData(int conectionID, int ChannelID, int recHost, NetMeg msg)
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
    #endregion
    public void FormatTestData()
    {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "Hola soy el cliente";
        Debug.Log(connectionid);
        SendeServer(nt);


    }
}

