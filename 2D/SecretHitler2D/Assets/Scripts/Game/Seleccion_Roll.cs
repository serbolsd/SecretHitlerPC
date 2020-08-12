﻿using UnityEngine;
using static afiliaciones;

public class Seleccion_Roll : MonoBehaviour
{
  public int numPlayers = 0;
  public int[] idsRol;//0=liberal 1=fascista 2=hitler

  public void createRole()
  {
    //if (numPlayers < 5 | numPlayers > 10)
    //{
    //    return;
    //}
    if (numPlayers < 5)
    {
      numPlayers = 5;
    }
    idsRol = new int[numPlayers];
    int iterator = 0;
    idsRol[iterator++] = 2;
    if (numPlayers > 8)
    {
      while (iterator < 1 + 3)
      {
        idsRol[iterator++] = 1;
      }
    }
    else if (numPlayers > 6)
    {
      while (iterator < 1 + 2)
      {
        idsRol[iterator++] = 1;
      }
    }
    else
    {
      idsRol[iterator++] = 1;
    }
    while (iterator < numPlayers)
    {
      idsRol[iterator++] = 0;
    }
    for (int i = numPlayers - 1; i > 0; i--)
    {
      int r = Random.Range(0, i);
      int tmp = idsRol[i];
      idsRol[i] = idsRol[r];
      idsRol[r] = tmp;
    }
  }
}
