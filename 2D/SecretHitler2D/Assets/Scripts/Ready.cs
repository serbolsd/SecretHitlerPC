using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Ready : MonoBehaviour
{

  int ready = 0;

  public GameObject readybutton;
  public void imReady()
  {
    Server tmpServer = FindObjectOfType<Server>();
    if (tmpServer)
    {
      tmpServer.playerRedyDic[0] = 1;
      return;
    }
    ready = 1;

    Net_MessageTest apod = new Net_MessageTest();
    apod.Test = "apodo_";
    apod.Test += FindObjectOfType<Client>().m_myApodo.text;
    Debug.Log(apod.Test);
    FindObjectOfType<Client>().SendeServer(apod);


    Net_MessageTest nt = new Net_MessageTest();
    nt.Test = "ready: 1";
    FindObjectOfType<Client>().SendeServer(nt);


    ColorBlock color = readybutton.GetComponent<Button>().colors;
    color.normalColor = Color.green;
    color.selectedColor = Color.green;
    readybutton.GetComponent<Button>().colors = color;

  }
}
