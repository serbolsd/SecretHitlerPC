using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goSelectMode : MonoBehaviour
{
  private void Start()
  {
    if (FindObjectOfType<Server>())
    {
      Destroy(FindObjectOfType<Server>());
    }
    if (FindObjectOfType<Client>())
    {
      Destroy(FindObjectOfType<Client>());
    }
  }
  public void goSelectModeScene()
  {
    SceneManager.LoadScene("SelectMode");
  }
}
