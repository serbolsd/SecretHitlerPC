using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ronda : MonoBehaviour
{
  public sesionLegislativa m_fLegislativa;
  public Elecciones m_fElecciones;
  int phase=0; //0 elecciones 1 legislativa
  bool AlreadyInitPhase = false;
    // Start is called before the first frame update
    void Start()
    {
      m_fElecciones.OnStart();
      m_fLegislativa.onStart();
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
        break;
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
  }
}
