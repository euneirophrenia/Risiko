using System.Collections.Generic;
using System;

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

public abstract class SelectManager
{
    protected StatoController _stateTemp;
    protected Giocatore _currentPlayer;

    public delegate void statoSelect(StatoController stato1, StatoController stato2);       
    public static event statoSelect EndSelection;                                      

	private delegate void toggle(StatoController c, StatoController.Action f);
	private static toggle _register = (s, funzione)=>s.Clicked += funzione;
	private static toggle _unregister= (s, funzione)=>s.Clicked -= funzione;

	private static SelectManager GetSelectManager(string phase)
	{
		switch (phase)
		{
			case "Attacco": 
				return SelectAttack.GetInstance(); 
			case "Sposta Armate": 
				return SelectMove.GetInstance(); 
			
			default: 
				return SelectDefault.GetInstance(); //default deny, potremmo se no lanciare eccezione
		}
	}

	protected abstract bool IsAValidSecond(StatoController s1 , StatoController s2);
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
            if(this.IsAValidSecond(_stateTemp, stato))
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