using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reglas : MonoBehaviour
{
  public int numPlayers;
  SessionEjecutiva ejecutiva;
  public Image tabInScene;
  public Sprite tab56;
  public Sprite tab78;
  public Sprite tab910;

  public void onStart() {
    ejecutiva = FindObjectOfType<SessionEjecutiva>();
    if (numPlayers < 5 || numPlayers > 10)
    {
      //No se puede iniciar la partida
    }
    if (numPlayers > 8)//9/10 players
    {
      tabInScene.sprite = tab910;
      ejecutiva.gamePowersForNumPlayer = 2;
    }
    else if (numPlayers > 6)//7/8 players
    {
      tabInScene.sprite = tab78;
      ejecutiva.gamePowersForNumPlayer = 1;
    }
    else//5/6 players
    {
      tabInScene.sprite = tab56;
      ejecutiva.gamePowersForNumPlayer = 0;
    }

  }
}
