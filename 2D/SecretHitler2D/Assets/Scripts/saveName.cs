using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveName : MonoBehaviour
{

  //Function to save the information
  //Assign this to a button
  public static void savename(ref string name)
  {
    //Get the information
     PlayerPrefs.SetString("SavedString", name);
    //Saves the prefs
    PlayerPrefs.Save();
    //Just for debug :D
    Debug.Log("Game data saved!");
  }

  //Function to load the information
  public static bool loadName(ref string name)
  {
    //Serch the key
    if (PlayerPrefs.HasKey("SavedString"))
    {
      //Save to our variable the previous information
      name = PlayerPrefs.GetString("SavedString");
      //Just for thebug :D
      Debug.Log("Game data loaded!");
      return true;
    }
    else { 
      //Case you don't have info saved previously
      Debug.LogError("There is no save data!");
    }
    return false;
  }
}
