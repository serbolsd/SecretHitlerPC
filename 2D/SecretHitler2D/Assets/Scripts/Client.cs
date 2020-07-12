using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
  const int Users = 5;
  const int Port = 26000;
  int HostId;
  string ServerIP = "26.53.249.17";
  const int Web = 26001;
  bool severStarted;
  private byte ReliableChannel;
  byte error;
  const int BYTE_SIZE = 1024;
  public int connectionid;
  public int myId;

  public sesionLegislativa refSesionLegislativa;
  public SessionEjecutiva refSesionEjecutiva;
  public Elecciones refElecciones;
  public Ronda refRonda;
  public bool bContinueGame = false;
  public bool bInGameScene = false;
  public bool wait = false;
  public bool connected = false;

  public int m_numPlayers;

  public InputField m_ipInput;
  public InputField m_myApodo;
  bool DoOnce = false;
  bool debug = false;
  public GameObject connectButtom;

  public Dictionary<int, string> playerApode = new Dictionary<int, string>();

  public void ChangeServerIP(InputField InputField)
  {
    if (DoOnce == false)
    {
      if (Input.GetKey(KeyCode.Return))
      {
        Debug.Log(ServerIP);
        ServerIP = InputField.text;
        DoOnce = true;
        debug = true;
      }
      if (debug == true)
      {
        Debug.Log(ServerIP);
        Debug.Log("DDOS Attack");
      }
      Init();
    }
  }

  public void conectToServer()
  {
    ServerIP = m_ipInput.text;
    Debug.Log(ServerIP);
    Init();
  }

  public void Init()
  {
    if (connected)
    {
      return;
    }
    DontDestroyOnLoad(gameObject);
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
    ColorBlock color = connectButtom.GetComponent<Button>().colors;
    if (NetworkError.Ok != (NetworkError)error)
    {
      color.normalColor = Color.red;
      color.selectedColor = Color.red;
      connectButtom.GetComponent<Button>().colors = color;
      Debug.Log("couldn't connectButtom");
      return;
    }
    else
    {
      color.normalColor = Color.green;
      color.selectedColor = Color.green;
      connectButtom.GetComponent<Button>().colors = color;
      Debug.Log("connected");
      connected = true;
      
    }
    Debug.Log(string.Format("Puerto{0}", ServerIP));
    severStarted = true;
    Net_MessageTest nt = new Net_MessageTest();
    nt.Test = "apodo_";
    nt.Test += m_myApodo.text;
    Debug.Log(nt.Test);
    SendeServer(nt);

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
        //Debug.Log(string.Format("Data", connectionId));
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
    if (!bInGameScene)
    {
      if (msg.Test.Contains("yourid"))
      {
        string[] chek = msg.Test.Split('_');
        int.TryParse(chek[1], out myId);
      }
      if (msg.Test.Contains("apodo"))
      {
        string[] chek = msg.Test.Split('_');
        if (msg.Test.Contains("apodo0"))
          playerApode.Add(0,chek[1]);
        if (msg.Test.Contains("apodo1"))
          playerApode.Add(1,chek[1]);
        if (msg.Test.Contains("apodo2"))
          playerApode.Add(2,chek[1]);
        if (msg.Test.Contains("apodo3"))
          playerApode.Add(3,chek[1]);
        if (msg.Test.Contains("apodo4"))
          playerApode.Add(4,chek[1]);
        if (msg.Test.Contains("apodo5"))
          playerApode.Add(5, chek[1]);
        if (msg.Test.Contains("apodo6"))
          playerApode.Add(6, chek[1]);
        if (msg.Test.Contains("apodo7"))
          playerApode.Add(7, chek[1]);
        if (msg.Test.Contains("apodo8"))
          playerApode.Add(8, chek[1]);
        if (msg.Test.Contains("apodo9"))
          playerApode.Add(9, chek[1]);
      }
      if (msg.Test.Contains("initGame"))
      {
        bInGameScene = true;
        if (msg.Test.Contains("1play"))
          m_numPlayers = 1;
        if (msg.Test.Contains("2play"))
          m_numPlayers = 2;
        if (msg.Test.Contains("3play"))
          m_numPlayers = 3;
        if (msg.Test.Contains("4play"))
          m_numPlayers = 4;
        if (msg.Test.Contains("5play"))
          m_numPlayers = 5;
        if (msg.Test.Contains("6play"))
          m_numPlayers = 6;
        if (msg.Test.Contains("7play"))
          m_numPlayers = 7;
        if (msg.Test.Contains("8play"))
          m_numPlayers = 8;
        if (msg.Test.Contains("9play"))
          m_numPlayers = 9;
        if (msg.Test.Contains("10play"))
          m_numPlayers = 10;
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "Okey";
        SendeServer(nt);

        SceneManager.LoadScene("GameServer");
      }
    }
    else
    {
      if (msg.Test.Contains("ronda0"))
      {
        refRonda.phase = 0;
        if (msg.Test.Contains("elec0"))
        {
          if (msg.Test.Contains("continue"))
          {
            wait = false;
          }
          refElecciones.g_phase = 0;
          refElecciones.g_isPhase = false;
          refElecciones.waitingNextTurn = false;
          refSesionLegislativa.alredySelectedCard = false;
          refSesionLegislativa.faseAlready = false;
          if (msg.Test.Contains("presi-1"))
            refElecciones.g_idPresident = -1;
          if (msg.Test.Contains("presi0"))
            refElecciones.g_idPresident = 0;
          if (msg.Test.Contains("presi1"))
            refElecciones.g_idPresident = 1;
          if (msg.Test.Contains("presi2"))
            refElecciones.g_idPresident = 2;
          if (msg.Test.Contains("presi3"))
            refElecciones.g_idPresident = 3;
          if (msg.Test.Contains("presi4"))
            refElecciones.g_idPresident = 4;
          if (msg.Test.Contains("presi5"))
            refElecciones.g_idPresident = 5;
          if (msg.Test.Contains("presi6"))
            refElecciones.g_idPresident = 6;
          if (msg.Test.Contains("presi7"))
            refElecciones.g_idPresident = 7;
          if (msg.Test.Contains("presi8"))
            refElecciones.g_idPresident = 8;
          if (msg.Test.Contains("presi9"))
            refElecciones.g_idPresident = 9;

          if (msg.Test.Contains("oldpresi-1"))
            refElecciones.g_idOldPresident = -1;
          if (msg.Test.Contains("oldpresi0"))
            refElecciones.g_idOldPresident = 0;
          if (msg.Test.Contains("oldpresi1"))
            refElecciones.g_idOldPresident = 1;
          if (msg.Test.Contains("oldpresi2"))
            refElecciones.g_idOldPresident = 2;
          if (msg.Test.Contains("oldpresi3"))
            refElecciones.g_idOldPresident = 3;
          if (msg.Test.Contains("oldpresi4"))
            refElecciones.g_idOldPresident = 4;
          if (msg.Test.Contains("oldpresi5"))
            refElecciones.g_idOldPresident = 5;
          if (msg.Test.Contains("oldpresi6"))
            refElecciones.g_idOldPresident = 6;
          if (msg.Test.Contains("oldpresi7"))
            refElecciones.g_idOldPresident = 7;
          if (msg.Test.Contains("oldpresi8"))
            refElecciones.g_idOldPresident = 8;
          if (msg.Test.Contains("oldpresi9"))
            refElecciones.g_idOldPresident = 9;

          if (msg.Test.Contains("oldcanci-1"))
            refElecciones.g_idOldChancellor = -1;
          if (msg.Test.Contains("oldcanci0"))
            refElecciones.g_idOldChancellor = 0;
          if (msg.Test.Contains("oldcanci1"))
            refElecciones.g_idOldChancellor = 1;
          if (msg.Test.Contains("oldcanci2"))
            refElecciones.g_idOldChancellor = 2;
          if (msg.Test.Contains("oldcanci3"))
            refElecciones.g_idOldChancellor = 3;
          if (msg.Test.Contains("oldcanci4"))
            refElecciones.g_idOldChancellor = 4;
          if (msg.Test.Contains("oldcanci5"))
            refElecciones.g_idOldChancellor = 5;
          if (msg.Test.Contains("oldcanci6"))
            refElecciones.g_idOldChancellor = 6;
          if (msg.Test.Contains("oldcanci7"))
            refElecciones.g_idOldChancellor = 7;
          if (msg.Test.Contains("oldcanci8"))
            refElecciones.g_idOldChancellor = 8;
          if (msg.Test.Contains("oldcanci9"))
            refElecciones.g_idOldChancellor = 9;
        }
        else if (msg.Test.Contains("elec1"))
        {
          refElecciones.hidePlayers();
          refElecciones.g_phase = 1;
          refElecciones.g_isPhase = false;
          if (msg.Test.Contains("canci-1"))
            refElecciones.g_idChancellor = -1;
          if (msg.Test.Contains("canci0"))
            refElecciones.g_idChancellor = 0;
          if (msg.Test.Contains("canci1"))
            refElecciones.g_idChancellor = 1;
          if (msg.Test.Contains("canci2"))
            refElecciones.g_idChancellor = 2;
          if (msg.Test.Contains("canci3"))
            refElecciones.g_idChancellor = 3;
          if (msg.Test.Contains("canci4"))
            refElecciones.g_idChancellor = 4;
          if (msg.Test.Contains("canci5"))
            refElecciones.g_idChancellor = 5;
          if (msg.Test.Contains("canci6"))
            refElecciones.g_idChancellor = 6;
          if (msg.Test.Contains("canci7"))
            refElecciones.g_idChancellor = 7;
          if (msg.Test.Contains("canci8"))
            refElecciones.g_idChancellor = 8;
          if (msg.Test.Contains("canci9"))
            refElecciones.g_idChancellor = 9;
          refElecciones.g_isChancellor = true;
        }
        else if (msg.Test.Contains("elec2"))
        {
          refElecciones.g_phase = 2;
          refElecciones.g_isPhase = false;
          refElecciones.waitingNextTurn = true;
          refElecciones.hidePlayers();
          wait = true;
        }
      }
      else if (msg.Test.Contains("ronda1"))
      {

      }
      else if (msg.Test.Contains("ronda2"))
      {
        if (msg.Test.Contains("continue"))
        {
          wait = false;
        }
        refRonda.phase = 2;
        if (msg.Test.Contains("leg0"))
        {


          refSesionLegislativa.m_phase = 0;
          refElecciones.g_phase = 0;
          refElecciones.g_isPhase = false;
          refElecciones.waitingNextTurn = false;
          refElecciones.g_isChancellor = false;
        }
        if (msg.Test.Contains("leg1"))
        {
          refSesionLegislativa.hideTakeCard();
          refSesionLegislativa.m_phase = 1;
          if (msg.Test.Contains("car00"))
          {
            refSesionLegislativa.card0 = 0;
          }
          else
          {
            refSesionLegislativa.card0 = 1;
          }
          if (msg.Test.Contains("car10"))
          {
            refSesionLegislativa.card1 = 0;
          }
          else
          {
            refSesionLegislativa.card1 = 1;
          }
          if (msg.Test.Contains("car20"))
          {
            refSesionLegislativa.card2 = 0;
          }
          else
          {
            refSesionLegislativa.card2 = 1;
          }
        }
        if (msg.Test.Contains("leg2"))
        {
          refSesionLegislativa.m_phase = 2;
          refSesionLegislativa.hidePresidentSelection();
          if (msg.Test.Contains("car00"))
          {
            refSesionLegislativa.card0 = 0;
          }
          else
          {
            refSesionLegislativa.card0 = 1;
          }
          if (msg.Test.Contains("car10"))
          {
            refSesionLegislativa.card1 = 0;
          }
          else
          {
            refSesionLegislativa.card1 = 1;
          }
          //refSesionLegislativa.PresidentEndSelection();
        }
        if (msg.Test.Contains("leg3"))
        {
          refSesionLegislativa.m_phase = 3;
          refSesionLegislativa.CansillerEndSelection();
          refSesionLegislativa.m_phase = 3;
          refSesionLegislativa.faseAlready = false;
          refSesionLegislativa.waitingNextTurn = true;
          refSesionLegislativa.hideCansillerselection();
          refElecciones.g_isChancellor = false;
          refElecciones.m_alreadyVote = false;

          wait = true;
        }

      }
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "Okey";
      SendeServer(nt);
    }
  }
  #endregion
  public void FormatTestData()
  {
    Net_MessageTest nt = new Net_MessageTest();
    nt.Test = "Hola soy el cliente";
    Debug.Log(connectionid);
    SendeServer(nt);


  }

  public void preGameUpdate()
  {
    if (m_myApodo.text.Length<3)
    {
      connectButtom.GetComponent<Button>().interactable = false;
    }
    else
    {
      connectButtom.GetComponent<Button>().interactable = true;
    }
    if (connected)
    {
      UpdateMEssagePump(); 
    }
  }

  public void onStartGame()
  {
    refSesionLegislativa = FindObjectOfType<sesionLegislativa>();
    refSesionEjecutiva = FindObjectOfType<SessionEjecutiva>();
    refElecciones = FindObjectOfType<Elecciones>();
    refRonda = FindObjectOfType<Ronda>();
    UpdateMEssagePump();
  }

  public void gameUpdate()
  {
    UpdateMEssagePump();
  }
}

