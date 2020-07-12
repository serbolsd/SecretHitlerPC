using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
  public List<int> conections = new List<int>();
  //public List<int> playerRedy = new List<int>();
  public Dictionary<int, int> playerRedyDic = new Dictionary<int, int>();
  public Dictionary<int, int> playerId = new Dictionary<int, int>();
  public Dictionary<int, int> playerVoteDic = new Dictionary<int, int>();
  public Dictionary<int, string> playerApode = new Dictionary<int, string>();

  public InputField m_ip;
  public InputField m_myApodo;

  public sesionLegislativa refSesionLegislativa;
  public SessionEjecutiva refSesionEjecutiva;
  public Elecciones refElecciones;
  public Ronda refRonda;
  public bool bContinueGame = false;
  public bool bWaitingGame = false;
  bool bInGameScene = false;
  public bool bAllVoted = false;

  bool canInitGame = true;
  public GameObject btnAllread;

  int rondaFase = 0;
  int eleccionesFase = 0;
  int legislativaFase = 0;
  #region Monobehaviour
  #endregion
  public void Init()
  {
    DontDestroyOnLoad(gameObject);
    NetworkTransport.Init();
    ConnectionConfig cc = new ConnectionConfig();
    ReliableChannel = cc.AddChannel(QosType.Reliable);
    HostTopology top = new HostTopology(cc, Users);
    HostId = NetworkTransport.AddHost(top, Port, null);
    conections.Add(HostId);
    playerRedyDic.Add(0, 0);
    playerId.Add(0, 0);
    playerVoteDic.Add(0, 0);
    WebHostID = NetworkTransport.AddWebsocketHost(top, Web, null);
    Debug.Log(string.Format("Puerto{0} y webport{1}", Web, Port));
    severStarted = true;
    string ipv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
    //string ipv6 = IPManager.GetIP(ADDRESSFAM.IPv6);
    m_ip.text = ipv4;

  }
  public void Shutdown()
  {
    severStarted = false;
    NetworkTransport.Shutdown();

  }

  int setIdToPlayer()
  {
    int id = 0;
    for (int i = 0; i < conections.Count; i++)
    {
      if (playerId[conections[i]] != id)
      {
        return id;
      }
      id++;

    }
    return id;
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
        Debug.Log(string.Format("User has connected" + connectionId));
        //playerRedy.Add(0);
        int id = setIdToPlayer();
        playerId.Add(connectionId, id);
        conections.Add(connectionId);
        playerRedyDic.Add(id, 0);
        playerVoteDic.Add(id, 0);
        playerApode.Add(id,"");
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "yourid_";
        nt.Test += id;
        SendClient(connectionId, nt);
        Debug.Log(playerRedyDic.Count);
        break;
      case NetworkEventType.DisconnectEvent:
        Debug.Log(string.Format("User has dissconected" + connectionId));
        conections.Remove(connectionId);
        int idToRemove = playerId[connectionId];
        playerId.Remove(connectionId);
        playerRedyDic.Remove(idToRemove);
        playerVoteDic.Remove(idToRemove);
        playerApode.Remove(idToRemove);
        Debug.Log(playerRedyDic.Count);
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
    Debug.Log(connectionId + ": " + msg.Test);
    if (!bInGameScene)
    {
      if (msg.Test.Contains("ready:"))
      {
        if (msg.Test.Contains("1"))
        {
          playerRedyDic[playerId[connectionId]] = 1;
        }
        else
        {
          playerRedyDic[playerId[connectionId]] = 0;
        }
      }
      if (msg.Test.Contains("Okey"))
      {
        playerRedyDic[playerId[connectionId]] = 1;
      }
      if (msg.Test.Contains("apodo"))
      {
        string[] chek = msg.Test.Split('_');
        playerApode[playerId[connectionId]] = chek[1];
      }
    }
    else
    {
      if (msg.Test.Contains("Okey"))
      {
        playerRedyDic[playerId[connectionId]] = 1;
      }

      if (0 == rondaFase) //elecciones
      {
        if (msg.Test.Contains("canciller:"))
        {
          if (msg.Test.Contains("canciller:0"))
            refElecciones.g_idChancellor = 0;
          if (msg.Test.Contains("canciller:1"))
            refElecciones.g_idChancellor = 1;
          if (msg.Test.Contains("canciller:2"))
            refElecciones.g_idChancellor = 2;
          if (msg.Test.Contains("canciller:3"))
            refElecciones.g_idChancellor = 3;
          if (msg.Test.Contains("canciller:4"))
            refElecciones.g_idChancellor = 4;
          if (msg.Test.Contains("canciller:5"))
            refElecciones.g_idChancellor = 5;
          if (msg.Test.Contains("canciller:6"))
            refElecciones.g_idChancellor = 6;
          if (msg.Test.Contains("canciller:7"))
            refElecciones.g_idChancellor = 7;
          if (msg.Test.Contains("canciller:8"))
            refElecciones.g_idChancellor = 8;
          if (msg.Test.Contains("canciller:9"))
            refElecciones.g_idChancellor = 9;
          refElecciones.g_isChancellor = true;
        }
        if (msg.Test.Contains("vote:"))
        {
          if (msg.Test.Contains("Yes"))
          {
            playerVoteDic[playerId[connectionId]] = 2;
          }
          else
          {
            playerVoteDic[playerId[connectionId]] = 1;
          }
        }
      }
      else if (1 == rondaFase)//ejecutiva
      {

      }
      else if (2 == rondaFase)//legislativa
      {
        if (msg.Test.Contains("takeCards"))
        {
          refSesionLegislativa.takeCards();
        }
        if (1 == refSesionLegislativa.m_phase)
        {
          if (msg.Test.Contains("car0"))
          {
            refSesionLegislativa.selectCard1();
          }
          if (msg.Test.Contains("car1"))
          {
            refSesionLegislativa.selectCard2();
          }
          if (msg.Test.Contains("car2"))
          {
            refSesionLegislativa.selectCard3();
          }
        }
        else if (2 == refSesionLegislativa.m_phase)
        {
          if (msg.Test.Contains("car0"))
          {
            refSesionLegislativa.selectCard1();
          }
          if (msg.Test.Contains("car1"))
          {
            refSesionLegislativa.selectCard2();
          }
        }
      }
      if (msg.Test.Length < 2)
      {
        bWaitingGame = true;
      }

    }

  }
  public void SendClient(int connwctioID, NetMeg msg)
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
    nt.Test += 1;
    for (int i = 0; i < conections.Count; i++)
    {
      if (i != HostId)
      {
        Debug.Log(conections[i]);
        SendClient(conections[i], nt);
      }
    }
  }

  public void goToGame()
  {
    bInGameScene = true;
    for (int i = 0; i < playerApode.Count; i++)
    {
      Net_MessageTest apode = new Net_MessageTest();
      apode.Test = "apodo";
      apode.Test += i;
      apode.Test += "_";
      apode.Test += playerApode[i];
      for (int j = 0; j < conections.Count; j++)
      {
        if (i != HostId)
        {
          Debug.Log(conections[j]);
          SendClient(conections[j], apode);
        }
      }
    }
    

    Net_MessageTest nt = new Net_MessageTest();
    nt.Test = "initGame: ";
    nt.Test += conections.Count;
    nt.Test += "play";
    for (int i = 0; i < conections.Count; i++)
    {
      if (i != HostId)
      {
        Debug.Log(conections[i]);
        SendClient(conections[i], nt);
      }
    }

    bContinueGame = false;
    for (int i = 0; i < playerRedyDic.Count; i++)
    {
      playerRedyDic[i] = 0;
    }
    playerRedyDic[0] = 1;
    SceneManager.LoadScene("GameServer");
  }

  public void preGameUpdate()
  {
    UpdateMEssagePump();
    canInitGame = true;
    if (m_myApodo.text.Length<3)
    {
      GetComponent<Ready>().readybutton.GetComponent<Button>().interactable = false;
    }
    else
    {
      playerApode[0] = m_myApodo.text;
      GetComponent<Ready>().readybutton.GetComponent<Button>().interactable = true;
    }
    for (int i = 0; i < playerRedyDic.Count; i++)
    {
      if (playerRedyDic[conections[i]] != 1)
      {
        canInitGame = false;
        break;
      }
    }
    if (!bInGameScene)
    {
      if (canInitGame)
      {
        btnAllread.SetActive(true);
      }
      else
        btnAllread.SetActive(false);
    }
  }

  public void onStartGame()
  {
    refSesionLegislativa = FindObjectOfType<sesionLegislativa>();
    refSesionEjecutiva = FindObjectOfType<SessionEjecutiva>();
    refElecciones = FindObjectOfType<Elecciones>();
    refRonda = FindObjectOfType<Ronda>();
    for (int i = 0; i < playerRedyDic.Count; i++)
    {
      playerRedyDic[i] = 0;
    }
    //playerVoteDic = playerRedyDic;
    //ENVIAR REGLAS
    //ENVIAR NUMERO DE JUGADORES Y SUS DATOS
    //ENVIAR ID PRESIDENTE
  }

  void checkForContinueGame()
  {
    bContinueGame = true;
    for (int i = 0; i < playerRedyDic.Count; i++)
    {
      if (0 == playerRedyDic[i])
      {
        bContinueGame = false;
        return;
      }
    }
  }

  bool checkCambioRonda()
  {
    if (rondaFase != refRonda.phase)
    {
      rondaFase = refRonda.phase;
      if (0 == rondaFase) //elecciones
      {

      }
      else if (1 == rondaFase)//ejecutiva
      {

      }
      else if (2 == rondaFase)//legislativa
      {

      }

    }
    return false;
  }

  public void gameUpdate()
  {
    rondaFase = refRonda.phase;
    UpdateMEssagePump();
    if (!bContinueGame)
    {
      for (int i = 0; i < playerRedyDic.Count; i++)
      {
        if (0 == playerRedyDic[conections[i]])
        {
          return;
        }
      }
      bContinueGame = true;
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "continue ";
      Debug.Log(nt.Test);

    }
    if (1 == refElecciones.g_phase)
    {
      bAllVoted = true;
      for (int i = 0; i < playerVoteDic.Count; i++)
      {
        if (0 == playerVoteDic[conections[i]])
        {
          bAllVoted = false;
        }
      }
    }
    //checkForContinueGame(); 
  }

  public void sentInfoGame()
  {
    Net_MessageTest nt = new Net_MessageTest();
    if (bContinueGame)
    {
      //agregar datos importantes
      //rondaFase = refRonda.phase;
      if (0 == rondaFase) //elecciones
      {
        nt.Test = "ronda0 :";
        //refRonda.phase = 0;
        if (0 == refElecciones.g_phase)
        {
          for (int i = 0; i < playerVoteDic.Count; i++)
          {
            playerVoteDic[playerId[conections[i]]] = 0;
          }
          bAllVoted = false;
          refSesionLegislativa.alredySelectedCard = false;
          refSesionLegislativa.faseAlready = false;
          refSesionLegislativa.m_phase = 0;
          nt.Test += "continue ";
          nt.Test += " elec0";
          nt.Test += " :";
          nt.Test += " presi";
          nt.Test += refElecciones.g_idPresident;
          nt.Test += " : ";
          nt.Test += "oldpresi";
          nt.Test += refElecciones.g_idOldPresident;
          nt.Test += " : ";
          nt.Test += "oldcanci";
          nt.Test += refElecciones.g_idOldChancellor;
        }
        if (1 == refElecciones.g_phase)
        {
          nt.Test += " elec1";
          nt.Test += " : ";
          nt.Test += "canci";
          nt.Test += refElecciones.g_idChancellor;
        }
        if (2 == refElecciones.g_phase)
        {
          nt.Test += " elec2";
        }

        for (int i = 0; i < conections.Count; i++)
        {
          if (i != HostId)
          {
            if (1 == refElecciones.g_phase && 0 != playerVoteDic[conections[i]])
            {
              Net_MessageTest nwt = new Net_MessageTest();
              nwt.Test = "alredy vote";
              Debug.Log(conections[i]);
              SendClient(conections[i], nwt);
            }
            else
            {
              Debug.Log(conections[i]);
              SendClient(conections[i], nt);
            }

          }
        }
      }
      else if (1 == rondaFase)//ejecutiva
      {
        nt.Test = "ronda1 ";


      }
      else if (2 == rondaFase)//legislativa
      {
        nt.Test = "ronda2 ";
        if (0 == refSesionLegislativa.m_phase)
        {
          nt.Test += "leg0 ";
          nt.Test += "continue ";
        }
        else if (1 == refSesionLegislativa.m_phase)
        {
          nt.Test += "leg1 ";
          nt.Test += "car0";
          nt.Test += refSesionLegislativa.baraja.enMano[0].GetComponent<CartaDePoliza>().tipoDeCarta;
          nt.Test += "car1";
          nt.Test += refSesionLegislativa.baraja.enMano[1].GetComponent<CartaDePoliza>().tipoDeCarta;
          nt.Test += "car2";
          nt.Test += refSesionLegislativa.baraja.enMano[2].GetComponent<CartaDePoliza>().tipoDeCarta;

        }
        else if (2 == refSesionLegislativa.m_phase)
        {
          nt.Test += "leg2 ";
          nt.Test += "car0";
          nt.Test += refSesionLegislativa.baraja.enMano[0].GetComponent<CartaDePoliza>().tipoDeCarta;
          nt.Test += "car1";
          nt.Test += refSesionLegislativa.baraja.enMano[1].GetComponent<CartaDePoliza>().tipoDeCarta;
        }
        else if (3 == refSesionLegislativa.m_phase)
        {
          nt.Test += "leg3 ";
          for (int i = 0; i < playerVoteDic.Count; i++)
          {
            playerVoteDic[playerId[conections[i]]] = 0;
          }
          bAllVoted = false;
          refElecciones.g_isChancellor = false;
          refElecciones.g_isPhase = false;
          //refElecciones.BtnNextTurn();

        }
        for (int i = 0; i < conections.Count; i++)
        {
          if (i != HostId)
          {
            Debug.Log(conections[i]);
            SendClient(conections[i], nt);
          }
        }
      }
      bContinueGame = false;
      for (int i = 0; i < playerRedyDic.Count; i++)
      {
        playerRedyDic[conections[i]] = 0;
      }
      playerRedyDic[0] = 1;
    }
    Debug.Log(nt.Test);
    bWaitingGame = false;
  }
}


