using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reglas : MonoBehaviour
{
    int numPlayers;
  SessionEjecutiva ejecutiva;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  void onStart() {
    ejecutiva = FindObjectOfType<SessionEjecutiva>();
    if (numPlayers < 5 || numPlayers > 10)
    {
      //No se puede iniciar la partida
    }
    if (numPlayers > 8)//9/10 players
    {
      ejecutiva.gamePowersForNumPlayer = 2;
    }
    else if (numPlayers > 6)//7/8 players
    {
      ejecutiva.gamePowersForNumPlayer = 1;
    }
    else//5/6 players
    {
      ejecutiva.gamePowersForNumPlayer = 0;
    }

  }
}
