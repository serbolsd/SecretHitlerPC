using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionEjecutiva : MonoBehaviour
{
  public int gamePowersForNumPlayer = -1;//0= 5,6 ; 1= 7,8 ; 2= 9,10
  public int cartasFascistasColocadas = 0;
  public bool waitingNextTurn = false;
  public bool poderActivo = false;
  // Start is called before the first frame update
  void Start()
  {

  }

  void Update()
  {

  }

  public void onUpdate()
  {

  }

  void de5a6Jugadores( int playerId)
  {
    switch (cartasFascistasColocadas)
    {
      case 1:
        //nada
        break;
      case 2:
        //nada
        break;
      case 3:
        revisionDePolizas(playerId);
        break;
      case 4:
        Ejecucion(playerId);
        break;
      case 5:
        Ejecucion(playerId);
        break;
      case 6:
        //victoria fascista
        break;
      default:
        break;
    }
  }

  void de7a8Jugadores(int playerId)
  {
    switch (cartasFascistasColocadas)
    {
      case 1:
        //nada
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

  void de9a10Jugadores(int playerId)
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

  public void verLealtad(int playerId)
  {

  }

  public void EleccionEspecial(int playerId)
  {
    //HACERLAS CON SERGIO 
  }

  public void revisionDePolizas(int playerId)
  {
    //HACERLAS CON SERGIO 
  }

  public void Ejecucion(int playerId)
  {

  }

  public void BtnPlayer1()
  {
    switch (gamePowersForNumPlayer)
    {
      case 0:
        de5a6Jugadores(0);
        break;
      case 1:
        de7a8Jugadores(0);
        break;
      case 2:
        de9a10Jugadores(0);
        break;
      default:
        break;
    }
  }
  public void BtnPlayer2()
  {

  }
  public void BtnPlayer3()
  {

  }
  public void BtnPlayer4()
  {

  }
  public void BtnPlayer5()
  {

  }
  public void BtnPlayer6()
  {

  }
  public void BtnPlayer7()
  {

  }
  public void BtnPlayer8()
  {

  }
  public void BtnPlayer9()
  {

  }
  public void BtnPlayer10()
  {

  }
}
