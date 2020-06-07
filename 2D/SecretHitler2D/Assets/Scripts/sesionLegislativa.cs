using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sesionLegislativa : MonoBehaviour
{
    int etapa=0;
    public GameObject buttonTakeCard;
    public GameObject buttonPresidentCard1;
    public GameObject buttonPresidentCard2;
    public GameObject buttonPresidentCard3;
    public GameObject buttonCansillerCard1;
    public GameObject buttonCansillerCard2;
    public BarajaDePolizas baraja;
    bool faseAlready = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (etapa)
        {
            case 0:
                takeCardsFase();
                break;
            case 1:
                PresidentSelection();
                break;
            case 2:
                CansillerSelection();
                break;
            default:
                break;
        }
    }

    public void takeCardsFase()
    {
        if(!faseAlready)
        {
            buttonTakeCard.SetActive(true);
            faseAlready = true;
        }
    }
    public void takeCards()
    {
        etapa++;
        faseAlready = false;
        baraja.takeCards();
        buttonTakeCard.SetActive(false);
    }
    public void selectCard1()
    {
        baraja.selectCard1();
        EndSelecction();
    }

    public void selectCard2()
    {
        baraja.selectCard2();
        EndSelecction();
    }

    public void selectCard3()
    {
        baraja.selectCard3();
        EndSelecction();
    }

    void PresidentSelection()
    {
        if (!faseAlready)
        {
            ColorBlock ColorButton1 = buttonPresidentCard1.GetComponent<Button>().colors;
            colorButton(ref ColorButton1, 0);
            buttonPresidentCard1.GetComponent<Button>().colors = ColorButton1;
            buttonPresidentCard1.SetActive(true);

            ColorBlock ColorButton2 = buttonPresidentCard2.GetComponent<Button>().colors;
            colorButton(ref ColorButton2, 1);
            buttonPresidentCard2.GetComponent<Button>().colors = ColorButton2;
            buttonPresidentCard2.SetActive(true);

            ColorBlock ColorButton3 = buttonPresidentCard3.GetComponent<Button>().colors;
            colorButton(ref ColorButton3, 2);
            buttonPresidentCard3.GetComponent<Button>().colors = ColorButton3;
            buttonPresidentCard3.SetActive(true);
            faseAlready = true;
        }
    }
    void PresidentEndSelection()
    {
        buttonPresidentCard1.SetActive(false);
        buttonPresidentCard2.SetActive(false);
        buttonPresidentCard3.SetActive(false);
        faseAlready = false;
    }

    void CansillerSelection()
    {
        if(!faseAlready)
        {
            ColorBlock ColorButton1 = buttonCansillerCard1.GetComponent<Button>().colors;
            colorButton(ref ColorButton1, 0);
            buttonCansillerCard1.GetComponent<Button>().colors = ColorButton1;
            buttonCansillerCard1.SetActive(true);

            ColorBlock ColorButton2 = buttonCansillerCard2.GetComponent<Button>().colors;
            colorButton(ref ColorButton2, 1);
            buttonCansillerCard2.GetComponent<Button>().colors = ColorButton2;
            buttonCansillerCard2.SetActive(true);
            faseAlready = true;
        }
    }
    void CansillerEndSelection()
    {
        buttonCansillerCard1.SetActive(false);
        buttonCansillerCard2.SetActive(false);
        faseAlready = false;
    }

    void EndSelecction()
    {
        if (etapa==1)
        {
            PresidentEndSelection();
        }
        else
        {
            CansillerEndSelection();
        }
        etapa++;
        if (etapa > 2)
            etapa = 0;
    }

    void colorButton(ref ColorBlock color,int indexCard)
    {
        if (baraja.enMano[indexCard].GetComponent<CartaDePoliza>().tipoDeCarta == 1)
        {
            color.highlightedColor = Color.red;
            color.normalColor = Color.red;
            color.pressedColor = Color.red;
        }
        else
        {
            color.highlightedColor = Color.blue;
            color.normalColor = Color.blue;
            color.pressedColor = Color.blue;
        }
    }
}
