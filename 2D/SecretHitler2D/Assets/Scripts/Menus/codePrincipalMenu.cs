using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

  using Honeti;
public class codePrincipalMenu : MonoBehaviour
{
  public GameObject mainMenu;
  public GameObject settingsMenu;
  public GameObject RulesMenu;
  public GameObject LanguageMenu;
  public GameObject btnBack;

  public void StartPlay()
  {
    //Cambiamos el indice del escenario para poder empezar a jugar
    SceneManager.LoadScene("SelectMode");
  }

  public void CloseGame()
  {
    //Con esto cerraremos el juego
    Application.Quit();
  }

  public void showMain()
  {
    mainMenu.SetActive(true);
    settingsMenu.SetActive(false);
    RulesMenu.SetActive(false);
    btnBack.SetActive(false);
  }

  public void showSettings()
  {
    mainMenu.SetActive(false);
    settingsMenu.SetActive(true);
    RulesMenu.SetActive(false);
    btnBack.SetActive(true);
  }

  public void showRules()
  {
    mainMenu.SetActive(false);
    settingsMenu.SetActive(false);
    RulesMenu.SetActive(true);
    btnBack.SetActive(true);
  }

  public void backMenu()
  {
    showMain();
  }

  public void showLanguageMenu()
  {
    LanguageMenu.SetActive(true);
  }

  public void backLanguageMenu()
  {
    LanguageMenu.SetActive(false);
  }

  public void btnEspañol()
  {
    FindObjectOfType<I18N>().setLanguage("ES");
    LanguageMenu.SetActive(false);
  }

  public void btnEnglish()
  {
    FindObjectOfType<I18N>().setLanguage("EN");
    LanguageMenu.SetActive(false);
  }
}
