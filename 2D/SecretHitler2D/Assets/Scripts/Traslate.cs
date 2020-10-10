using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traslate : MonoBehaviour
{
  private const string GAME_LANG = "game_language";
  static public string getTextToAliad() {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Allys:";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Aliados:";
        break;
    }
    return str;
  }

  static public string getRolLiberal()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Rol: Liberal";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Rol: Liberal";
        break;
    }
    return str;
  }

  static public string getRolFascista()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Rol: Fascist";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Rol: Fascista";
        break;
    }
    return str;
  }

  static public string getRolHitler()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Rol: Hitler";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Rol: Hitler";
        break;
    }
    return str;
  }

  static public string getTxtFascista()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Fascist";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Fascista";
        break;
    }
    return str;
  }

  static public string getTxtLiberal()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Liberal";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Liberal";
        break;
    }
    return str;
  }

  static public string getTxtEjecute()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Execution";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Ejecución";
        break;
    }
    return str;
  }

  static public string getTxtSpecialElection()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Special Election";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Elección Especial";
        break;
    }
    return str;
  }

  static public string getTxtSeeMembership()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "See membership";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Ver Lealtad";
        break;
    }
    return str;
  }

  static public string getTxtPolicyReview()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Policy review";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Revision De Pólizas";
        break;
    }
    return str;
  }

  static public string getTxtTakeCards()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Take Cards";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Tomar Cartas";
        break;
    }
    return str;
  }

  static public string getTxtVoteFor()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Vote For ";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Vota Por ";
        break;
    }
    return str;
  }

  static public string getTxtLiberalWon()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Liberal Won";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Ganaron Los Liberales";
        break;
    }
    return str;
  }

  static public string getTxtFascistWon()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Fascist Won";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Ganaron Los Fascistas";
        break;
    }
    return str;
  }

  static public string getTxtDoVeto()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Do veto";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Realizar Veto";
        break;
    }
    return str;
  }

  static public string getTxtWaitForVeto()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Wait for answer";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Esperar respuesta";
        break;
    }
    return str;
  }

  static public string getTxtPresidentVeto()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "veto?";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "¿Realizar veto?";
        break;
    }
    return str;
  }
  
  static public string getTxtSelectAChancellor()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "choose a chancellor";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Elige un canciller";
        break;
    }
    return str;
  }

  static public string getTxDiscardCard()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Discard a card";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Descarta una carta";
        break;
    }
    return str;
  }

  static public string getTxPlaceCard()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Place a card";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Coloca una carta";
        break;
    }
    return str;
  }
  // El presidente esta eligiendo el canciller
  static public string getTxtPickAChancellorP1()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "The president ";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "El presidente ";
        break;
    }
    return str;
  }
  // El presidente esta eligiendo el canciller
  static public string getTxtPickAChancellorP2()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = " is choosing a chancellor";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = " está eligiendo un canciller";
        break;
    }
    return str;
  }

  // Esperando a que todos voten
  static public string getTxtWaitingForVote()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "Waiting for everyone to vote";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "Esperando a que todos voten";
        break;
    }
    return str;
  }

  // El presidente esta tomando las cartas
  static public string getTxtTakingCardsP1()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "The president ";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "El presidente ";
        break;
    }
    return str;
  }

  static public string getTxtTakingCardsP2()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = " taking cards";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = " está tomando cartas";
        break;
    }
    return str;
  }

  // El presidente esta descartando carta
  static public string getTxtDiscardingCards()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = " discarding a Card";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = " está descartando una carta";
        break;
    }
    return str;
  }

  // El canciller esta colocanco la carta
  static public string getTxtPlacingCardsP1()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = "The Chancellor ";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "El Canciller ";
        break;
    }
    return str;
  }

  static public string getTxtPlacingCardsP2()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = " is placing a Card";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = "  está colocando una carta";
        break;
    }
    return str;
  }

  // Esperando a que se ejecute el poder
  static public string getTxtWaitingForPowers()
  {
    string str = "";
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Polish:
        str = " ";
        break;
      case SystemLanguage.English:
        str = " to execute his power";
        break;
      case SystemLanguage.German:
        str = " ";
        break;
      case SystemLanguage.French:
        str = " ";
        break;
      case SystemLanguage.Spanish:
        str = " ejecute su poder";
        break;
    }
    return str;
  }

}
