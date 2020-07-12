using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sirve para guardar la información del jugador
//Los managers detectan que va a ser el jugador
public class Jugador : MonoBehaviour
{
    int Rol = -1;   //0=liberal 1=Facistas 2=Hitler
    public int ID = -1;
    public int Cargo = -1; //0=Nada 1=Presidente 2=Viseprecidente
    public int siONo = -1; //0 = no 1 = si -1 = espera
    public int wasSelected = 0; //0 = no, 1 = si
    public bool bIsDead = false;
  public string Apodo= "noname";
}
