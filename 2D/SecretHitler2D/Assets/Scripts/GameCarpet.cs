using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCarpet : MonoBehaviour
{
  public GameObject folderObject;
  public GameObject buttonToOpen;

  bool opening = false;
  bool clossing = false;
  bool opened = true;

  Vector3 openPos = Vector3.zero;
  Vector3 closePos =new Vector3(0,-500,0 );

  [Range(0.0f,0.5f)]
  public float timeToOpen = 0.2f;
  float timeTrans = 0.0f;


  private void Update()
  {
    if (opening || clossing)
    {
      timeTrans += Time.deltaTime;
      Vector3 currentPos = Vector3.zero;
      if (opening)
      {
        currentPos = openPos - closePos;
      }
      else
      {
        currentPos = closePos - openPos;
      }
      if (timeTrans>=timeToOpen)
      {
        timeTrans = timeToOpen;

      }
      
      float porcentaje = (timeTrans / timeToOpen);
      currentPos = currentPos * porcentaje;
      if (opening)
      {
        folderObject.transform.localPosition = closePos + currentPos;
      }
      else
      {
        folderObject.transform.localPosition = openPos + currentPos;
      }
      if (timeTrans >= timeToOpen)
      {
        if (opening)
        {
          opened = true;
          buttonToOpen.SetActive(false);
        }
        else
        {
          opened = false;
          buttonToOpen.SetActive(true);
        }
        opening = false;
        clossing = false;
      }
    }

  }

  public void openFolder() {
    if (opened || clossing|| opening)
    {
      return;
    }
    timeTrans = 0.0f;
    opening = true;
    //folderObject.SetActive(true);
    //buttonToOpen.SetActive(false);
  }

  public void closeFolder() {
    //folderObject.SetActive(false);
    //buttonToOpen.SetActive(true);
    if (!opened || clossing || opening)
    {
      return;
    }
    timeTrans = 0.0f;
    clossing = true;
  }
}
