using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

  private void Start()
  {
    ///Primero se busca si el archivo existe localmente, usamos un nombre genérico
    ///como identificador
    if (System.IO.File.Exists("bin/PrimeraVez.txt"))
    {
      ///Buscamos y almacenamos la información del texto
      string file = System.IO.File.ReadAllText("bin/PrimeraVez.txt");

      ///El texto del archivo de formato texto lo convertimos en entero
      ///para poder sumar una nueva ejecución
      uint count = UInt16.Parse(file);
      count += 1;
      ///Y finalmente remplazar el viejo texto con el nuevo generado
      file = file.Replace(file.ToString(), count.ToString());
      System.IO.File.WriteAllText("bin/PrimeraVez.txt", file);
    }
    else
    {
      ///Generamos una variable que generará el nombre del archivo
      TextWriter newFile;
      newFile = new StreamWriter("bin/PrimeraVez.txt");

      ///Este contador nos servirá para poder iniciar
      ///las veces que se a ejecutado el código
      uint count = 1;
      ///Con está variable almacenaremos el contador
      ///para generar un mensaje de númeor a texto
      string message;
      message = count.ToString();

      ///Finalmente, pasamos el texto al nuevo archivo
      newFile.WriteLine(message);
      newFile.Close();
      showLanguageMenu();
    }
  }

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
