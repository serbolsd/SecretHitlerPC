using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarajaDePolizas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[]baraja;
    public GameObject cartaPreFab;
    GameObject []desechadas;
    GameObject []colocadas;
    GameObject []disponibles;

    const int numCartas = 17;
    const int numCartasFas = 11;
    const int numCartasLib = 6;
    void Start()
    {
        onStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onStart()
    {
        baraja = new GameObject[numCartas];
        for (int i = 0; i < numCartas; i++)
        {
            
            baraja[i] = Instantiate(cartaPreFab,this.transform);
            if (i<numCartasFas)
            {
                baraja[i].GetComponent<CartaDePoliza>().tipoDeCarta = 1;
            }
            else
            {
                baraja[i].GetComponent<CartaDePoliza>().tipoDeCarta = 0;
            }
        }
    }

    void barajear()
    {

    }
}
