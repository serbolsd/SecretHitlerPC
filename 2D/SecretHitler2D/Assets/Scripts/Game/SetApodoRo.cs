using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetApodoRo : MonoBehaviour
{
  public Text apodo;
  public Text rolNAliados;
  public gameManager refman;
  public Ronda refron;
  public Seleccion_Roll refrols;
  // Start is called before the first frame update
  public void onInit()
  {
    refman = FindObjectOfType<gameManager>();
    refron = FindObjectOfType<Ronda>();
    refrols = FindObjectOfType<Seleccion_Roll>();
    apodo.text = refman.g_Players[refman.idConection].GetComponent<Jugador>().Apodo;
    switch (refron.m_fEjecutiva.gamePowersForNumPlayer)
    {
      case 0:
        if (0 == refman.g_Players[refman.idConection].GetComponent<Jugador>().Rol)
        {
          //rolNAliados.text = "Rol: Liberal";
          rolNAliados.text = Traslate.getRolLiberal();
          rolNAliados.text += "\n";
        }
        else
        {
          if (1 == refman.g_Players[refman.idConection].GetComponent<Jugador>().Rol)
          {
            //rolNAliados.text = "Rol: Fascist";
            rolNAliados.text = Traslate.getRolFascista();
          }
          else
          {
            //rolNAliados.text = "Rol: Hitler";
            rolNAliados.text = Traslate.getRolHitler();
          }
          rolNAliados.text += "\n";
          rolNAliados.text += Traslate.getTextToAliad(); 
          rolNAliados.text += "\n";
          for (int i = 0; i < refrols.numPlayers; i++)
          {
            if (0 != refman.g_Players[i].GetComponent<Jugador>().Rol&& refman.idConection!=i)
            {
              rolNAliados.text += refman.g_Players[i].GetComponent<Jugador>().Apodo;
              rolNAliados.text += ".";
            }
          }
        }
        break;
      default:
        if (0 == refman.g_Players[refman.idConection].GetComponent<Jugador>().Rol)
        {
          //rolNAliados.text = "Rol: Liberal";
          rolNAliados.text = Traslate.getRolLiberal();
        }
        if (2 == refman.g_Players[refman.idConection].GetComponent<Jugador>().Rol)
        {
          //rolNAliados.text = "Rol: Hitler";
          rolNAliados.text = Traslate.getRolHitler();
        }
        else
        {
          if (1 == refman.g_Players[refman.idConection].GetComponent<Jugador>().Rol)
          {
            //rolNAliados.text = "Rol: Fascist";
            rolNAliados.text = Traslate.getRolFascista();
          }

          //rolNAliados.text += "  Aliados: ";
          rolNAliados.text += "\n";
          rolNAliados.text += Traslate.getTextToAliad();
          rolNAliados.text += "\n";
          for (int i = 0; i < refrols.numPlayers; i++)
          {
            if (0 != refman.g_Players[i].GetComponent<Jugador>().Rol && refman.idConection != i)
            {
              //if (2== refman.g_Players[i].GetComponent<Jugador>().Rol)
              //{
              // rolNAliados.text += "Hitler-";
              //}
              rolNAliados.text += refman.g_Players[i].GetComponent<Jugador>().Apodo;
              rolNAliados.text += ".\n";
            }
          }

          for (int i = 0; i < refrols.numPlayers; i++)
          {
            if (0 != refman.g_Players[i].GetComponent<Jugador>().Rol && refman.idConection != i)
            {
              if (2 == refman.g_Players[i].GetComponent<Jugador>().Rol)
              {
                rolNAliados.text += "Hitler-";
              }
              //rolNAliados.text += refman.g_Players[i].GetComponent<Jugador>().Apodo;
              //rolNAliados.text += ". ";
            }
          }
        }
        break;
    }
  }

  // Update is called once per frame
  void onUpdate()
  {

  }
}
