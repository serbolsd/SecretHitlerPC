using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMode : MonoBehaviour
{
  private void Start()
  {
    if (FindObjectOfType<Server>())
    {
      FindObjectOfType<Server>().Shutdown();
      Destroy(FindObjectOfType<Server>().gameObject);
    }
    if (FindObjectOfType<Client>())
    {
      FindObjectOfType<Client>().Shutdown();
      Destroy(FindObjectOfType<Client>().gameObject);
    }
  }
  public void goServer()
  {
    SceneManager.LoadScene("Server");
  }
  public void goClient()
  {
    SceneManager.LoadScene("Client");
  }

  public void back()
  {
    SceneManager.LoadScene("example_text");
  }
}
