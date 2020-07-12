using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  // Start is called before the first frame update
  void Start()
  {
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
      if (!refServer)
      {
         g_Players[i].GetComponent<Jugador>().Apodo = refCliente.playerApode[i];
      }
      else
      {
         g_Players[i].GetComponent<Jugador>().Apodo = refServer.playerApode[i];
      }
    }
    refRonda.m_fElecciones.g_Players = g_Players;
  }

  // Update is called once per frame
  void Update()
  {
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
    refRonda.onUpdate();
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
}
