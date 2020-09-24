using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elecciones : MonoBehaviour
{
  public bool g_isPresident = true;   //Si el presidente ya fue elegido
  bool g_firstTurn = true;
  public bool g_isChancellor = false;
  public bool g_isPhase = false;

  public bool specialSelection = false;

  public int g_usuarios = 5;
  public int g_idPresident = 1;
  public int g_savedidPresident = 1;
  public int g_specialIDPresident = 1;
  public int g_idOldPresident = -1;
  public int g_savedidOldPresident = -1;
  public int g_idChancellor = 0;
  public int g_savedidChancellor = 0;
  public int g_idOldChancellor = -1;
  public int g_savedidOldChancellor = -1;
  public int g_phase = 0; //0 = Elección, 1 = Votación, 2 = Espera sig turno
  int g_turnCount = 0;
  int g_noCount = 0;
  int g_iteratorPlayers = 0;

  public int typeTopCard;

  public int[] g_listPlayerVote; //Lista donde se van a agregar los si o no

  public bool waitingNextTurn = false;
  public bool m_alreadyVote = false;

  public GameObject g_PrefabPlayer;
  public GameObject[] g_Players;
  public GameObject[] g_BtnPlayers = new GameObject[10];

  public GameObject g_currentPlayerTxt;
  public GameObject g_BtnYes;
  public GameObject g_BtnNo;
  public GameObject g_BtnNextTurn;

  gameManager refGameMan;
  sesionLegislativa refLeg;

  public bool putTheTopCard = false;
  //Llamamos esta función cuando queramos y siempre será para el inicio
  //Reiniciar o pruebas
  public void OnStart()
  {
    refGameMan = FindObjectOfType<gameManager>();
    refLeg = FindObjectOfType<sesionLegislativa>();
    //Crear y setearle un ID  a los jugadores
    g_usuarios = FindObjectOfType<Seleccion_Roll>().numPlayers;
    g_idPresident = 0;
    //

  }

  public void onUpdate()
  {
    switch (g_phase)
    {

      case 0:
        if (waitingNextTurn)
        {
          return;
        }
        m_alreadyVote = false;
        PhaseElection();
        break;

      case 1:
        PhaseVotation();
        break;

      case 2:
        //WaitingNextTurn();
        waitingNextTurn = true;
        break;

      default:
        break;
    }
  }
  //Función de espera por turnos
  void WaitingNextTurn()
  {
    if (!g_isPhase)
    {
      g_isPhase = true;

      g_BtnNextTurn.SetActive(true);
    }

  }

  //Fase de votaciones
  void PhaseVotation()
  {
    if (!g_isPhase && !m_alreadyVote)
    {
      if (refGameMan.bServer)
      {
        refGameMan.refServer.bWaitingGame = true;
      }
      g_isPhase = true;

      //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
      g_currentPlayerTxt.SetActive(true);
      g_BtnYes.SetActive(true);
      g_BtnNo.SetActive(true);

      //g_currentPlayerTxt.GetComponent<Text>().text = "vota por ";
      g_currentPlayerTxt.GetComponentInChildren<Text>().text = Traslate.getTxtVoteFor();
      g_currentPlayerTxt.GetComponentInChildren<Text>().text += g_Players[g_idChancellor].GetComponent<Jugador>().Apodo;

      //g_listPlayerVote = new int[g_usuarios];

      return;
    }
    //Esto se cambiara a cada pantalla
    //g_currentPlayerTxt.GetComponent<Text>().text = "vota por ";
    g_currentPlayerTxt.GetComponentInChildren<Text>().text = Traslate.getTxtVoteFor();
    g_currentPlayerTxt.GetComponentInChildren<Text>().text +=g_Players[g_idChancellor].GetComponent<Jugador>().Apodo;
    /*
    if (g_iteratorPlayers >= g_usuarios)
    {
        CountingVotes();
    }*/
    if (refGameMan.bServer)
    {
      if (refGameMan.refServer.bAllVoted)
      {
        CountingVotes();
      }
    }
  }

  //Función para poder contar los votos
  void CountingVotes()
  {
    int yes = 0;
    int no = 0;

    //for (int i = 0; i < g_usuarios; i++)
    for (int i = 0; i < refGameMan.refServer.playerVoteDic.Count; i++)
    {
      //if (g_listPlayerVote[i] == 0)
      if (refGameMan.refServer.playerVoteDic[i] == 1)
      {
        no++;
      }
      else if (refGameMan.refServer.playerVoteDic[i] == 2)
      {
        yes++;
      }
    }

    //Checamos para ver si el presidente debe cambiar por votación
    if (no > yes)
    {
      NextPresident();
    }
    else
    {
      //if (true && refGameMan.g_Players[g_idChancellor].GetComponent<Jugador>().Rol == 2)
      if (refGameMan.canWinFascist && refGameMan.g_Players[g_idChancellor].GetComponent<Jugador>().Rol == 2)
      {
        refGameMan.fascistWon = true;
      }
      g_phase++;
      g_idOldPresident = g_idPresident;
      g_idOldChancellor = g_idChancellor;
      //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
      g_currentPlayerTxt.SetActive(false);
      g_BtnYes.SetActive(false);
      g_BtnNo.SetActive(false);

      g_iteratorPlayers = 0;

      g_noCount = 0;

      g_currentPlayerTxt.GetComponentInChildren<Text>().text = g_iteratorPlayers.ToString();

      g_isPhase = false;
    }
  }

  //Función para la selección de otro presidente
  void NextPresident()
  {
    g_phase = 0;
    g_isChancellor = false;
    g_isPhase = false;
    if (specialSelection)
    {
      g_idPresident = g_savedidPresident;
      g_idOldPresident = g_savedidOldPresident;
      g_savedidPresident = -1;
      g_savedidOldPresident = -1;
      specialSelection = false;
    }
    g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 0;
    g_idPresident++;

    while (true)
    {
      if (g_idPresident != g_idOldPresident && g_idPresident < g_usuarios && !g_Players[g_idPresident].GetComponent<Jugador>().bIsDead)
        break;
      g_idPresident++;
      if (g_idPresident >= g_usuarios)
        g_idPresident = 0;
    }
    //if (g_idPresident == g_idOldPresident)
    //{
    //  g_idPresident++;
    //}
    //
    //if (g_idPresident >= g_usuarios)
    //{
    //  g_idPresident = 0;
    //}

    g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 1;

    g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 0;

    //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
    g_currentPlayerTxt.SetActive(false);
    g_BtnYes.SetActive(false);
    g_BtnNo.SetActive(false);

    g_iteratorPlayers = 0;

    //g_currentPlayerTxt.GetComponent<Text>().text = g_iteratorPlayers.ToString();

    g_noCount++;

    refGameMan.refServer.bWaitingGame = true;
    if (g_noCount > 3)
    {
      g_noCount = 0;
      refLeg.addTopCard(ref typeTopCard);
      putTheTopCard = true;
      //agregar la carta de arriba
    }
  }

  //Fase de las elecciones
  void PhaseElection()
  {
    //Si el presidente ya fue elegido
    if (g_isPresident)
    {
      SelectingChancellor();
      return;
    }

    if (!refGameMan.bServer)
    {
      return;
    }
    //En caso de iniciar la nueva ronda del juego
    if (g_firstTurn)
    {
      g_firstTurn = false;

      //Tenemos que mostrar todos los posibles jugadores para ser presidente
      int numRandom = 0;
      numRandom = Random.Range(0, g_usuarios - 1);

      //Se define aleatoriamente al presidente
      g_Players[1].GetComponent<Jugador>().Cargo = 1;
      g_isPresident = true;
    }
  }

  //Función para la selección del cansiller
  void SelectingChancellor()
  {
    bool draw = true;
    if (refGameMan.bServer && 0 != g_idPresident)
    {
      draw = false;
      g_isPhase = true;
    }
    else if (!refGameMan.bServer && refGameMan.idConection != g_idPresident)
    {
      draw = false;
      g_isPhase = true;
    }

    if (draw)
    {
      if (!g_isPhase)
      {
        //Tiene que mostrar todos los posibles jugadores a canciller
        for (int i = 0; i < g_usuarios; i++)
        {
          //Solo es color del boton
          if (i < g_usuarios && i != g_idPresident && i != g_idOldPresident && i != g_idOldChancellor&&! g_Players[i].GetComponent<Jugador>().bIsDead)
          {
            g_BtnPlayers[i].SetActive(true);
            g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = g_Players[i].GetComponent<Jugador>().Apodo;

            ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

            ColorButton.highlightedColor = Color.blue;
            ColorButton.normalColor = Color.white;
            ColorButton.pressedColor = Color.gray;

            g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
          }
          else
          {
            ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

            ColorButton.highlightedColor = Color.blue;
            ColorButton.normalColor = Color.black;
            ColorButton.pressedColor = Color.gray;

            g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
          }
        }

        g_isPhase = true;
        return;
      }
    }


    //El presidente selecciona el canciller
    if (g_isChancellor)
    {
      g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 2;

      for (int i = 0; i < 10; i++)
      {
        //Solo es color del boton
        if (i < g_usuarios)
        {
          g_BtnPlayers[i].SetActive(false);
          g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = g_Players[i].GetComponent<Jugador>().Apodo;

          ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

          ColorButton.highlightedColor = Color.blue;
          ColorButton.normalColor = Color.white;
          ColorButton.pressedColor = Color.gray;

          g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
        }
        else
        {
          ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

          ColorButton.highlightedColor = Color.blue;
          ColorButton.normalColor = Color.black;
          ColorButton.pressedColor = Color.gray;

          g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
        }
      }

      //
      g_phase = 1;
      g_isPhase = false;
    }
  }

  //Botones de todos los jugadores
  public void BtnPlayer1()
  {
    ChancellorSelected(0);
  }
  public void BtnPlayer2()
  {
    ChancellorSelected(1);
  }
  public void BtnPlayer3()
  {
    ChancellorSelected(2);
  }
  public void BtnPlayer4()
  {
    ChancellorSelected(3);
  }
  public void BtnPlayer5()
  {
    ChancellorSelected(4);
  }
  public void BtnPlayer6()
  {
    ChancellorSelected(5);
  }
  public void BtnPlayer7()
  {
    ChancellorSelected(6);
  }
  public void BtnPlayer8()
  {
    ChancellorSelected(7);
  }
  public void BtnPlayer9()
  {
    ChancellorSelected(8);
  }
  public void BtnPlayer10()
  {
    ChancellorSelected(9);
  }

  //Funcion de botones extra 
  public void BtnYes()
  {
    if (refLeg.bDoingVeto)
    {
      if (refGameMan.bServer)
      {
        //refLeg.veto();
        refGameMan.refServer.realizeVeto();
      }
      else
      {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "vetoyes";
        refGameMan.refCliente.SendeServer(nt);
      }
      return;
    }
    //g_listPlayerVote[g_iteratorPlayers] = 1;
    if (refGameMan.bServer)
    {
      refGameMan.refServer.playerVoteDic[0] = 2;
      refGameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "vote: Yes";
      refGameMan.refCliente.SendeServer(nt);
    }
    g_currentPlayerTxt.SetActive(false);
    g_BtnYes.SetActive(false);
    g_BtnNo.SetActive(false);
    m_alreadyVote = true;
  }
  public void BtnNO()
  {
    if (refLeg.bDoingVeto)
    {
      if (refGameMan.bServer)
      {
        refGameMan.refServer.cancelVeto();
      }
      else
      {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test = "vetono";
        refGameMan.refCliente.SendeServer(nt);
      }
      return;
    }
    //g_listPlayerVote[g_iteratorPlayers] = 1;
    if (refGameMan.bServer)
    {
      refGameMan.refServer.playerVoteDic[0] = 1;
      refGameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "vote: No";
      refGameMan.refCliente.SendeServer(nt);
    }
    g_currentPlayerTxt.SetActive(false);
    g_BtnYes.SetActive(false);
    g_BtnNo.SetActive(false);
    m_alreadyVote = true;
  }
  public void BtnNextTurn()
  {
    g_phase = 0;
    g_isPhase = false;

    g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 0;
    if (!specialSelection)
    {
      g_idPresident++;

      while (true)
      {
        if (g_idPresident != g_idOldPresident && g_idPresident < g_usuarios && !g_Players[g_idPresident].GetComponent<Jugador>().bIsDead)
          break;
        g_idPresident++;
        if (g_idPresident >= g_usuarios)
          g_idPresident = 0;
      }
    }
    //if (g_idPresident == g_idOldPresident)
    //{
    //  g_idPresident++;
    //
    //}
    //
    //if (g_idPresident >= g_usuarios)
    //{
    //  g_idPresident = 0;
    //}

    g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 1;

    if (g_idChancellor!=-1)
    {
      g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 0;
    }

    g_noCount = 0;
    g_isChancellor = false;

    g_BtnNextTurn.SetActive(false);
    if (refGameMan.bServer)
    {
      refGameMan.refServer.bWaitingGame = true;
    }
  }

  public void hidePlayers()
  {
    for (int i = 0; i < g_usuarios; i++)
    {
      //Solo es color del boton
      if (i < g_usuarios)
      {
        g_BtnPlayers[i].SetActive(false);
        g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = g_Players[i].GetComponent<Jugador>().Apodo;

        ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

        ColorButton.highlightedColor = Color.blue;
        ColorButton.normalColor = Color.white;
        ColorButton.pressedColor = Color.gray;

        g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
      }
      else
      {
        ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

        ColorButton.highlightedColor = Color.blue;
        ColorButton.normalColor = Color.black;
        ColorButton.pressedColor = Color.gray;

        g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
      }
    }
  }

  //Después de la selección del cansiller
  void ChancellorSelected(int _x)
  {
    if (refGameMan.bServer)
    {
      refGameMan.refServer.playerRedyDic[0] = 1;
      g_idChancellor = _x;
      g_isChancellor = true;
      refGameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "canciller:";
      nt.Test += _x;
      refGameMan.refCliente.SendeServer(nt);
    }
    //El presidente selecciona el canciller
    if (g_isChancellor)
    {
      g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 2;

      for (int i = 0; i < 10; i++)
      {
        //Solo es color del boton
        if (i < g_usuarios)
        {
          g_BtnPlayers[i].SetActive(false);
        }
        else
        {
          ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

          ColorButton.highlightedColor = Color.blue;
          ColorButton.normalColor = Color.black;
          ColorButton.pressedColor = Color.gray;

          g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
        }
      }

      //
      g_phase = 1;
      g_isPhase = false;
    }
  }
}