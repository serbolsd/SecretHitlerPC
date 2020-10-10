using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SessionEjecutiva : MonoBehaviour
{
  public int gamePowersForNumPlayer = -1;//0= 5,6 ; 1= 7,8 ; 2= 9,10
  public int cartasFascistasColocadas = 0;
  public bool waitingNextTurn = false;
  public bool poderActivo = false;
  public sesionLegislativa refLeg;
  public Elecciones refElec;
  public Ronda refRonda;
  public gameManager refman;
  public bool bAlredyInitPhase = false;
  public GameObject ButtonOK;
  public GameObject MembershipCard;
  public GameObject TextPower;
  public int phase = 0;
  public GameObject[] cardsToPreview = new GameObject[3];
  public GameObject[] cardsToPreviewTop = new GameObject[3];
  public GameObject[] g_BtnPlayers = new GameObject[10];

  public int card0=-1;
  public int card1=-1;
  public int card2=-1;

  public int playerEjecuted = -1;

  public int PowerToSelectPlayer = -1;// 0= ejecution 1= ver lealtad 2= eleccion especial   -1=NADA 3=verpolizas;

  bool liberalWin = false;
  bool fascistWin = false;
  public bool bhideButtonAndText = false;
  public bool bhideButtons = false;
  public bool bAlredyDrawMembership = false;
  public bool bSeeingMembership = false;
  public bool alredydoit = false;

  public void onInit()
  {
    refLeg = FindObjectOfType<sesionLegislativa>();
    refElec = FindObjectOfType<Elecciones>();
    refRonda = FindObjectOfType<Ronda>();
    refman = FindObjectOfType<gameManager>();
  }

  public void onUpdate()
  {
    cartasFascistasColocadas = refLeg.baraja.numCartasFasColocadas;
    switch (phase)
    {
      case 0://button ok
        bhideButtonAndText = false;
        bAlredyDrawMembership = false;
        bSeeingMembership = false;
        switch (gamePowersForNumPlayer)
        {
          case 0:
            checkPowers56();
            break;
          case 1:
            checkPowers78();
            break;
          case 2:
            checkPowers910();
            break;
          default:
            break;
        }
        if (phase==0)
        {
          waitOK();
        }

        break;
      case 1: //power
        powerPhase();
        break;
      case 2://finish
        hideAll();
        waitingNextTurn = true;
        refLeg.baraja.canEjecutive = false;
        HideInstructionToNoPresident();
        break;
      default:
        break;
    }
  }

  void checkPowers56()
  {
    switch (cartasFascistasColocadas)
    {
      case 0:
        phase = 2;
        break;
      case 1:
        hideButtonOkAndTextPower();
        //TextPower.GetComponent<Text>().text = Traslate.getTxtEjecute();
        //TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 2:
        hideButtonOkAndTextPower();
        //TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 3:
        //TextPower.GetComponent<Text>().text = Traslate.getTxtSpecialElection();
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtPolicyReview();
        break;
      case 4:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 5:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void checkPowers78()
  {
    switch (cartasFascistasColocadas)
    {
      case 0:
        phase = 2;
        break;
      case 1:
        //nada
        hideButtonOkAndTextPower();
        break;
      case 2:
        //verLealtad(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtSeeMembership(); ;
        break;
      case 3:
        //EleccionEspecial(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtSpecialElection(); ;
        break;
      case 4:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 5:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void checkPowers910()
  {
    switch (cartasFascistasColocadas)
    {
      case 0:
        phase = 2;
        break;
      case 1:
        //verLealtad(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtSeeMembership();
        break;
      case 2:
        //verLealtad(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtSeeMembership();
        break;
      case 3:
        //EleccionEspecial(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtSpecialElection();
        break;
      case 4:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 5:
        //Ejecucion(playerId);
        TextPower.GetComponentInChildren<Text>().text = Traslate.getTxtEjecute();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void waitOK()
  {
    if (!bAlredyInitPhase)
    {
      if (refman.idConection == refElec.g_idPresident)
      {
        bhideButtonAndText = false;
        ButtonOK.SetActive(true);
        TextPower.SetActive(true);
      }
      bAlredyInitPhase = true;
      if (refman.bServer)
      {
        refman.refServer.bWaitingGame = true;
      }
    }
    else
    {
      if (refman.bServer && !refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().connected)
      {
        OKbutton();
      }
    }
  }

  public void hideButtonOkAndTextPower()
  {
    ButtonOK.SetActive(false);
    TextPower.SetActive(false);
    bAlredyInitPhase = false;
    bhideButtonAndText = true;
    if (refman.bServer)
    {
      refman.refServer.bWaitingGame = true;
    }
    phase = 1;
  }

  void powerPhase()
  {
    switch (gamePowersForNumPlayer)
    {
      case 0:
        de5a6Jugadores();
        break;
      case 1:
        de7a8Jugadores();
        break;
      case 2:
        de9a10Jugadores();
        break;
      default:
        break;
    }
  }

  public void OKbutton()
  {
    switch (phase)
    {
      case 0:
        hideButtonOkAndTextPower();
        if (!refman.bServer)
        {
          Net_MessageTest nt = new Net_MessageTest();
          nt.Test = "OKejec";
          refman.refCliente.SendeServer(nt);
        }
        break;
      case 1:
        if (!refman.bServer)
        {
          Net_MessageTest nt = new Net_MessageTest();
          nt.Test = "OKejec";
          refman.refCliente.SendeServer(nt);
        }
        hideAll();
        break;
      default:
        break;
    }
  }

  void de5a6Jugadores()
  {
    switch (cartasFascistasColocadas)
    {
      case 1:
        //EleccionEspecial();
        //verLealtad();
        //Ejecucion();
        //revisionDePolizas();
        hideAll();
        if (refman.bServer)
        {
          phase = 2;
          refman.refServer.bWaitingGame = true;
        }
        //nada
        break;
      case 2:
        //EleccionEspecial();
        //verLealtad();
        //Ejecucion();
        //revisionDePolizas();
        hideAll();
        if (refman.bServer)
        {
          phase = 2;
          refman.refServer.bWaitingGame = true;
        }
        //nada
        break;
      case 3:
        //EleccionEspecial();
        revisionDePolizas();
        break;
      case 4:
        Ejecucion();
        break;
      case 5:
        Ejecucion();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void de7a8Jugadores()
  {
    if (bAlredyInitPhase)
      return;
    switch (cartasFascistasColocadas)
    {
      case 1:
        hideAll();
        if (refman.bServer)
        {
          phase = 2;
          refman.refServer.bWaitingGame = true;
        }
        break;
      case 2:
        verLealtad();
        break;
      case 3:
        EleccionEspecial();
        break;
      case 4:
        Ejecucion();
        break;
      case 5:
        Ejecucion();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void de9a10Jugadores()
  {
    switch (cartasFascistasColocadas)
    {
      case 1:
        verLealtad();
        break;
      case 2:
        verLealtad();
        break;
      case 3:
        EleccionEspecial();
        break;
      case 4:
        Ejecucion();
        break;
      case 5:
        Ejecucion();
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  public void verLealtad()
  {
    instructionToNoPresident();
    if (bSeeingMembership)
    {
      if (!bAlredyDrawMembership)
      {

        bAlredyDrawMembership = true;
      }
      return;
    }
    if (bAlredyInitPhase)
    {
      if (refman.bServer && !refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().connected)
      {
        OKbutton();
        return;
      }
      return;
    }
    PowerToSelectPlayer = 1;
    bool draw = true;
    if (refman.bServer && 0 != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }
    else if (!refman.bServer && refman.idConection != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }

    if (draw)
    {
      if (!bAlredyInitPhase)
      {
        //Tiene que mostrar todos los posibles jugadores a canciller
        for (int i = 0; i < refElec.g_usuarios; i++)
        {
          //Solo es color del boton
          if (i < refElec.g_usuarios && i != refElec.g_idPresident && !refman.g_Players[i].GetComponent<Jugador>().bIsDead)
          {
            g_BtnPlayers[i].SetActive(true);
            g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = refman.g_Players[i].GetComponent<Jugador>().Apodo;

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
        bAlredyInitPhase = true;
        return;
      }
    }
  }

  public void EleccionEspecial()
  {
    instructionToNoPresident();
    //HACERLAS CON SERGIO 
    if (bAlredyInitPhase)
    {
      if (refman.bServer && !refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().connected)
      {
        int selection = Random.Range(0, refElec.g_usuarios - 1);
        while (selection == refElec.g_idPresident || refman.g_Players[selection].GetComponent<Jugador>().bIsDead)
        {
          selection = Random.Range(0, refElec.g_usuarios - 1);
        }
        //ChancellorSelected(selection);
        switch (selection)
        {
          case 0:
            BtnPlayer1();
            break;
          case 1:
            BtnPlayer2();
            break;
          case 2:
            BtnPlayer3();
            break;
          case 3:
            BtnPlayer4();
            break;
          case 4:
            BtnPlayer5();
            break;
          case 5:
            BtnPlayer6();
            break;
          case 6:
            BtnPlayer7();
            break;
          case 7:
            BtnPlayer8();
            break;
          case 8:
            BtnPlayer9();
            break;
          case 9:
            BtnPlayer10();
            break;
          default:
            break;
        }
        return;
      }
      return;
    }
    PowerToSelectPlayer = 2;
    bool draw = true;
    if (refman.bServer && 0 != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }
    else if (!refman.bServer && refman.idConection != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }

    if (draw)
    {
      if (!bAlredyInitPhase)
      {
        //Tiene que mostrar todos los posibles jugadores a canciller
        for (int i = 0; i < refElec.g_usuarios; i++)
        {
          //Solo es color del boton
          if (i < refElec.g_usuarios && i != refElec.g_idPresident && !refman.g_Players[i].GetComponent<Jugador>().bIsDead)
          {
            g_BtnPlayers[i].SetActive(true);
            g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = refman.g_Players[i].GetComponent<Jugador>().Apodo;

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
        bAlredyInitPhase = true;
        return;
      }
    }
  }

  public void revisionDePolizas()
  {
    PowerToSelectPlayer = 3;
    instructionToNoPresident();
    if (refman.bServer && !refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().connected)
    {
      OKbutton();
      return;
    }

    if (refman.bServer&&!bAlredyInitPhase)
    {
      refLeg.baraja.getTop(ref cardsToPreviewTop);
      card0 = cardsToPreviewTop[0].GetComponent<CartaDePoliza>().tipoDeCarta;
      card1 = cardsToPreviewTop[1].GetComponent<CartaDePoliza>().tipoDeCarta;
      card2 = cardsToPreviewTop[2].GetComponent<CartaDePoliza>().tipoDeCarta;
    }
    if (card0==-1)
    {
      return;
    }
    if (refman.idConection == refElec.g_idPresident)
    {
      if (!bAlredyInitPhase)
      {
        if (refman.bServer)
        {
          ColorBlock ColorButton2 = cardsToPreview[0].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, cardsToPreviewTop[0].GetComponent<CartaDePoliza>().tipoDeCarta);
          cardsToPreview[0].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[0].SetActive(true);

          ColorButton2 = cardsToPreview[1].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, cardsToPreviewTop[1].GetComponent<CartaDePoliza>().tipoDeCarta);
          cardsToPreview[1].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[1].SetActive(true);

          ColorButton2 = cardsToPreview[2].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, cardsToPreviewTop[2].GetComponent<CartaDePoliza>().tipoDeCarta);
          cardsToPreview[2].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[2].SetActive(true);
          ButtonOK.SetActive(true);
          bAlredyInitPhase = true;
        }
        else
        {
          ColorBlock ColorButton2 = cardsToPreview[0].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, card0);
          cardsToPreview[0].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[0].SetActive(true);

          ColorButton2 = cardsToPreview[1].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, card1);
          cardsToPreview[1].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[1].SetActive(true);

          ColorButton2 = cardsToPreview[2].GetComponent<Button>().colors;
          colorCards(ref ColorButton2, card2);
          cardsToPreview[2].GetComponent<Button>().colors = ColorButton2;
          cardsToPreview[2].SetActive(true);
          ButtonOK.SetActive(true);
          bAlredyInitPhase = true;
        }
      }
    }
  }

  void colorCards(ref ColorBlock color, int tipecard)
  {
    if (tipecard == 1)
    {
      color.highlightedColor = Color.red;
      color.normalColor = Color.red;
      color.pressedColor = Color.red;
      color.selectedColor = Color.red;
      color.disabledColor = Color.red;
    }
    else
    {
      color.highlightedColor = Color.blue;
      color.normalColor = Color.blue;
      color.pressedColor = Color.blue;
      color.selectedColor = Color.blue;
      color.disabledColor = Color.blue;
    }
  }

  void hideRevisionDePolizas()
  {

    cardsToPreview[0].SetActive(false);
    cardsToPreview[1].SetActive(false);
    cardsToPreview[2].SetActive(false);
    ButtonOK.SetActive(false);
    bAlredyInitPhase = false;
  }

  public void hideAll()
  {
    //hideButtonOkAndTextPower();
    ButtonOK.SetActive(false);
    TextPower.SetActive(false);
    //bAlredyInitPhase = false;
    bhideButtonAndText = true;
    hideRevisionDePolizas();
    hideSeeMembershipCard();
    hidePlayersButtons();
    if (refman.bServer)
    {
      phase = 2;
      refman.refServer.bWaitingGame = true;
    }
    card0 = -1;
    card1 = -1;
    card2 = -1;
    phase = 2;
  }

  public void Ejecucion()
  {
    instructionToNoPresident();
    if (bAlredyInitPhase || bhideButtons)
    {
      if (refman.bServer && !refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().connected)
      {
        int selection = Random.Range(0, refElec.g_usuarios - 1);
        while (selection == refElec.g_idPresident || refman.g_Players[selection].GetComponent<Jugador>().bIsDead)
        {
          selection = Random.Range(0, refElec.g_usuarios - 1);
        }
        //ChancellorSelected(selection);
        switch (selection)
        {
          case 0:
            BtnPlayer1();
            break;
          case 1:
            BtnPlayer2();
            break;
          case 2:
            BtnPlayer3();
            break;
          case 3:
            BtnPlayer4();
            break;
          case 4:
            BtnPlayer5();
            break;
          case 5:
            BtnPlayer6();
            break;
          case 6:
            BtnPlayer7();
            break;
          case 7:
            BtnPlayer8();
            break;
          case 8:
            BtnPlayer9();
            break;
          case 9:
            BtnPlayer10();
            break;
          default:
            break;
        }
        return;
      }
      return;
    }
    PowerToSelectPlayer = 0;
    bool draw = true;
    if (refman.bServer && 0 != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }
    else if (!refman.bServer && refman.idConection != refElec.g_idPresident)
    {
      draw = false;
      bAlredyInitPhase = true;
    }

    if (draw)
    {
      if (!bAlredyInitPhase)
      {
        //Tiene que mostrar todos los posibles jugadores a canciller
        for (int i = 0; i < refElec.g_usuarios; i++)
        {
          //Solo es color del boton
          if (i < refElec.g_usuarios && i != refElec.g_idPresident && !refman.g_Players[i].GetComponent<Jugador>().bIsDead)
          {
            g_BtnPlayers[i].SetActive(true);
            g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = refman.g_Players[i].GetComponent<Jugador>().Apodo;

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
        bAlredyInitPhase = true;
        return;
      }
    }
  }

  void hidePlayersButtons()
  {
    for (int i = 0; i < refElec.g_usuarios; i++)
    {
      g_BtnPlayers[i].SetActive(false);
    }
  }

  void hideSeeMembershipCard()
  {
    MembershipCard.SetActive(false);
    ButtonOK.SetActive(false);
  }

  public void BtnPlayer1()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(0);
        break;
      case 1:
        CheckMembershipOfPlayer(0);
        break;
      case 2:
        especialElection(0);
        break;
    }
  }
  public void BtnPlayer2()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(1);
        break;
      case 1:
        CheckMembershipOfPlayer(1);
        break;
      case 2:
        especialElection(1);
        break;
    }
  }
  public void BtnPlayer3()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(2);
        break;
      case 1:
        CheckMembershipOfPlayer(2);
        break;
      case 2:
        especialElection(2);
        break;
    }
  }
  public void BtnPlayer4()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(3);
        break;
      case 1:
        CheckMembershipOfPlayer(3);
        break;
      case 2:
        especialElection(3);
        break;
    }
  }
  public void BtnPlayer5()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(4);
        break;
      case 1:
        CheckMembershipOfPlayer(4);
        break;
      case 2:
        especialElection(4);
        break;
    }
  }
  public void BtnPlayer6()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(5);
        break;
      case 1:
        CheckMembershipOfPlayer(5);
        break;
      case 2:
        especialElection(5);
        break;
    }
  }
  public void BtnPlayer7()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(6);
        break;
      case 1:
        CheckMembershipOfPlayer(6);
        break;
      case 2:
        especialElection(6);
        break;
    }
  }
  public void BtnPlayer8()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(7);
        break;
      case 1:
        CheckMembershipOfPlayer(7);
        break;
      case 2:
        especialElection(7);
        break;
    }
  }
  public void BtnPlayer9()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(8);
        break;
      case 1:
        CheckMembershipOfPlayer(8);
        break;
      case 2:
        especialElection(8);
        break;
    }
  }
  public void BtnPlayer10()
  {
    switch (PowerToSelectPlayer)
    {
      case 0:
        ejecuteplayer(9);
        break;
      case 1:
        CheckMembershipOfPlayer(9);
        break;
      case 2:
        especialElection(9);
        break;
    }
  }

  void ejecuteplayer(int id)
  {
    if (refman.bServer)
    {
      hideAll();
      playerEjecuted = id;
      refman.g_Players[id].GetComponent<Jugador>().bIsDead = true;
      if (2==refman.g_Players[id].GetComponent<Jugador>().Rol)
      {
        liberalWin = true;
        refman.liberalWon = true;
      }
      phase = 2;

      refman.refServer.bWaitingGame = true;
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "ejecute";
      nt.Test += id;
      refman.refCliente.SendeServer(nt);
    }
    bhideButtons = true;
  }

  void CheckMembershipOfPlayer(int id)
  {
    ColorBlock ColorButton2 = MembershipCard.GetComponent<Button>().colors;
    if (1 == refman.g_Players[id].GetComponent<Jugador>().idAfiliation)
    {
      colorCards(ref ColorButton2, 1);
      MembershipCard.GetComponent<Button>().GetComponentInChildren<Text>().text = Traslate.getTxtFascista();
    }
    else
    {
      colorCards(ref ColorButton2, 0);
      MembershipCard.GetComponent<Button>().GetComponentInChildren<Text>().text = Traslate.getTxtLiberal();
    }

    bSeeingMembership = true;
    MembershipCard.GetComponent<Button>().colors = ColorButton2;
    hidePlayersButtons();
    MembershipCard.SetActive(true);
    ButtonOK.SetActive(true);

  }

  void especialElection(int id)
  {
    hideAll();
    if (refman.bServer)
    {
      refElec.specialSelection = true;
      refElec.g_specialIDPresident = id;
      refElec.g_savedidChancellor =    refElec.g_idChancellor;
      refElec.g_savedidOldChancellor = refElec.g_idOldChancellor;
      refElec.g_savedidOldPresident =  refElec.g_idOldPresident;
      refElec.g_savedidPresident =     refElec.g_idPresident;

      refElec.g_idChancellor = -1;
      refElec.g_idOldChancellor = -1;
      refElec.g_idOldPresident = -1;
      refElec.g_idPresident=id;

      refman.refServer.bWaitingGame = true;
     
    }
    else
    {
      Net_MessageTest nt = new Net_MessageTest();
      nt.Test = "special";
      nt.Test += id;
      refman.refCliente.SendeServer(nt);
    }
    phase = 2;
  }

  void instructionToNoPresident()
  {
    if (refman.idConection == refElec.g_idPresident)
    {
      return;
    }
    refRonda.txtIntructionFase.SetActive(true);
    refRonda.txtIntructionFase.GetComponentInChildren<Text>().text = Traslate.getTxtTakingCardsP1();
    refRonda.txtIntructionFase.GetComponentInChildren<Text>().text +=
      refman.g_Players[refElec.g_idPresident].GetComponent<Jugador>().Apodo;
    refRonda.txtIntructionFase.GetComponentInChildren<Text>().text += Traslate.getTxtWaitingForPowers();
  }
  void HideInstructionToNoPresident()
  {
    refRonda.txtIntructionFase.SetActive(false);
  }
}
