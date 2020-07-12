using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afiliaciones : MonoBehaviour
{
    
    public int numPlay = 0;
    int[] idRol;
    public string[] afil;
    public Seleccion_Roll SR; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void darAfiliacion()
    {
        idRol = SR.idsRol;
        numPlay = SR.numPlayers;
        afil = new string[numPlay];
        for (int i = 0; i < numPlay; i++)
        {
            switch (idRol[i])
            {
                case 0:
                    afil[i] = "Liberal";
                    break;
                default:
                    afil[i] = "Fascista";
                    break;
            }
        }
    }

}
