using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PhaseManager
{
    private static PhaseManager _instance = null;

    private Giocatore _currentPlayer;
    private IPhase _currentPhaseManager;
    private IEnumerable<Giocatore> _players;

    private int _playerIndex = 0;                       //si può anche rendere casuale il giocatore iniziale
    private int _phaseIndex = 0;
    private bool _preturnoPerTutti = true;

    public delegate void PhaseChangedDelegate(string phase);
    public delegate void TurnChangedDelegate(Giocatore giocatore);
    public event PhaseChangedDelegate phaseChanged;
    public event TurnChangedDelegate turnChanged;

    public static PhaseManager GetInstance()
    {
        if (_instance == null)
            _instance = new PhaseManager();
        return _instance;
    }

    private PhaseManager()
    {
        this._players = MainManager.GetInstance().Players;
        this._currentPhaseManager = (IPhase) MainManager.GetManagerInstance(Settings.PhaseManagers.ElementAt(_phaseIndex));
        this._currentPlayer = this._players.ElementAt(_playerIndex);
        
        if(this.phaseChanged != null)
            this.phaseChanged(this._currentPhaseManager.PhaseName);
        
        if(this.turnChanged != null)
            this.turnChanged(this._currentPlayer);

        GameObject.Find("MainScene/GUI").GetComponent<GUIController>().nextClicked += ChangePhase;
		
		
	}

	public void Begin()
	{
		this._currentPhaseManager.Register ();
	}


    public Giocatore CurrentPlayer
    {
        get
        {
            return this._currentPlayer;
        }
    }

    public bool PreturnoPerTutti
    {
        get
        {
            return this._preturnoPerTutti;
        }
    }

    //funzione chiamata dall'OnClick del bottone next o registrata nel costruttore all'evento relativo
    public void ChangePhase()
    {
        this._currentPhaseManager.Unregister();

        if (!this._preturnoPerTutti)
        {
            this._phaseIndex = (this._phaseIndex + 1) % Settings.PhaseManagers.Count();
            this._currentPhaseManager = (IPhase) MainManager.GetManagerInstance(Settings.PhaseManagers.ElementAt(_phaseIndex));
        }

        if (this._phaseIndex == 0 || this._preturnoPerTutti)
        {
            this.ChangeTurn();
        }

        if (this._playerIndex == 0 && this._preturnoPerTutti)
        {
            this._preturnoPerTutti = false;
        }

        if (this.phaseChanged != null)
            this.phaseChanged(this._currentPhaseManager.PhaseName);

        this._currentPhaseManager.Register();
    }

    private void ChangeTurn()
    {
        this._playerIndex = (this._playerIndex + 1) % this._players.Count();
        this._currentPlayer = this._players.ElementAt(this._playerIndex);

        if (this.turnChanged != null)
            this.turnChanged(this._currentPlayer);
    }

    public string CurrentPhaseName
    {
        get
        {
            return this._currentPhaseManager.PhaseName;
        }
    }
}