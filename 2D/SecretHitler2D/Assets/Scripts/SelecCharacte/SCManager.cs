using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCManager : MonoBehaviour
{
  public GameObject[] playerList= new GameObject[10];
  public Server refServer;
  public Client refClient;
  public GameObject apodo;
  public GameObject Play;
  public characterSelected characSelec;
  public bool reorganize = false;
  
  //public characterSelected characters;
  // Start is called before the first frame update
  void Start()
  {
    Play.SetActive(false);
    refServer = FindObjectOfType<Server>();
    if (refServer)
    {
      Play.SetActive(true);
      refServer.selectCharacterInit();
      apodo.GetComponentInChildren<Text>().text = refServer.savedapodo;
    }
    refClient = FindObjectOfType<Client>();
    if (refClient)
    {
      refClient.selectCharacterInit();
      apodo.GetComponentInChildren<Text>().text = refClient.apodo;
    }
    characSelec = FindObjectOfType<characterSelected>();
    characSelec.onStart();
  }

  // Update is called once per frame
  void Update()
  {
    if (refServer)
    {
      refServer.selectCharacterUpdate();
    }
    if (refClient && refClient.connected)
    {
      refClient.selecCharacterUpdate();

    }
    else if (refClient && !refClient.connected)
    {
      
    }
    updateAssitences();
  }

  public void activatePlayer(int id, string apodo)
  {
    if (id<0||id>9)
    {
      return;
    }

    playerList[id].GetComponentInChildren<Text>().text = apodo;
    playerList[id].SetActive(true);
  }

  public void desactivePlayer(int id)
  {
    playerList[id].SetActive(false);
  }

  public void updateAssitences()
  {
    if (refServer)
    {
      for (int i = 0; i < 10; i++)
      {
        if (i<refServer.playerApode.Count)
        {
          activatePlayer(i, refServer.playerApode[refServer.playerId[refServer.conections[i]]]);
          characSelec.apodos[i] = refServer.playerApode[i];
        }
        else
        {
          desactivePlayer(i);
        }

      }
    }
    if (refClient)
    {
      if (apodo.GetComponentInChildren<Text>().text != refClient.apodo)
      { 
        apodo.GetComponentInChildren<Text>().text = refClient.apodo;
      }
      for (int i = 0; i < 10; i++)
      {
        if (refClient.playerApode[i]!="N")
        {
          activatePlayer(i, refClient.playerApode[i]);
          characSelec.apodos[i] = refClient.playerApode[i];
        }
        else
        {
          desactivePlayer(i);
        }
      }
      if (reorganize)
      {
        reorganize = false;
        FindObjectOfType<characterSelected>().clientReorganize();
      }
    }
  }

  public void disconnect()
  {
    if (refServer)
    {
      refServer.Shutdown();
      Destroy(FindObjectOfType<Server>().gameObject);
      Destroy(FindObjectOfType<characterSelected>().gameObject);
      SceneManager.LoadScene("Server");
    }
    if (refClient)
    {
      refClient.Shutdown();
      Destroy(FindObjectOfType<characterSelected>().gameObject);
      SceneManager.LoadScene("Client");
    }
  }

  public void goToGame()
  {
    refServer.goToGame();
  }

  public void selecCharacter(int id)
  {
    if (refServer)
    {
      characSelec.selectCharac(id,0);
    }
    else
    {
      characSelec.selectCharac(id, refClient.myId);
    }
  }
}
