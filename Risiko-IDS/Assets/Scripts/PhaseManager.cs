using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PhaseManager : IManager
{
    private Giocatore currentPlayer;
    private IPhase currentPhaseManager;
    private IEnumerable<Giocatore> players;

    private int playerIndex = 0;                       //si può anche rendere casuale il giocatore iniziale
    private int phaseIndex = 0;

    public delegate void PhaseChangedDelegate(string phase);
    public delegate void TurnChangedDelegate(Giocatore giocatore);
    public event PhaseChangedDelegate phaseChanged;
    public event TurnChangedDelegate turnChanged;

    public PhaseManager()
    {
        this.players = MainManager.GetInstance().Players;
        this.currentPhaseManager = (IPhase) MainManager.GetManagerInstance(Settings.PhaseManagers.ElementAt(phaseIndex));
        this.currentPlayer = this.players.ElementAt(playerIndex);
        
        if(this.phaseChanged != null)
            this.phaseChanged(this.currentPhaseManager.PhaseName);
        
        if(this.turnChanged != null)
            this.turnChanged(this.currentPlayer);

        GameObject.Find("MainScene/GUI").GetComponent<GUIController>().nextClicked += ChangePhase;
		
		
	}

	public void Begin()
	{
		this.currentPhaseManager.Register ();
	}


    public Giocatore CurrentPlayer
    {
        get
        {
            return this.currentPlayer;
        }
    }

    //funzione chiamata dall'OnClick del bottono next o registrata nel costruttore all'evento relativo
    public void ChangePhase()
    {
        this.currentPhaseManager.Unregister();
        this.phaseIndex = (this.phaseIndex + 1) % Settings.PhaseManagers.Count();
        this.currentPhaseManager = (IPhase) MainManager.GetManagerInstance(Settings.PhaseManagers.ElementAt(phaseIndex));

        if (this.phaseIndex == 0)
        {
            this.ChangeTurn();
        }

        if (this.phaseChanged != null)
            this.phaseChanged(this.currentPhaseManager.PhaseName);

        this.currentPhaseManager.Register();
    }

    private void ChangeTurn()
    {
        this.playerIndex = (this.playerIndex + 1) % this.players.Count();
        this.currentPlayer = this.players.ElementAt(this.playerIndex);

        if (this.turnChanged != null)
            this.turnChanged(this.currentPlayer);
     
    }

    public string CurrentPhaseName
    {
        get
        {
            return this.currentPhaseManager.PhaseName;
        }
    }
}