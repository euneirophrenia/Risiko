using System.Collections.Generic;
using System;
using System.Linq;

/* Aggiungere un criterio di selezione significa:
 * -creare la classe che estenda il selectManager
 * -renderla singleton
 * -istruire la factory
 * 
 * Abbastanza tristemente, questo select manager è pensato solo per gestire input binari,
 * ossia che la selezione sia conclusa appena si hanno 2 stati validi.
 * Inoltre, per generalizzare rispetto ai tipi (StatoController e delegato) occorrerebbe aggiungere un livello di astrazione
 * che però al momento mi pare un po' eccessivo e non richiesto. 
 * Esempio: generalizzare StatoController a SelectedElement, interfaccia contenente qualche funzione per il rendering grafico della selezione
 * e l'evento "ehy m'han cliccato". 
 * */


public abstract partial class SelectManager
{
    protected StatoController _stateTemp;
    protected Giocatore _currentPlayer;

	private static Dictionary<string, SelectManager> _map=new Dictionary<string, SelectManager>();

    public delegate void statoSelect(StatoController stato1, StatoController stato2);       
    public static event statoSelect EndSelection;                                      

	private delegate void toggle(StatoController c, StatoController.Action f);
	private static toggle _register = (s, funzione)=>s.Clicked += funzione;
	private static toggle _unregister= (s, funzione)=>s.Clicked -= funzione;

	protected static void InitMap()
	{
		_map[AttackManager.GetInstance().PhaseName]=SelectAttack.GetInstance();
		_map[MoveManager.GetInstance().PhaseName]=SelectMove.GetInstance();
	}

	private static SelectManager GetSelectManager(string phase)
	{
		//Questa soluzione anzi che il case perché il case, come era prima, funzionava solo con costanti.
		//Non volevo lasciare scritte delle costanti del tipo "Attacco" per dire il nome della fase dell'attackManager.
		//Altra soluzione poteva essere una cascata di if oppure scegliere una convenzione di nomi e andare di reflection.
		//La mappa mi sembrava il compromesso migliore tra l'efficienza del case e la flessibilità degli if.
		//La pecca più grande è questo primo if che serve ad inizializzare la mappa ed evita corse critiche
		//Al momento mi sembra comunque un prezzo accettabile.
		//Aggiungere alla factory dei manager => modificare initMap istruendola con la coppia (fase,Manager)

		if (_map.Keys.Count==0)
			InitMap();
		if (_map.ContainsKey(phase))
			return _map[phase];
		return SelectDefault.GetInstance();
	}

	protected abstract bool IsAValidSecond(StatoController s2);
	protected abstract bool IsAValidFirst(StatoController s);

	protected void UnToggle()
	{
		if (_stateTemp!= null)
		{
			this._stateTemp.Toggle(false);
			this._stateTemp = null;
		}
	}
    protected void Init()
    {
		UnToggle ();
        PhaseManager phase = PhaseManager.GetInstance();
        _currentPlayer = phase.CurrentPlayer;
    }

	private static void DoRegistration(bool doit)
	{
		MainManager main = MainManager.GetInstance();
		IEnumerable<StatoController> states = main.States;
		SelectManager sm = SelectManager.GetSelectManager(PhaseManager.GetInstance().CurrentPhaseName);
		sm.Init();
		toggle t= doit? _register: _unregister;

		//Debug.Log (sm.GetType().Name+" "+doit);
		foreach (StatoController s in states)
		{
			t(s, sm.Select);
		}
	}

	public static void Register()
	{
		DoRegistration(true);
	}

    public static void UnRegister()
    {
		DoRegistration(false);
    }
	
    protected  void Select(StatoController stato)
    {
        if (this._stateTemp == null && this.IsAValidFirst(stato))
        {
            this._stateTemp = stato;
            this._stateTemp.Toggle(true);
            return;
        }
        if (_stateTemp!=null)
        {   
            if(this.IsAValidSecond(stato))
            {
                stato.Toggle(true);
                if (EndSelection != null)
                    EndSelection(this._stateTemp, stato);
                this._stateTemp = null;
            }
            else if (this.IsAValidFirst(stato))
            {
                this._stateTemp.Toggle(false);
                this._stateTemp = stato;
                this._stateTemp.Toggle(true);
            }
        }

    }
    
}