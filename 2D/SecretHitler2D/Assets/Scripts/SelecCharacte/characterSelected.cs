using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterSelected : MonoBehaviour
{

  public string apodo0,apodo1,apodo2,apodo3,apodo4,apodo5,apodo6,apodo7,apodo8,apodo9;
  public string[] apodos= new string[10];
  public GameObject[] characters = new GameObject[10];
  public Sprite[] spriteLiberales = new Sprite[10];
  public Sprite[] spriteFascistas = new Sprite[10];
  public Sprite[] spriteHitler = new Sprite[10];
  public GameObject[] textcharacters = new GameObject[10];
  public int[] selected = new int[10];
  Server refServer;
  Client refClient;

  public ColorBlock colorSelected;
  public ColorBlock colorNotSelected;
  public void onStart()
  {
    DontDestroyOnLoad(gameObject);
    refServer = FindObjectOfType<Server>();
    refClient = FindObjectOfType<Client>();
    for (int i = 0; i < 10; i++)
    {
      selected[i] = -1;
    }
    //colorSelected.normalColor = new Color(92/255, 92 / 255, 92 / 255, 255 / 255);
    //colorSelected.selectedColor = new Color(92 / 255, 92 / 255, 92 / 255, 255);
    //colorSelected.highlightedColor = new Color(255, 255, 255, 255);
    //colorSelected.pressedColor = new Color(200 / 255, 200 / 255, 200 / 255, 255);
    //colorSelected.colorMultiplier = 1;
    //
    //colorNotSelected.normalColor = new Color(255, 255, 255, 255);
    //colorNotSelected.selectedColor = new Color(255, 255, 255, 255);
    //colorNotSelected.highlightedColor = new Color(255, 255, 255, 255);
    //colorNotSelected.pressedColor = new Color(200 / 255, 200 / 255, 200 / 255, 255);
    //colorNotSelected.colorMultiplier = 1;
  }

  public void setApodo(int id, string apodo)
  {
    switch (id)
    {
      case 0:
        apodo0 = apodo;
        apodos[0] = apodo;
        break;
      case 1:
        apodo1 = apodo;
        apodos[1] = apodo;
        break;
      case 2:
        apodo2 = apodo;
        apodos[2] = apodo;
        break;
      case 3:
        apodo3 = apodo;
        apodos[3] = apodo;
        break;
      case 4:
        apodo4 = apodo;
        apodos[4] = apodo;
        break;
      case 5:
        apodo5 = apodo;
        apodos[5] = apodo;
        break;
      case 6:
        apodo6 = apodo;
        apodos[6] = apodo;
        break;
      case 7:
        apodo7 = apodo;
        apodos[7] = apodo;
        break;
      case 8:
        apodo8 = apodo;
        apodos[8] = apodo;
        break;
      case 9:
        apodo9 = apodo;
        apodos[9] = apodo;
        break;
      default:
        break;
    }
  }

  public string getApodo(int id)
  {
    switch (id)
    {
      case 0:
        return apodo0;
      case 1:
        return apodo1;
      case 2:
        return apodo2;
      case 3:
        return apodo3;
      case 4:
        return apodo4;
      case 5:
        return apodo5;
      case 6:
        return apodo6;
      case 7:
        return apodo7;
      case 8:
        return apodo8;
      case 9:
        return apodo9;
      default:
        break;
    }
    return "";
  }

  public void selectCharac(int id,int playerId)
  {
    if (refServer)
    {
      setSelected(id, playerId);
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test += "P";
      nt.Test += playerId.ToString();
      nt.Test += "C_";
      nt.Test += id.ToString();

      for (int i = 0; i < refServer.conections.Count; i++)
      {
        if (i != 0)
        {
          Debug.Log(refServer.conections[i]);
          refServer.SendClient(refServer.conections[i], nt);
        }
      }
    }
    if (refClient)
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "c";
      nt.Test += id.ToString();
      FindObjectOfType<Client>().SendeServer(nt);
    }

  }

  public void unselecte(int id)
  {
    selected[id] = -1;
    characters[id].GetComponent<Image>().color = new Color(255, 255, 255, 255);
    characters[id].GetComponent<Button>().colors = colorNotSelected;
    textcharacters[id].GetComponent<Text>().gameObject.SetActive(false);
  }

  public bool checkIfCan(int id)
  {

    if (selected[id] != -1)
    {
      return false;
    }
    return true;
  }

  public void setSelected(int id, int playerId)
  {
    if (id==-1 || !checkIfCan(id))
    {
      return;
    }
   
    for (int i = 0; i < 10; i++)
    {
      textcharacters[id].GetComponent<Text>().text = apodos[playerId];
      if (selected[i] == playerId)
      {
        selected[i] = -1;
        characters[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        characters[i].GetComponent<Button>().colors = colorNotSelected;
        textcharacters[i].GetComponent<Text>().gameObject.SetActive(false);
      }
    }
    selected[id] = playerId;
    characters[id].GetComponent<Image>().color = new Color(92, 92, 92, 255);
    characters[id].GetComponent<Button>().colors = colorSelected;
    textcharacters[id].GetComponent<Text>().gameObject.SetActive(true);
  }

  public void desconectPlayer(int playerid)
  {
    int cid = -1;
    for (int i = 0; i < 10; i++)
    {
      if (selected[i] == playerid)
      {
        selected[i] = -1;
        characters[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        characters[i].GetComponent<Button>().colors = colorNotSelected;
        textcharacters[i].GetComponent<Text>().gameObject.SetActive(false);
        cid = i;
        break;
      }
    }
    Net_MessageTest nt = new Net_MessageTest();
    nt.Test += "C";
    nt.Test += cid.ToString();
    nt.Test += "_";
    nt.Test += "-1";

    for (int i = 0; i < refServer.conections.Count; i++)
    {
      if (i != 0)
      {
        Debug.Log(refServer.conections[i]);
        refServer.SendClient(refServer.conections[i], nt);
      }
    }
  }

  public void reorganize()
  {
    var ser = FindObjectOfType<Server>();
    for (int i = 0; i < 10; i++)
    {
      for (int j = 0; j < ser.playerId.Count; j++)
      {
        if (textcharacters[i].GetComponent<Text>().text == ser.playerApode[ser.playerId[ser.conections[j]]])
        {
          selected[i] = ser.playerId[ser.conections[j]];
          break;
        }
      }
    }
  }
  public void clientReorganize()
  {
    for (int i = 0; i < 10; i++)
    {
      for (int j = 0; j < 10; j++)
      {
        if (apodos[j] == "N" || apodos[j] == "")
        {
          break;
        }
        if (apodos[j] == textcharacters[i].GetComponent<Text>().text)
        {
          selected[i] = j;
          selected[i] = -1;
          break;
        }
      }
    }
    activatesPlayerUpdate();
  }

  void activatesPlayerUpdate()
  {
    for (int i = 0; i < 10; i++)
    {
      if (selected[i]!=-1)
      {
        textcharacters[i].GetComponent<Text>().text = apodos[selected[i]];
        characters[i].GetComponent<Image>().color = new Color(92, 92, 92, 255);
        characters[i].GetComponent<Button>().colors = colorSelected;
        textcharacters[i].GetComponent<Text>().gameObject.SetActive(true);
      }
    }
  }

  public void sendCharacters()
  {
    for (int i = 0; i < 10; i++)
    {
      if (selected[i]!=-1)
      {
        Net_MessageTest nt = new Net_MessageTest();
        nt.Test += "P";
        nt.Test += selected[i].ToString();
        nt.Test += "C_";
        nt.Test += i.ToString();

        for (int j = 0; j < refServer.conections.Count; j++)
        {
          if (j != 0)
          {
            Debug.Log(refServer.conections[j]);
            refServer.SendClient(refServer.conections[j], nt);
          }
        }
      }
    }
    
  }
}
