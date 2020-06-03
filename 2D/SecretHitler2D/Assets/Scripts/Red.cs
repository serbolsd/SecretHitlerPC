
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEditor.PackageManager;
using System.Threading;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
/*
public struct ConnectionInfo
{
    public string DireccionIp;
    public int Port;
    public string Name;
    public ConnectionInfo(string mAddress, string mData)
    {
        DireccionIp = mAddress.Substring(mAddress.LastIndexOf(":") + 1, mAddress.Length - (mAddress.LastIndexOf(":") + 1));
        string PortText = mData.Substring(mData.LastIndexOf(":") + 1, mData.Length - (mData.LastIndexOf(":") + 1));
        Port = 7777;
        Name = "local";
    }
}

//fsdfsdf
public class Red : NetworkDiscovery
{
    public float TimeOut = 5.0f;

    private Dictionary<ConnectionInfo, float> LanAdresses = new Dictionary<ConnectionInfo, float>();
    string Adrress;
    private void Awake()
    {
        base.Initialize();
        base.StartAsClient();
        //StartCoroutine(CleanupExpiredEntries());
    }

     public void StartBroadcast()
    {
        StopBroadcast();
        base.Initialize();
        base.StartAsServer();

    }
  // private IEnumerator CleanupExpiredEntries()
  // {
  //    // while(true)
  //    // {
  //    //     bool changed = false;
  //    //     var keys = LanAdresses.Keys.ToList();
  //    //     foreach(var key in keys)
  //    //     {
  //    //         if(LanAdresses[key]<=Time.time)
  //    //         {
  //    //             LanAdresses.Remove(key);
  //    //             changed = true;
  //    //         }
  //    //         if (changed)
  //    //
  //    //             yield return new WaitForSeconds(TimeOut);
  //    //
  //    //     }
  //    //
  //    //
  //    // }
  //
  //
  // }
  

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        ConnectionInfo info = new ConnectionInfo(fromAddress,data);
        Adrress = fromAddress;
        if(LanAdresses.ContainsKey(info)==false)
        {
            LanAdresses.Add(info, Time.time + TimeOut);
           
        }
        else
        {
            LanAdresses[info] = Time.time + TimeOut;
        }

    }





    public void RecieveMsg()
    {

        Debug.Log("Consss");
        base.OnReceivedBroadcast(Adrress,base.broadcastData);
    }

}
*/
public class Red : NetworkDiscovery
{

    UdpClient sender;
    int remotePort = 19784;
    void Start()
    {

        sender = new UdpClient(7777, AddressFamily.InterNetwork);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
        sender.Connect(groupEP);

        //SendData ();
        InvokeRepeating("SendData", 0, 5f);

    }
    void SendData()
    {
        string customMessage = "Chong" + " * " + "192.168.0.1" + " * " + "Pito";

        if (customMessage != "")
        {
            sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
        }
    }
    UdpClient receiver;

    public void StartReceivingIP()
    {
        try
        {
            if (receiver == null)
            {
                receiver = new UdpClient(remotePort);
                receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }
    private void ReceiveData(IAsyncResult result)
    {
        receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
        byte[] received;
        if (receiver != null)
        {
            received = receiver.EndReceive(result, ref receiveIPGroup);
        }
        else
        {
            return;
        }
        receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
        string receivedString = Encoding.ASCII.GetString(received);
    }
}