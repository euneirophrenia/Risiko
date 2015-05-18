using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainManager : MonoBehaviour 
{
    public List<StatoController> states;

    private List<Giocatore> players;
    private static MainManager instance;            //DA TESTARE

	// Use this for initialization
	void Start () 
    {
        this.players = new List<Giocatore>();
        instance = this;                            //DA TESTARE
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static IManager GetManagerInstance(string manager)
    {
        return null;
    }

    public static MainManager GetInstance()
    {
        return instance;                            //DA TESTARE
    }

    public void Init(string[] playerNames)
    {
        InitialPhaseManager init = new InitialPhaseManager();
        this.players = init.Create(playerNames, this.states); 
    }

    public List<StatoController> States
    {
        get
        {
            return this.states;
        }
    }


}
