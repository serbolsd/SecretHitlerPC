﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sesionLegislativa : MonoBehaviour
{
  public int m_phase = 3;
  public GameObject buttonTakeCard;
  public GameObject buttonPresidentCard1;
  public GameObject buttonPresidentCard2;
  public GameObject buttonPresidentCard3;
  public GameObject buttonCansillerCard1;
  public GameObject buttonCansillerCard2;
  public BarajaDePolizas baraja;
  public gameManager m_gameMan;
  public Elecciones m_eleccionsData;
  public bool faseAlready = false;
  public bool waitingNextTurn = false;
  public bool alredySelectedCard = false;
  public int card0 = -1;
  public int card1 = -1;
  public int card2 = -1;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void onStart()
  {
    baraja.onStart();
    m_gameMan = FindObjectOfType<gameManager>();
    m_eleccionsData = FindObjectOfType<Elecciones>();

  }

  public void onUpdate()
  {
    switch (m_phase)
    {
      case 0:
        alredySelectedCard = false;
        takeCardsFase();
        break;
      case 1:
        PresidentSelection();
        break;
      case 2:
        CansillerSelection();
        break;
      case 3:
        waitingNextTurn = true;
        return;
      default:
        break;
    }
  }
  public void takeCardsFase()
  {
    if (m_gameMan.idConection != m_eleccionsData.g_idPresident)
    {
      return;
    }
    if (!faseAlready)
    {
      buttonTakeCard.SetActive(true);
      faseAlready = true;

    }
  }

  public void hideTakeCard()
  {
    faseAlready = false;
    buttonTakeCard.SetActive(false);
  }

  public void takeCards()
  {

    if (!m_gameMan.bServer)
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "takeCards";
      m_gameMan.refCliente.SendeServer(nt);
      faseAlready = false;
      buttonTakeCard.SetActive(false);
    }
    else
    {
      m_gameMan.refServer.bWaitingGame = true;
      m_phase++;
      faseAlready = false;
      baraja.takeCards();
      buttonTakeCard.SetActive(false);
    }
  }
  public void selectCard1()
  {
    if (m_gameMan.bServer)
    {
      baraja.selectCard1();
      m_gameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "car0";
      m_gameMan.refCliente.SendeServer(nt);
    }
    EndSelecction();
  }

  public void selectCard2()
  {
    if (m_gameMan.bServer)
    {
      baraja.selectCard2();
      m_gameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "car1";
      m_gameMan.refCliente.SendeServer(nt);
    }
    EndSelecction();
  }

  public void selectCard3()
  {
    if (m_gameMan.bServer)
    {
      baraja.selectCard3();
      m_gameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "car2";
      m_gameMan.refCliente.SendeServer(nt);
    }
    EndSelecction();
  }

  void PresidentSelection()
  {
    if (m_gameMan.idConection != m_eleccionsData.g_idPresident || alredySelectedCard)
    {
      return;
    }

    if (m_gameMan.bServer)
    {
      ColorBlock ColorButton1 = buttonPresidentCard1.GetComponent<Button>().colors;
      colorButton(ref ColorButton1, 0);
      buttonPresidentCard1.GetComponent<Button>().colors = ColorButton1;
      buttonPresidentCard1.SetActive(true);

      ColorBlock ColorButton2 = buttonPresidentCard2.GetComponent<Button>().colors;
      colorButton(ref ColorButton2, 1);
      buttonPresidentCard2.GetComponent<Button>().colors = ColorButton2;
      buttonPresidentCard2.SetActive(true);

      ColorBlock ColorButton3 = buttonPresidentCard3.GetComponent<Button>().colors;
      colorButton(ref ColorButton3, 2);
      buttonPresidentCard3.GetComponent<Button>().colors = ColorButton3;
      buttonPresidentCard3.SetActive(true);
      faseAlready = true;
      alredySelectedCard = true;
    }
    else
    {
      ColorBlock ColorButton1 = buttonPresidentCard1.GetComponent<Button>().colors;
      colorButtonForClient(ref ColorButton1, card0);
      buttonPresidentCard1.GetComponent<Button>().colors = ColorButton1;
      buttonPresidentCard1.SetActive(true);

      ColorBlock ColorButton2 = buttonPresidentCard2.GetComponent<Button>().colors;
      colorButtonForClient(ref ColorButton2, card1);
      buttonPresidentCard2.GetComponent<Button>().colors = ColorButton2;
      buttonPresidentCard2.SetActive(true);

      ColorBlock ColorButton3 = buttonPresidentCard3.GetComponent<Button>().colors;
      colorButtonForClient(ref ColorButton3, card2);
      buttonPresidentCard3.GetComponent<Button>().colors = ColorButton3;
      buttonPresidentCard3.SetActive(true);
      faseAlready = true;
      alredySelectedCard = true;
    }

  }

  public void hidePresidentSelection()
  {
    buttonPresidentCard1.SetActive(false);
    buttonPresidentCard2.SetActive(false);
    buttonPresidentCard3.SetActive(false);
  }

  public void PresidentEndSelection()
  {
    buttonPresidentCard1.SetActive(false);
    buttonPresidentCard2.SetActive(false);
    buttonPresidentCard3.SetActive(false);
    faseAlready = false;
    alredySelectedCard = false;
  }

  void CansillerSelection()
  {
    if (m_gameMan.idConection != m_eleccionsData.g_idChancellor || alredySelectedCard)
    {
      return;
    }
    if (!faseAlready)
    {
      if (m_gameMan.bServer)
      {
        ColorBlock ColorButton1 = buttonCansillerCard1.GetComponent<Button>().colors;
        colorButtonForClient(ref ColorButton1, 0);
        buttonCansillerCard1.GetComponent<Button>().colors = ColorButton1;
        buttonCansillerCard1.SetActive(true);

        ColorBlock ColorButton2 = buttonCansillerCard2.GetComponent<Button>().colors;
        colorButtonForClient(ref ColorButton2, 1);
        buttonCansillerCard2.GetComponent<Button>().colors = ColorButton2;
        buttonCansillerCard2.SetActive(true);
        faseAlready = true;
        alredySelectedCard = true;
      }
      else
      {
        ColorBlock ColorButton1 = buttonCansillerCard1.GetComponent<Button>().colors;
        colorButtonForClient(ref ColorButton1, card0);
        buttonCansillerCard1.GetComponent<Button>().colors = ColorButton1;
        buttonCansillerCard1.SetActive(true);

        ColorBlock ColorButton2 = buttonCansillerCard2.GetComponent<Button>().colors;
        colorButtonForClient(ref ColorButton2, card1);
        buttonCansillerCard2.GetComponent<Button>().colors = ColorButton2;
        buttonCansillerCard2.SetActive(true);
        faseAlready = true;
        alredySelectedCard = true;
      }

    }
  }

  public void hideCansillerselection()
  {
    buttonCansillerCard1.SetActive(false);
    buttonCansillerCard2.SetActive(false);
  }

  public void CansillerEndSelection()
  {
    buttonCansillerCard1.SetActive(false);
    buttonCansillerCard2.SetActive(false);
    faseAlready = false;
  }

  void EndSelecction()
  {
    if (m_phase == 1)
    {
      PresidentEndSelection();
    }
    else
    {
      CansillerEndSelection();
    }
    m_phase++;
    if (m_phase > 3)
      m_phase = 3;
  }

  void colorButton(ref ColorBlock color, int indexCard)
  {
    if (baraja.enMano[indexCard].GetComponent<CartaDePoliza>().tipoDeCarta == 1)
    {
      color.highlightedColor = Color.red;
      color.normalColor = Color.red;
      color.pressedColor = Color.red;
    }
    else
    {
      color.highlightedColor = Color.blue;
      color.normalColor = Color.blue;
      color.pressedColor = Color.blue;
    }
  }

  void colorButtonForClient(ref ColorBlock color, int tipecard)
  {
    if (tipecard == 1)
    {
      color.highlightedColor = Color.red;
      color.normalColor = Color.red;
      color.pressedColor = Color.red;
    }
    else
    {
      color.highlightedColor = Color.blue;
      color.normalColor = Color.blue;
      color.pressedColor = Color.blue;
    }
  }
}
