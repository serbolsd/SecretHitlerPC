using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
  Ronda refRonda;
  public Server refServer;
  public Client refCliente;
  public bool bServer = true;
  public int idConection;
  public GameObject g_PrefabPlayer;
  public GameObject[] g_Players;
  public int m_numUsuarios;
  public bool canWinFascist = false;
  public bool fascistWon = false;
  public bool liberalWon = false;
  public bool gameFinished = false;

  public GameObject textTofinal;
  public GameObject buttonToMenu;
  // Start is called before the first frame update
  void Start()
  {

    //FindObjectOfType<Reglas>().onStart();
    refRonda = FindObjectOfType<Ronda>();
    refServer = FindObjectOfType<Server>();
    if (!refServer)
    {
      bServer = false;
      refCliente = FindObjectOfType<Client>();
      refCliente.onStartGame();
      FindObjectOfType<Seleccion_Roll>().numPlayers = m_numUsuarios = refCliente.m_numPlayers;
      idConection = refCliente.myId;
    }
    else
    {
      idConection = 0;
      refServer.onStartGame();
      FindObjectOfType<Seleccion_Roll>().numPlayers = m_numUsuarios = refServer.playerRedyDic.Count;
    }
    FindObjectOfType<Seleccion_Roll>().createRole();
    FindObjectOfType<afiliaciones>().darAfiliacion();
    refRonda.onStart();
    g_Players = new GameObject[FindObjectOfType<Seleccion_Roll>().numPlayers];
    for (int i = 0; i < FindObjectOfType<Seleccion_Roll>().numPlayers; i++)
    {
      g_Players[i] = Instantiate(g_PrefabPlayer, this.transform);
    }
    for (int i = 0; i < m_numUsuarios; i++)
    {
      //Genera una instancia del prefab
      //Al ponerlo en la posición this lo vuelve su hijo
      //g_Players[i] = Instantiate(g_PrefabPlayer, this.transform);
      g_Players[i].GetComponent<Jugador>().ID = i;
      g_Players[i].GetComponent<Jugador>().Cargo = 0;
      g_Players[i].GetComponent<Jugador>().Rol = FindObjectOfType<Seleccion_Roll>().idsRol[i];
      g_Players[i].GetComponent<Jugador>().afil = FindObjectOfType<afiliaciones>().afil[i];
      if ("Fascista" == g_Players[i].GetComponent<Jugador>().afil)
      {
        g_Players[i].GetComponent<Jugador>().idAfiliation = 1;
      }
      else
      {
        g_Players[i].GetComponent<Jugador>().idAfiliation = 0;
      }
      if (!refServer)
      {
         g_Players[i].GetComponent<Jugador>().Apodo = refCliente.playerApode[i];
      }
      else
      {
         g_Players[i].GetComponent<Jugador>().Apodo = refServer.playerApode[i];
      }
    }
    for (int i = 0; i < FindObjectOfType<Seleccion_Roll>().numPlayers; i++)
    {
      //Genera una instancia del prefab
      //Al ponerlo en la posición this lo vuelve su hijo
      //g_Players[i] = Instantiate(g_PrefabPlayer, this.transform);
      g_Players[i].GetComponent<Jugador>().ID = i;
      g_Players[i].GetComponent<Jugador>().Cargo = 0;
      g_Players[i].GetComponent<Jugador>().Rol = FindObjectOfType<Seleccion_Roll>().idsRol[i];
      g_Players[i].GetComponent<Jugador>().afil = FindObjectOfType<afiliaciones>().afil[i];
      if ("Fascista" == g_Players[i].GetComponent<Jugador>().afil)
      {
        g_Players[i].GetComponent<Jugador>().idAfiliation = 1;
      }
      else
      {
        g_Players[i].GetComponent<Jugador>().idAfiliation = 0;
      }
    }
    if(bServer)
    {
      for (int i = 0; i < FindObjectOfType<Seleccion_Roll>().numPlayers; i++)
      {
        refServer.sendRol(i, g_Players[i].GetComponent<Jugador>().Rol);
      }
      for (int i = 0; i < FindObjectOfType<Seleccion_Roll>().numPlayers; i++)
      {
        refServer.sendAfiliations(i, g_Players[i].GetComponent<Jugador>().idAfiliation);
      }

    }
    refRonda.m_fElecciones.g_Players = g_Players;
    FindObjectOfType<SetApodoRo>().onInit();
    FindObjectOfType<Reglas>().numPlayers = FindObjectOfType<Seleccion_Roll>().numPlayers;
    FindObjectOfType<Reglas>().onStart();
  }

  // Update is called once per frame
  void Update()
  {
    if (gameFinished)
    {
      activeButtonFinished();
      return;
    }
    if (bServer)
    {
      refServer.gameUpdate();

    }
    else
    {
      refCliente.gameUpdate();
      if (refCliente.wait)
      {
        return;
      }
    }
    if (!liberalWon && !fascistWon)
    {
      refRonda.onUpdate();
    }
    else
    {
      if (bServer)
      {
        refServer.bWaitingGame = true;
        refServer.bContinueGame = true;
      }
    }
    if (bServer)
    {
      if (!refServer.bContinueGame || !refServer.bWaitingGame)
      {
        return;
      }
      refServer.sentInfoGame();
    }
    else
    {
    }
  }

  void activeButtonFinished()
  {
    if (liberalWon)
    {
      //textTofinal.GetComponent<Text>().text = "Liberal won";
      textTofinal.GetComponentInChildren<Text>().text = Traslate.getTxtLiberal();
    }
    else
    {
      //textTofinal.GetComponent<Text>().text = "Fascist won";
      textTofinal.GetComponentInChildren<Text>().text = Traslate.getTxtFascistWon();
    }
    textTofinal.SetActive(true);
    buttonToMenu.SetActive(true);
  }

  public void goMenu()
  {
    SceneManager.LoadScene("SelectMode");
  }
}
