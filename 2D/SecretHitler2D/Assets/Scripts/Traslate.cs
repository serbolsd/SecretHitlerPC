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

  // El presidente esta eligiendo el canciller
  // Esperando a que todos voten
  // El presidente esta tomando las cartas
  // El presidente esta descartando carta
  // El canciller esta colocanco la carta
  // Esperando a que se ejecute el poder

}
