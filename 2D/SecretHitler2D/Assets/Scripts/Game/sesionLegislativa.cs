using System.Collections;
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
  public GameObject txtToVeto;
  public GameObject buttonToVeto;
  public GameObject buttonYesVeto;
  public GameObject buttonNoVeto;
  public GameObject txtPresidetnDoNoVeto;
  public GameObject txtSelectCard;
  public BarajaDePolizas baraja;
  public gameManager m_gameMan;
  public Elecciones m_eleccionsData;
  public bool faseAlready = false;
  public bool waitingNextTurn = false;
  public bool alredySelectedCard = false;
  public bool bFascist = false;
  public bool bCanVeto = false;
  public bool bDoingVeto = false;
  public int card0 = -1;
  public int card1 = -1;
  public int card2 = -1;

  float timeIA = 0;

  public void onStart()
  {
    baraja.onStart();
    m_gameMan = FindObjectOfType<gameManager>();
    m_eleccionsData = FindObjectOfType<Elecciones>();
    bCanVeto = true;
  }

  public void onUpdate()
  {
    switch (m_phase)
    {
      case 0:
        alredySelectedCard = false;
        bDoingVeto = false;
        if(baraja.numCartasFasColocadas>=5)
        {
          bCanVeto = true;
        }
        else
        {
          bCanVeto = false;
        }
        takeCardsFase();
        break;
      case 1:
        PresidentSelection();
        break;
      case 2:
        CansillerSelection();
        break;
      case 3:
        hideCansillerselection();
        waitingNextTurn = true;
        return;
      default:
        break;
    }
  }
  public void takeCardsFase()
  {
    if (m_gameMan.bServer && !m_gameMan.g_Players[m_eleccionsData.g_idPresident].GetComponent<Jugador>().connected)
    {
      if (timeIA>=3.0f)
      {
        takeCards();
        timeIA = 0;
      }
      else
      {
        timeIA += Time.deltaTime;
      }
    }
    if (m_gameMan.idConection != m_eleccionsData.g_idPresident)
    {
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtPickAChancellorP1();
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
        m_gameMan.g_Players[m_eleccionsData.g_idPresident].GetComponent<Jugador>().Apodo;
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtTakingCardsP2();
      m_gameMan.refRonda.txtIntructionFase.SetActive(true);

      return;
    }
    if (!faseAlready)
    {
      m_gameMan.refRonda.txtIntructionFase.SetActive(false);
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
      //nt.Test = Traslate.getTxtTakeCards();
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
    if (bDoingVeto)
    {
      return;
    }
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
    if (bDoingVeto)
    {
      return;
    }
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
    if (m_gameMan.bServer && !m_gameMan.g_Players[m_eleccionsData.g_idPresident].GetComponent<Jugador>().connected)
    {
      txtSelectCard.SetActive(false);
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtPickAChancellorP1();
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
        m_gameMan.g_Players[m_eleccionsData.g_idPresident].GetComponent<Jugador>().Apodo;
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtDiscardingCards();
      m_gameMan.refRonda.txtIntructionFase.SetActive(true);
      if (timeIA>=3.0f)
      {
        int slecction = Random.Range(0, 2);
        timeIA = 0;
        switch (slecction)
        {
          case 0:
            selectCard1();
            break;
          case 1:
            selectCard2();
            break;
          case 2:
            selectCard3();
            break;
        }
      }
      else
      {
        timeIA += Time.deltaTime;
      }
      
    }
    if (alredySelectedCard)
    {
      return;
    }
    if (m_gameMan.idConection != m_eleccionsData.g_idPresident)
    {
      txtSelectCard.SetActive(false);

      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtPickAChancellorP1();
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
        m_gameMan.g_Players[m_eleccionsData.g_idPresident].GetComponent<Jugador>().Apodo;
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtDiscardingCards();
      m_gameMan.refRonda.txtIntructionFase.SetActive(true);
      return;
    }



    if (m_gameMan.bServer)
    {
      m_gameMan.refRonda.txtIntructionFase.SetActive(false);
      txtSelectCard.SetActive(true);
      txtSelectCard.GetComponentInChildren<Text>().text = Traslate.getTxDiscardCard();
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
      m_gameMan.refRonda.txtIntructionFase.SetActive(false);
      txtSelectCard.SetActive(true);
      txtSelectCard.GetComponentInChildren<Text>().text = Traslate.getTxDiscardCard();
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
    txtSelectCard.SetActive(false);
    faseAlready = false;
    alredySelectedCard = false;
  }

  void CansillerSelection()
  {
    if (m_gameMan.bServer && !m_gameMan.g_Players[m_eleccionsData.g_idChancellor].GetComponent<Jugador>().connected)
    {
      txtSelectCard.SetActive(false);

      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtPlacingCardsP1();
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
        m_gameMan.g_Players[m_eleccionsData.g_idChancellor].GetComponent<Jugador>().Apodo;
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtPlacingCardsP2();
      m_gameMan.refRonda.txtIntructionFase.SetActive(true);
      if (timeIA >=3.0f)
      {
        int slecction = Random.Range(0, 1);
        timeIA = 0;
        switch (slecction)
        {
          case 0:
            selectCard1();
            break;
          case 1:
            selectCard2();
            break;
        }
      }
      else
      {
        timeIA += Time.deltaTime;
      }
     
    }
    if (alredySelectedCard)
    {
      return;
    }
    if (m_gameMan.idConection != m_eleccionsData.g_idChancellor)
    {
      txtSelectCard.SetActive(false);

      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtPlacingCardsP1();
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
        m_gameMan.g_Players[m_eleccionsData.g_idChancellor].GetComponent<Jugador>().Apodo;
      m_gameMan.refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtPlacingCardsP2();
      m_gameMan.refRonda.txtIntructionFase.SetActive(true);

      return;
    }
    if (m_gameMan.bServer)
    {
      txtSelectCard.SetActive(true);
      m_gameMan.refRonda.txtIntructionFase.SetActive(false);
      txtSelectCard.GetComponentInChildren<Text>().text = Traslate.getTxPlaceCard();
      ColorBlock ColorButton1 = buttonCansillerCard1.GetComponent<Button>().colors;
      colorButton(ref ColorButton1, 0);
      buttonCansillerCard1.GetComponent<Button>().colors = ColorButton1;
      buttonCansillerCard1.SetActive(true);

      ColorBlock ColorButton2 = buttonCansillerCard2.GetComponent<Button>().colors;
      colorButton(ref ColorButton2, 1);
      buttonCansillerCard2.GetComponent<Button>().colors = ColorButton2;
      buttonCansillerCard2.SetActive(true);
      faseAlready = true;
      alredySelectedCard = true;
      if (bCanVeto)
      {
        showVeto();
      }
    }
    else
    {
      txtSelectCard.SetActive(true);
      m_gameMan.refRonda.txtIntructionFase.SetActive(false);
      txtSelectCard.GetComponentInChildren<Text>().text = Traslate.getTxPlaceCard();
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
      if (bCanVeto)
      {
        showVeto();
      }
    }
  }

  public void hideCansillerselection()
  {
    buttonCansillerCard1.SetActive(false);
    buttonCansillerCard2.SetActive(false);
    txtSelectCard.SetActive(false);//////////////////////////////////////////
  }

  public void CansillerEndSelection()
  {
    buttonCansillerCard1.SetActive(false);
    buttonCansillerCard2.SetActive(false);
    txtSelectCard.SetActive(false);////////////////////////////////////////
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

  public void addTopCard(ref int type)
  {
    baraja.addTopCard(ref type);
  }

  public void showVeto()
  {
    txtToVeto.GetComponentInChildren<Text>().text = Traslate.getTxtDoVeto();
    txtToVeto.SetActive(true);
    buttonToVeto.SetActive(true);
  }

  public void anwerForVeto()
  {
    bDoingVeto = true;
    txtToVeto.GetComponentInChildren<Text>().text = Traslate.getTxtWaitForVeto();
    buttonToVeto.SetActive(false);
    if (m_gameMan.bServer)
    {
      baraja.selectCard3();
      m_gameMan.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "doveto";
      m_gameMan.refCliente.SendeServer(nt);
    }
  }

  public void precidentDecideForVeto()
  {
    if (m_gameMan.idConection != m_eleccionsData.g_idPresident)
    {
      return;
    }
    bDoingVeto = true;
    buttonYesVeto.SetActive(true);
    buttonNoVeto.SetActive(true);
    txtPresidetnDoNoVeto.GetComponentInChildren<Text>().text = Traslate.getTxtPresidentVeto();
    txtPresidetnDoNoVeto.SetActive(true);
  }

  public void cancelVeto()
  {
    bDoingVeto = false;
    buttonYesVeto.SetActive(false);
    buttonNoVeto.SetActive(false);
    txtPresidetnDoNoVeto.SetActive(false);
    txtToVeto.SetActive(false);
    buttonToVeto.SetActive(false);
  }

  public void veto()
  {
    bDoingVeto = false;
    if (m_gameMan.bServer)
    {
      baraja.veto();
    }
    buttonYesVeto.SetActive(false);
    buttonNoVeto.SetActive(false);
    txtPresidetnDoNoVeto.SetActive(false);
    txtToVeto.SetActive(false);
    buttonToVeto.SetActive(false);
    buttonCansillerCard1.SetActive(false);
    buttonCansillerCard2.SetActive(false);
    EndSelecction();
  }
}
