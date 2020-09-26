using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class preGameManager : MonoBehaviour
{
  Server refServer;
  Client refClient;
  bool bServer = true;
  bool bCanRun = true;
  public GameObject readybutton;
  // Start is called before the first frame update
  void Start()
  {
    refServer = FindObjectOfType<Server>();
    if (refServer)
    {
      refServer.Init();
    }
    else
    {
      bServer = false;
      refClient = FindObjectOfType<Client>();
      if (refClient)
      {
        refClient.loadApodo();
        //refClient.Init();
      }
      else
      {
        bCanRun = false;
        Debug.Log("No se eoncotro ni servidor ni cliente");
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (!bCanRun)
      return;
    if (bServer)
    {
      refServer.preGameUpdate();
    }
    else
    {
      refClient.preGameUpdate();
      if (refClient.connected)
      {
        readybutton.GetComponent<Button>().interactable = true;
      }
      else
      {
        readybutton.GetComponent<Button>().interactable = false;
      }
    }
  }

  public void back()
  {
    SceneManager.LoadScene("SelectMode");
  }
}
