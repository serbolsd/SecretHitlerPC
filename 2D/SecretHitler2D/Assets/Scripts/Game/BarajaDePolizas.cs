using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarajaDePolizas : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject[] baraja = new GameObject[17];
  public GameObject cartaPreFab;
  public GameObject[] desechadas = new GameObject[0];
  public GameObject[] disponibles = new GameObject[17];
  public GameObject[] enMano = new GameObject[3];
  public GameObject[] colocadas = new GameObject[0];

  const int numCartas = 17;
  int numCartasDisponibles = 17;
  int numCartasDesechadas = 0;
  int numCartasFas = 11;
  int numCartasLib = 6;
  public int numCartasFasColocadas = 0;
  public int numCartasLibColocadas = 0;
  int numCartasColocadas = 0;

  bool Descartar = true;
  public bool bFascist = false;
  public bool canEjecutive = false;


  public GameObject[] liberarCards = new GameObject[5];
  public GameObject[] facistCards = new GameObject[6];
  void Start()
  {
    //onStart();
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void onStart()
  {
    baraja = new GameObject[numCartas];
    for (int i = 0; i < numCartas; i++)
    {

      baraja[i] = Instantiate(cartaPreFab, this.transform);
      if (i < numCartasFas)
      {
        baraja[i].GetComponent<CartaDePoliza>().tipoDeCarta = 1;
      }
      else
      {
        baraja[i].GetComponent<CartaDePoliza>().tipoDeCarta = 0;
      }
      baraja[i].GetComponent<CartaDePoliza>().onInit();
    }
    disponibles = baraja;
    barajear();
  }

  void barajear()
  {
    for (int i = numCartas - 1; i > 0; i--)
    {
      int r = Random.Range(0, i);
      GameObject tmp = disponibles[i];
      disponibles[i] = disponibles[r];
      disponibles[r] = tmp;
    }
  }

  public void takeCards()
  {
    enMano = new GameObject[3];
    if (numCartasDisponibles < 3)
    {
      rebarajear();
    }
    enMano[0] = disponibles[numCartasDisponibles - 1];
    numCartasDisponibles--;
    enMano[1] = disponibles[numCartasDisponibles - 1];
    numCartasDisponibles--;
    enMano[2] = disponibles[numCartasDisponibles - 1];
    numCartasDisponibles--;

    GameObject[] tmpdisponibles = new GameObject[numCartasDisponibles];
    for (int i = 0; i < numCartasDisponibles; i++)
    {
      tmpdisponibles[i] = disponibles[i];
    }
    disponibles = new GameObject[numCartasDisponibles];
    disponibles = tmpdisponibles;
  }
  public void selectCard1()
  {
    if (Descartar)
    {
      quitCardOnHand(0);
      Descartar = false;
      return;
    }
    ColoqueCard(0);
    Descartar = true;
  }
  public void selectCard2()
  {
    if (Descartar)
    {
      quitCardOnHand(1);
      Descartar = false;
      return;
    }
    ColoqueCard(1);
    Descartar = true;
  }
  public void selectCard3()
  {
    quitCardOnHand(2);
    Descartar = false;
  }
  void quitCardOnHand(int indexHand)
  {
    GameObject[] tmpDescarte;
    GameObject[] tmpOnMano = new GameObject[2];
    tmpDescarte = new GameObject[numCartasDesechadas];
    for (int i = 0; i < numCartasDesechadas; i++)
    {
      tmpDescarte[i] = desechadas[i];
    }
    numCartasDesechadas++;
    desechadas = new GameObject[numCartasDesechadas];
    for (int i = 0; i < numCartasDesechadas - 1; i++)
    {
      desechadas[i] = tmpDescarte[i];
    }
    desechadas[numCartasDesechadas - 1] = enMano[indexHand];
    switch (indexHand)
    {
      case 0:
        tmpOnMano[0] = enMano[1];
        tmpOnMano[1] = enMano[2];
        break;
      case 1:
        tmpOnMano[0] = enMano[0];
        tmpOnMano[1] = enMano[2];
        break;
      default:
        tmpOnMano[0] = enMano[0];
        tmpOnMano[1] = enMano[1];
        break;
    }
    enMano = new GameObject[2];
    enMano = tmpOnMano;
  }

  void ColoqueCard(int IndexOption)
  {
    GameObject[] tmpColocadas;

    tmpColocadas = new GameObject[numCartasColocadas];
    for (int i = 0; i < numCartasColocadas; i++)
    {
      tmpColocadas[i] = colocadas[i];
    }
    numCartasColocadas++;
    colocadas = new GameObject[numCartasColocadas];
    for (int i = 0; i < numCartasColocadas - 1; i++)
    {
      colocadas[i] = tmpColocadas[i];
    }
    switch (enMano[IndexOption].GetComponent<CartaDePoliza>().tipoDeCarta)
    {
      case 0:
        liberarCards[numCartasLibColocadas].SetActive(true);
        numCartasLibColocadas++;
        bFascist = false;
        if (numCartasFasColocadas >= 5)
        {
          FindObjectOfType<sesionLegislativa>().bCanVeto = true;
        }
        if (numCartasFasColocadas == 6)
        {
          FindObjectOfType<gameManager>().liberalWon = true;
        }
        break;
      default:
        facistCards[numCartasFasColocadas].SetActive(true);
        numCartasFasColocadas++;
        bFascist = true;
        canEjecutive = true;
        if (numCartasFasColocadas>=3)
        {
          FindObjectOfType<gameManager>().canWinFascist = true;
        }
        if (numCartasFasColocadas == 6)
        {
          FindObjectOfType<gameManager>().fascistWon = true;
        }
        break;
    }
    colocadas[numCartasColocadas - 1] = enMano[IndexOption];
    switch (IndexOption)
    {
      case 0:
        desechar(1);
        break;
      default:
        desechar(0);
        break;
    }
    enMano = new GameObject[0];
  }

  void desechar(int indexOption)
  {
    GameObject[] tmpDescarte;
    tmpDescarte = new GameObject[numCartasDesechadas];
    for (int i = 0; i < numCartasDesechadas; i++)
    {
      tmpDescarte[i] = desechadas[i];
    }
    numCartasDesechadas++;
    desechadas = new GameObject[numCartasDesechadas];
    for (int i = 0; i < numCartasDesechadas - 1; i++)
    {
      desechadas[i] = tmpDescarte[i];
    }
    desechadas[numCartasDesechadas - 1] = enMano[indexOption];
  }
  void rebarajear()
  {
    GameObject[] tmpDis;
    tmpDis = new GameObject[numCartasDisponibles];
    for (int i = 0; i < numCartasDisponibles; i++)
    {
      tmpDis[i] = disponibles[i];
    }
    disponibles = new GameObject[numCartasDisponibles + numCartasDesechadas];
    for (int i = 0; i < numCartasDisponibles; i++)
    {
      disponibles[i] = tmpDis[i];
    }
    for (int i = numCartasDisponibles; i < numCartasDesechadas + numCartasDisponibles; i++)
    {
      disponibles[i] = desechadas[i - numCartasDisponibles];
    }
    numCartasDisponibles += numCartasDesechadas;
    for (int i = numCartasDisponibles - 1; i > 0; i--)
    {
      int r = Random.Range(0, i);
      GameObject tmp = disponibles[i];
      disponibles[i] = disponibles[r];
      disponibles[r] = tmp;
    }
    numCartasDesechadas = 0;
    desechadas = new GameObject[0];
  }

  public void clientUpdateTab(int type)
  {
    switch (type)
    {
      case 0://liberal
        liberarCards[numCartasLibColocadas].SetActive(true);
        numCartasLibColocadas++;
        bFascist = false;
        break;
      default://fascita
        facistCards[numCartasFasColocadas].SetActive(true);
        numCartasFasColocadas++;
        canEjecutive = true;
        bFascist = true;
        break;
    }
  }

  public void getTop(ref GameObject[] top)
  {
    if (numCartasDisponibles < 3)
    {
      rebarajear();
    }

    top[0] = disponibles[numCartasDisponibles - 1];
    top[1] = disponibles[numCartasDisponibles - 2];
    top[2] = disponibles[numCartasDisponibles - 3];
  }

  public void addTopCard(ref int type)
  {
    if (numCartasDisponibles < 1)
    {
      rebarajear();
    }
    GameObject[] tmpColocadas;

    tmpColocadas = new GameObject[numCartasColocadas];
    for (int i = 0; i < numCartasColocadas; i++)
    {
      tmpColocadas[i] = colocadas[i];
    }
    numCartasColocadas++;
    colocadas = new GameObject[numCartasColocadas];
    for (int i = 0; i < numCartasColocadas - 1; i++)
    {
      colocadas[i] = tmpColocadas[i];
    }
    colocadas[numCartasColocadas - 1] = disponibles[numCartasDisponibles - 1];
    switch (disponibles[numCartasDisponibles - 1].GetComponent<CartaDePoliza>().tipoDeCarta)
    {
      case 0:
        liberarCards[numCartasLibColocadas].SetActive(true);
        numCartasLibColocadas++;
        type = 0;
        bFascist = false;
        break;
      default:
        facistCards[numCartasFasColocadas].SetActive(true);
        numCartasFasColocadas++;
        type = 1;
        bFascist = true;
        break;
    }

    GameObject[] tmpDisponibles;
    --numCartasDisponibles;
    tmpDisponibles = new GameObject[numCartasDisponibles];
    for (int i = 0; i < numCartasDisponibles; i++)
    {
      tmpDisponibles[i] = disponibles[i];
    }
    disponibles = new GameObject[numCartasDisponibles];
    disponibles = tmpDisponibles;
  }

  public void addTopCardClient(int type)
  {
    switch (type)
    {
      case 0:
        liberarCards[numCartasLibColocadas].SetActive(true);
        numCartasLibColocadas++;
        break;
      default:
        facistCards[numCartasFasColocadas].SetActive(true);
        numCartasFasColocadas++;
        break;
    }
  }

  public void veto()
  {
    desechar(0);
    desechar(1);
    enMano = new GameObject[0];
  }
}
