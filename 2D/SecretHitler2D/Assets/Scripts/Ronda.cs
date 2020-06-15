using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ronda : MonoBehaviour
{
  public sesionLegislativa m_fLegislativa;
  public Elecciones m_fElecciones;
  public afiliaciones m_afiliaciones;
  public Seleccion_Roll m_roles;
  public GameObject m_initGameButton;
  public GameObject m_reRoleButton;
  int phase=-1; //0 elecciones 1 legislativa
  bool AlreadyInitPhase = false;
    // Start is called before the first frame update
    void Start()
    {
        reRole();
    }

    // Update is called once per frame
    void Update()
    {
      switch (phase)
      {
        case 0:
          ElectionPhase();
          break;
        case 1:
          LegislativePhase();
          break;
        default:
          preGamePhase();
          break;
      }
    }

    void preGamePhase()
    {
      m_initGameButton.SetActive(true);
      m_reRoleButton.SetActive(true);
    }
   
    public void initGame()
    {
      m_initGameButton.SetActive(false);
      m_reRoleButton.SetActive(false);
      phase = 0;
      m_fElecciones.g_usuarios = m_roles.numPlayers;
      m_fElecciones.OnStart();
      m_fLegislativa.onStart();
  }
   
    public void reRole()
    {
      m_roles.createRole();
      m_afiliaciones.darAfiliacion();
   
    }
   
    void ElectionPhase()
    {
      if(!AlreadyInitPhase)
      {
        if(m_fElecciones.waitingNextTurn)
        {
          m_fElecciones.BtnNextTurn();
        }
        AlreadyInitPhase = true;
        m_fElecciones.g_phase = 0;
        m_fElecciones.waitingNextTurn = false;
      }
      m_fElecciones.onUpdate();
      if (m_fElecciones.waitingNextTurn)
      {
        AlreadyInitPhase = false;
        m_fElecciones.waitingNextTurn = true;
        phase++;
      }
    }
   
    void LegislativePhase()
    {
      if (!AlreadyInitPhase)
      {
        AlreadyInitPhase = true;
        m_fLegislativa.m_phase = 0;
        m_fLegislativa.waitingNextTurn = false;
      }
      m_fLegislativa.onUpdate();
      if (m_fLegislativa.waitingNextTurn)
      {
        AlreadyInitPhase = false;
        m_fLegislativa.waitingNextTurn = true;
        phase = 0;
      }
    }

    public void resetScene()
    {
      SceneManager.LoadScene("Scene_elecciones");
    }
}
