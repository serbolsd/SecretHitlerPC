using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMode : MonoBehaviour
{
  public void goServer()
  {
    SceneManager.LoadScene("Server");
  }
  public void goClient()
  {
    SceneManager.LoadScene("Client");
  }
}
