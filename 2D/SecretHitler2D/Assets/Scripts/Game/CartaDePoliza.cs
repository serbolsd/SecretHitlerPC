using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaDePoliza : MonoBehaviour
{
    public int tipoDeCarta = 1; //0=liberal 1=facista
    string nombreDeCarta="";

    public void onInit()
    {
        if(tipoDeCarta==0)
        {
            nombreDeCarta = "liberta";
        }
        else
        {
            nombreDeCarta = "fasista";
        }
    }
}
