using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elecciones : MonoBehaviour
{
    bool g_isPresident = false;   //Si el presidente ya fue elegido
    bool g_firstTurn = true;
    bool g_isChancellor = false;
    bool g_isPhase = false;

    int g_usuarios = 5;
    public int g_idPresident = 0;
    public int g_idOldPresident = -1;
    public int g_idChancellor = 0;
    public int g_idOldChancellor = -1;
    public int g_phase = 0; //0 = Elección, 1 = Votación, 2 = Espera sig turno
    int g_turnCount = 0;
    int g_noCount = 0;
    int g_iteratorPlayers = 0;

    public int[] g_listPlayerVote; //Lista donde se van a agregar los si o no

    public bool waitingNextTurn = false;

    public GameObject g_PrefabPlayer;
    public GameObject[] g_Players;
    public GameObject[] g_BtnPlayers = new GameObject[10];

    public GameObject g_currentPlayerTxt;
    public GameObject g_BtnYes;
    public GameObject g_BtnNo;
    public GameObject g_BtnNextTurn;

    //Llamamos esta función cuando queramos y siempre será para el inicio
    //Reiniciar o pruebas
    public void OnStart()
    {
        //Crear y setearle un ID  a los jugadores
        g_Players = new GameObject[g_usuarios];

        //
        for (int i = 0; i < g_usuarios; i++)
        {
            //Genera una instancia del prefab
            //Al ponerlo en la posición this lo vuelve su hijo
            g_Players[i] = Instantiate(g_PrefabPlayer, this.transform);
            g_Players[i].GetComponent<Jugador>().ID = i;
            g_Players[i].GetComponent<Jugador>().Cargo = 0;

        }
    }

    void Start()
    {
        //Inicializamos a los jugadores
        //OnStart();
    }

    // Update is called once per frame
    void Update()
    {
       //switch (g_phase)
       // {
       //
       //     case 0:
       //         PhaseElection();
       //         break;
       //
       //     case 1:
       //          PhaseVotation();
       //         break;
       //
       //     case 2:
       //           //WaitingNextTurn();
       //         break;
       //
       //     default:
       //         break;
       // }
    }

  public void onUpdate()
  {
    switch (g_phase)
    {

      case 0:
        PhaseElection();
        break;

      case 1:
        PhaseVotation();
        break;

      case 2:
        //WaitingNextTurn();
        waitingNextTurn = true;
        break;

      default:
        break;
    }
  }
  //Función de espera por turnos
  void WaitingNextTurn()
    {
        if (!g_isPhase)
        {
            g_isPhase = true;

            g_BtnNextTurn.SetActive(true);
        }

    }

    //Fase de votaciones
    void PhaseVotation()
    {
        if (!g_isPhase)
        {
            g_isPhase = true;

            //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
            g_currentPlayerTxt.SetActive(true);
            g_BtnYes.SetActive(true);
            g_BtnNo.SetActive(true);

            g_currentPlayerTxt.GetComponent<Text>().text = g_iteratorPlayers.ToString();

            g_listPlayerVote = new int[g_usuarios];

            return;
        }
        g_currentPlayerTxt.GetComponent<Text>().text = g_iteratorPlayers.ToString();

        if (g_iteratorPlayers >= g_usuarios)
        {
            CountingVotes();
        }
    }

    //Función para poder contar los votos
    void CountingVotes()
    {
        int yes = 0;
        int no = 0;

        for (int i = 0; i < g_usuarios; i++)
        {
             if (g_listPlayerVote[i] == 0)
             {
                 no++;
             }
             else
             {
                 yes++;
             }
        }

        //Checamos para ver si el presidente debe cambiar por votación
        if (no > yes)
        {
            NextPresident();
        }
        else
        {
            g_phase++;
            g_idOldPresident = g_idPresident;
            g_idOldChancellor = g_idChancellor;
            //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
            g_currentPlayerTxt.SetActive(false);
            g_BtnYes.SetActive(false);
            g_BtnNo.SetActive(false);

            g_iteratorPlayers = 0;

            g_currentPlayerTxt.GetComponent<Text>().text = g_iteratorPlayers.ToString();

            g_isPhase = false;
        }

    }

    //Función para la selección de otro presidente
    void NextPresident()
    {
        g_phase=0;
        g_isChancellor = false;
        g_isPhase = false;

        g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 0;
        g_idPresident++;

        if (g_idPresident >= g_usuarios)
        {
            g_idPresident = 0;
        }

        g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 1;

        g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 0;

        //Hacemos visible los botones de selección e indicaremos quien es el que está escogiendo
        g_currentPlayerTxt.SetActive(false);
        g_BtnYes.SetActive(false);
        g_BtnNo.SetActive(false);

        g_iteratorPlayers = 0;

        g_currentPlayerTxt.GetComponent<Text>().text = g_iteratorPlayers.ToString();

        g_noCount++;

        if (g_noCount  > 3)
        {
            g_noCount = 0;
        }
    }

    //Fase de las elecciones
    void PhaseElection()
    {
        //Si el presidente ya fue elegido
        if (g_isPresident)
        {
            SelectingChancellor();
        }

        //En caso de iniciar la nueva ronda del juego
        if (g_firstTurn)
        {
            g_firstTurn = false;

            //Tenemos que mostrar todos los posibles jugadores para ser presidente
            int numRandom = 0;
            numRandom = Random.Range(0, g_usuarios - 1);

            //Se define aleatoriamente al presidente
            g_Players[numRandom].GetComponent<Jugador>().Cargo = 1;
            g_isPresident = true;
        }
        //Tenemos que rotar la presidencia a la izquierda del último jugador seleccionado
        else
        {

        }
    }

    //Función para la selección del cansiller
    void SelectingChancellor()
    {
        if (!g_isPhase)
        {
            //Tiene que mostrar todos los posibles jugadores a canciller
            for (int i = 0; i < 10; i++)
            {
                //Solo es color del boton
                if (i < g_usuarios && i!=g_idPresident && i!=g_idOldPresident&& i!=g_idOldChancellor)
                {
                    g_BtnPlayers[i].SetActive(true);
                    g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = i.ToString();

                    ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

                    ColorButton.highlightedColor = Color.blue;
                    ColorButton.normalColor = Color.white;
                    ColorButton.pressedColor = Color.gray;

                    g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
                }
                else
                {
                    ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

                    ColorButton.highlightedColor = Color.blue;
                    ColorButton.normalColor = Color.black;
                    ColorButton.pressedColor = Color.gray;

                    g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
                }
            }

            g_isPhase = true;
            return;
        }

        //El presidente selecciona el canciller
        if (g_isChancellor)
        {
            g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 2;

            for (int i = 0; i < 10; i++)
            {
                //Solo es color del boton
                if (i < g_usuarios)
                {
                    g_BtnPlayers[i].SetActive(false);
                    g_BtnPlayers[i].GetComponent<Button>().GetComponentInChildren<Text>().text = i.ToString();

                    ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

                    ColorButton.highlightedColor = Color.blue;
                    ColorButton.normalColor = Color.white;
                    ColorButton.pressedColor = Color.gray;

                    g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
                }
                else
                {
                    ColorBlock ColorButton = g_BtnPlayers[i].GetComponent<Button>().colors;

                    ColorButton.highlightedColor = Color.blue;
                    ColorButton.normalColor = Color.black;
                    ColorButton.pressedColor = Color.gray;

                    g_BtnPlayers[i].GetComponent<Button>().colors = ColorButton;
                }
            }

            //
            g_phase += 1;
            g_isPhase = false;
        }
    }

    //Botones de todos los jugadores
    public void BtnPlayer1() {

        ChancellorSelected(0);
    }
    public void BtnPlayer2() {

        ChancellorSelected(1);
    }
    public void BtnPlayer3() {

        ChancellorSelected(2);
    }
    public void BtnPlayer4() {

        ChancellorSelected(3);
    }
    public void BtnPlayer5() {

        ChancellorSelected(4);
    }
    public void BtnPlayer6() {

        ChancellorSelected(5);
    }
    public void BtnPlayer7() {

        ChancellorSelected(6);
    }
    public void BtnPlayer8() {

        ChancellorSelected(7);
    }
    public void BtnPlayer9() {

        ChancellorSelected(8);
    }
    public void BtnPlayer10(){

        ChancellorSelected(9);
    }

    //Funcion de botones extra 
    public void BtnYes()
    {
        g_listPlayerVote[g_iteratorPlayers] = 1;
        g_iteratorPlayers += 1;
    }
    public void BtnNO()
    {
        g_listPlayerVote[g_iteratorPlayers] = 0;
        g_iteratorPlayers += 1;
    }
    public void BtnNextTurn()
    {
        g_phase = 0;
        g_isPhase = false;

        g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 0;
        g_idPresident++;

        if (g_idPresident >= g_usuarios)
        {
            g_idPresident = 0;
        }

        g_Players[g_idPresident].GetComponent<Jugador>().Cargo = 1;

        g_Players[g_idChancellor].GetComponent<Jugador>().Cargo = 0;

        g_noCount = 0;
        g_isChancellor = false;

        g_BtnNextTurn.SetActive(false);

    }

    //Después de la selección del cansiller
    void ChancellorSelected(int _x)
    {
        g_idChancellor = _x;
        g_isChancellor = true;
    }
}