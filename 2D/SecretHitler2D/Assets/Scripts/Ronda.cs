using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ronda : MonoBehaviour
{
  public sesionLegislativa m_fLegislativa;
  public Elecciones m_fElecciones;
  public SessionEjecutiva m_fEjecutiva;
  public afiliaciones m_afiliaciones;
  public Seleccion_Roll m_roles;
  public GameObject m_initGameButton;
  public GameObject m_reRoleButton;


  public GameObject[] g_BtnPlayers;

  public GameObject[] g_BtnOk;

  int phase=-1; //0 elecciones 1 legislativa
  bool AlreadyInitPhase = false;
    // Start is called before the first frame update
    void Start()
    {
      m_fEjecutiva = FindObjectOfType<SessionEjecutiva>();
      m_fLegislativa = FindObjectOfType<sesionLegislativa>();
      m_fElecciones = FindObjectOfType <Elecciones>();
     g_BtnPlayers = m_fElecciones.g_BtnPlayers;
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
          EjecutivePhase();
          break;
        case 2:
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

  void EjecutivePhase()
  {
    if (null== m_fEjecutiva||!m_fEjecutiva.poderActivo)
    {
      ++phase;
      return;
    }
    if (!AlreadyInitPhase)
    {
      if (m_fEjecutiva.waitingNextTurn)
      {
        //si se esta esperando el siguiente turno y hay algo que inicializar se hace aqui
      }
      AlreadyInitPhase = true;
      //m_fElecciones.g_phase = 0;
      m_fElecciones.waitingNextTurn = false;
    }
    m_fElecciones.onUpdate();
    if (m_fElecciones.waitingNextTurn)
    {
      AlreadyInitPhase = false;
      phase++;
    }
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

  public void btnForPlayer1()//index 0
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer1();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer1();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer2()//index 1
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer2();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer2();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer3()//index 2
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer3();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer3();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer4()//index 3
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer4();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer4();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer5()//index 4
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer5();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer5();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer6()//index 5
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer6();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer6();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer7()//index 6
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer7();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer7();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer8()//index 7 
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer8();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer8();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer9()//index 8
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer9();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer9();
        break;
      default:
        break;
    }
  }
  public void btnForPlayer10()//index 9
  {
    switch (phase)
    {
      case 0:
        m_fElecciones.BtnPlayer10();
        break;
      case 1:
        m_fEjecutiva.BtnPlayer10();
        break;
      default:
        break;
    }
  }

  public void resetScene()
    {
      SceneManager.LoadScene("Scene_elecciones");
    }
}
