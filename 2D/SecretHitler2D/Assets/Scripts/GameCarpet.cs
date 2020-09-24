using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCarpet : MonoBehaviour
{
  public GameObject folderObject;
  public GameObject buttonToOpen;
  public void openFolder() {
    folderObject.SetActive(true);
    buttonToOpen.SetActive(false);
  }

  public void closeFolder() {
    folderObject.SetActive(false);
    buttonToOpen.SetActive(true);
  }
}
