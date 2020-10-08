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
  public gameManager refGManaer;
  public bool bContinueGame = false;
  public bool bInGameScene = false;
  public bool wait = false;
  public bool connected = false;
  public bool reconnecting = false;

  public int m_numPlayers;

  public InputField m_ipInput;
  public InputField m_myApodo;
  bool DoOnce = false;
  bool debug = false;
  public GameObject connectButtom;
  public GameObject ReadyButtom;
  public bool allPlayersRegister;

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

  public void loadApodo()
  {
    string savedapodo = "";

    if (saveName.loadName(ref savedapodo))
    {
      m_myApodo.text = savedapodo;
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
      connectButtom.GetComponent<Button>().interactable = false;
      ReadyButtom.SetActive(true);
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
        if (bInGameScene)
        {
          connected = false;
          //reconnecting = true;
        }
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
      if (msg.Test.Contains("redata"))
      {
        reconnectData(ref msg);
      }
      if (msg.Test.Contains("doVeto"))
      {
        refSesionLegislativa.precidentDecideForVeto();
      }
      if (msg.Test.Contains("cancelVeto"))
      {
        refSesionLegislativa.cancelVeto();
      }
      if (msg.Test.Contains("realizeVeto"))
      {
        refSesionLegislativa.veto();
      }
      if (msg.Test.Contains("liberalwon") || msg.Test.Contains("fascistwon"))
      {
        if (msg.Test.Contains("liberalwon"))
        {
          refGManaer.liberalWon = true;
        }
        else
        {
          refGManaer.fascistWon = true;
        }
        refGManaer.gameFinished = true;
      }
      if (msg.Test.Contains("rol"))
      {
        string[] chek = msg.Test.Split('_');
        int rol;
        int.TryParse(chek[1], out rol);
        if (msg.Test.Contains("rol0"))
        {
          refGManaer.g_Players[0].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol1"))
        {
          refGManaer.g_Players[1].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol2"))
        {
          refGManaer.g_Players[2].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol3"))
        {
          refGManaer.g_Players[3].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol4"))
        {
          refGManaer.g_Players[4].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol5"))
        {
          refGManaer.g_Players[5].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol6"))
        {
          refGManaer.g_Players[6].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol7"))
        {
          refGManaer.g_Players[7].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol8"))
        {
          refGManaer.g_Players[8].GetComponent<Jugador>().Rol = rol;
        }
        else if (msg.Test.Contains("rol9"))
        {
          refGManaer.g_Players[9].GetComponent<Jugador>().Rol = rol;
        }
      }
      if (msg.Test.Contains("ap"))
      {
        string[] chek = msg.Test.Split('_');
        int afil;
        int.TryParse(chek[1], out afil);
        if (msg.Test.Contains("ap0"))
        {
          refGManaer.g_Players[0].GetComponent<Jugador>().idAfiliation = afil;
          if (1==m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap1"))
        {
          refGManaer.g_Players[1].GetComponent<Jugador>().idAfiliation = afil;
          if (2 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap2"))
        {
          refGManaer.g_Players[2].GetComponent<Jugador>().idAfiliation = afil;
          if (3 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap3"))
        {
          refGManaer.g_Players[3].GetComponent<Jugador>().idAfiliation = afil;
          if (4 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap4"))
        {
          refGManaer.g_Players[4].GetComponent<Jugador>().idAfiliation = afil;
          if (5 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap5"))
        {
          refGManaer.g_Players[5].GetComponent<Jugador>().idAfiliation = afil;
          if (6 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap6"))
        {
          refGManaer.g_Players[6].GetComponent<Jugador>().idAfiliation = afil;
          if (7 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap7"))
        {
          refGManaer.g_Players[7].GetComponent<Jugador>().idAfiliation = afil;
          if (8 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap8"))
        {
          refGManaer.g_Players[8].GetComponent<Jugador>().idAfiliation = afil;
          if (9 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
        else if (msg.Test.Contains("ap9"))
        {
          refGManaer.g_Players[9].GetComponent<Jugador>().idAfiliation = afil;
          if (10 == m_numPlayers)
          {
            allPlayersRegister = true;
            FindObjectOfType<SetApodoRo>().onInit();
          }
        }
      }
      if (msg.Test.Contains("ronda0"))
      {
        refRonda.phase = 0;
        if (msg.Test.Contains("topCard"))
        {
          if (msg.Test.Contains("topCard1"))
          {
            refSesionLegislativa.baraja.addTopCardClient(1);
          }
          else
          {
            refSesionLegislativa.baraja.addTopCardClient(0);
          }
        }
        if (msg.Test.Contains("elec0"))
        {
          if (msg.Test.Contains("continue"))
          {
            wait = false;
          }
          refElecciones.g_phase = 0;
          refElecciones.g_isChancellor = false;
          refElecciones.g_isPhase = false;
          refElecciones.waitingNextTurn = false;
          refSesionLegislativa.alredySelectedCard = false;
          refSesionLegislativa.faseAlready = false;
          refSesionEjecutiva.hideAll();
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

          if (msg.Test.Contains("oldpre-1"))
            refElecciones.g_idOldPresident = -1;
          if (msg.Test.Contains("oldpre0"))
            refElecciones.g_idOldPresident = 0;
          if (msg.Test.Contains("oldpre1"))
            refElecciones.g_idOldPresident = 1;
          if (msg.Test.Contains("oldpre2"))
            refElecciones.g_idOldPresident = 2;
          if (msg.Test.Contains("oldpre3"))
            refElecciones.g_idOldPresident = 3;
          if (msg.Test.Contains("oldpre4"))
            refElecciones.g_idOldPresident = 4;
          if (msg.Test.Contains("oldpre5"))
            refElecciones.g_idOldPresident = 5;
          if (msg.Test.Contains("oldpre6"))
            refElecciones.g_idOldPresident = 6;
          if (msg.Test.Contains("oldpre7"))
            refElecciones.g_idOldPresident = 7;
          if (msg.Test.Contains("oldpre8"))
            refElecciones.g_idOldPresident = 8;
          if (msg.Test.Contains("oldpre9"))
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
          if (msg.Test.Contains("continue"))
          {
            wait = false;
          }
          refElecciones.hidePlayers();
          refElecciones.g_phase = 1;
          refElecciones.g_isPhase = false;
          refElecciones.g_isChancellor = true;
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
          if (msg.Test.Contains("fascista"))
          {
            refSesionLegislativa.baraja.clientUpdateTab(1);
          }
          else
          {
            refSesionLegislativa.baraja.clientUpdateTab(0);
          }
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
      else if (msg.Test.Contains("ronda3"))
      {
        if (msg.Test.Contains("continue"))
        {
          wait = false;
        }
        refRonda.phase = 3;
        if (msg.Test.Contains("eje0 "))
        {
          refSesionEjecutiva.bAlredyInitPhase = false;
          refSesionEjecutiva.phase = 0;
          refSesionEjecutiva.bhideButtons = false;
          refSesionEjecutiva.waitingNextTurn = false;
          refSesionEjecutiva.bhideButtonAndText = false;
          refSesionEjecutiva.bhideButtons = false;
          refSesionEjecutiva.bAlredyDrawMembership = false;
          refSesionEjecutiva.bSeeingMembership = false;
          refSesionEjecutiva.alredydoit = false;

        }
        if (msg.Test.Contains("eje1 "))
        {
          //refSesionEjecutiva.phase = 1;
          if (msg.Test.Contains("car00"))
          {
            refSesionEjecutiva.card0 = 0;
          }
          else
          {
            refSesionEjecutiva.card0 = 1;
          }
          if (msg.Test.Contains("car10"))
          {
            refSesionEjecutiva.card1 = 0;
          }
          else
          {
            refSesionEjecutiva.card1 = 1;
          }
          if (msg.Test.Contains("car20"))
          {
            refSesionEjecutiva.card2 = 0;
          }
          else
          {
            refSesionEjecutiva.card2 = 1;
          }
        }
        if (msg.Test.Contains("eje2 "))
        {
          if (msg.Test.Contains("ejecuted"))
          {
            if (msg.Test.Contains("ejecuted0"))
              refGManaer.g_Players[0].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted1"))
              refGManaer.g_Players[1].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted2"))
              refGManaer.g_Players[2].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted3"))
              refGManaer.g_Players[3].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted4"))
              refGManaer.g_Players[4].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted5"))
              refGManaer.g_Players[5].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted6"))
              refGManaer.g_Players[6].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted7"))
              refGManaer.g_Players[7].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted8"))
              refGManaer.g_Players[8].GetComponent<Jugador>().bIsDead = true;
            if (msg.Test.Contains("ejecuted9"))
              refGManaer.g_Players[9].GetComponent<Jugador>().bIsDead = true;
          }
          if (msg.Test.Contains("special"))
          {
            if (msg.Test.Contains("special-1"))
              refElecciones.g_idPresident = -1;
            if (msg.Test.Contains("special0"))
              refElecciones.g_idPresident = 0;
            if (msg.Test.Contains("special1"))
              refElecciones.g_idPresident = 1;
            if (msg.Test.Contains("special2"))
              refElecciones.g_idPresident = 2;
            if (msg.Test.Contains("special3"))
              refElecciones.g_idPresident = 3;
            if (msg.Test.Contains("special4"))
              refElecciones.g_idPresident = 4;
            if (msg.Test.Contains("special5"))
              refElecciones.g_idPresident = 5;
            if (msg.Test.Contains("special6"))
              refElecciones.g_idPresident = 6;
            if (msg.Test.Contains("special7"))
              refElecciones.g_idPresident = 7;
            if (msg.Test.Contains("special8"))
              refElecciones.g_idPresident = 8;
            if (msg.Test.Contains("special9"))
              refElecciones.g_idPresident = 9;
          }
          refSesionEjecutiva.phase = 2;
          refSesionLegislativa.baraja.canEjecutive = false;
        }
      }
      if (allPlayersRegister)
      {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "Okey";
        SendeServer(nt);
      }
    }
  }

  void reconnectData(ref Net_MessageTest msg)
  {
    if (msg.Test.Contains("player0dead"))
      refGManaer.g_Players[0].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player1dead"))
      refGManaer.g_Players[1].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player2dead"))
      refGManaer.g_Players[2].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player3dead"))
      refGManaer.g_Players[3].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player4dead"))
      refGManaer.g_Players[4].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player5dead"))
      refGManaer.g_Players[5].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player6dead"))
      refGManaer.g_Players[6].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player7dead"))
      refGManaer.g_Players[7].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player8dead"))
      refGManaer.g_Players[8].GetComponent<Jugador>().bIsDead = true;
    if (msg.Test.Contains("player9dead"))
      refGManaer.g_Players[9].GetComponent<Jugador>().bIsDead = true;

    if (msg.Test.Contains("fascistas1"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(1);
    if (msg.Test.Contains("fascistas2"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(2);
    if (msg.Test.Contains("fascistas3"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(3);
    if (msg.Test.Contains("fascistas4"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(4);
    if (msg.Test.Contains("fascistas5"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(5);
    if (msg.Test.Contains("fascistas6"))
      refSesionLegislativa.baraja.setNumFascistaPuestas(6);

    if (msg.Test.Contains("liberales1"))
      refSesionLegislativa.baraja.setNumLiberalPuestas(1);
    if (msg.Test.Contains("liberales2"))
      refSesionLegislativa.baraja.setNumLiberalPuestas(2);
    if (msg.Test.Contains("liberales3"))
      refSesionLegislativa.baraja.setNumLiberalPuestas(3);
    if (msg.Test.Contains("liberales4"))
      refSesionLegislativa.baraja.setNumLiberalPuestas(4);
    if (msg.Test.Contains("liberales5"))
      refSesionLegislativa.baraja.setNumLiberalPuestas(5);

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
    string name = m_myApodo.text;
    saveName.savename(ref name);

    if (m_myApodo.text.Length<3 || connected)
    {
      connectButtom.GetComponent<Button>().interactable = false;
    }
    else
    {
      connectButtom.GetComponent<Button>().interactable = true;
    }
    if(FindObjectOfType<Ready>().ready == 1)
    {
      ReadyButtom.GetComponent<Button>().interactable = false;
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
    refGManaer = FindObjectOfType<gameManager>();
    //UpdateMEssagePump();
  }

  public void gameUpdate()
  {
    if (reconnecting)
    {
      reconnect();
    }
    else
    { 
      UpdateMEssagePump();
    }
  }

  public void reconnect()
  {
    if (tryToConnect())
    {
      reconnecting = false;
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "reconnect";
      Debug.Log(connectionid);
      SendeServer(nt);
    }
  }

  bool tryToConnect()
  {
    if (connected)
    {
      return true;
    }
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
      Debug.Log("couldn't connectButtom");
      return false;
    }
    else
    {
      Debug.Log("connected");
      connected = true;
      return true;
    }
    Debug.Log(string.Format("Puerto{0}", ServerIP));
    return false;
  }


  public void disconectedTest()
  {
    NetworkManager.singleton.client.Disconnect();
    connected = false;
  }

}

