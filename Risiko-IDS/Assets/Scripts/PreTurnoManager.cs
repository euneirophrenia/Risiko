using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class PreTurnoManager : IPhase, IManager
{
    private List<StatoController> currentChanges;
    private readonly GUIController _guiController;
    public PreTurnoManager ()
    {
        _guiController = GameObject.Find("MainScene/GUI").GetComponent<GUIController>();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Add(StatoController s)
    {
        PhaseManager phaseman = (PhaseManager) MainManager.GetManagerInstance("PhaseManager");
        
        if(s.Player.Equals(phaseman.CurrentPlayer) && s.Player.ArmateDaAssegnare > 0)
        {
            s.TankNumber += 1;
            s.Player.ArmateDaAssegnare--;
            this.currentChanges.Add(s);
            s.PlayAnimation("PlusOne");
            _guiController.Refresh();
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Rollback()
    {
       foreach (StatoController s in this.currentChanges)
       {
           s.TankNumber -= 1;
           s.Player.ArmateDaAssegnare++;
        }
            
       this.currentChanges = new List<StatoController>();
    }

    //Metodo fatto se vogliamo implementare anche il remove manuale e non solo con Reset completo
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Remove(StatoController s)
    {
        PhaseManager phaseman = (PhaseManager)MainManager.GetManagerInstance("PhaseManager");

        if (this.currentChanges.Contains(s) && s.Player.Equals(phaseman.CurrentPlayer))
        {
            s.TankNumber -= 1;
            s.Player.ArmateDaAssegnare++;
            this.currentChanges.Remove(s);
        }
    }

	private void GiveBonus(MainManager main, PhaseManager phase, IEnumerable<StatoController> states)
	{
		phase.CurrentPlayer.ArmateDaAssegnare+=states.Count () / Settings.StatiPerArmataBonus;
		IEnumerable<string> continents = main.Continents;
		foreach(string c in continents)
		{
			bool bonus=true;
			foreach (StatoController s in main.GetStatesByContinent(c))
			{
				bonus&=s.Player.Equals(phase.CurrentPlayer);
				if (!bonus)
					break;
			}
			if (bonus)
				phase.CurrentPlayer.ArmateDaAssegnare+=Settings.ArmatePerContinente(c);
		}
		_guiController.Refresh(); //altrimenti non mostra sempre il risultato. Potrei forse girarci attorno con un ordine diverso delle istruzioni
		//ma preferisco così
	}
	
    public void Register()
    {
        MainManager main = MainManager.GetInstance();
        PhaseManager phase = (PhaseManager) MainManager.GetManagerInstance("PhaseManager");
		this.currentChanges = new List<StatoController>(); //importante! mancava e c'era un bug

		IEnumerable<StatoController> states=main.GetStatesByPlayer(phase.CurrentPlayer);

        foreach (StatoController s in states)
        {
                s.Clicked += this.Add;
                //s.RightClicked += this.Remove;
        }

        _guiController.resetClicked += this.Rollback;
		GiveBonus(main, phase, states); //gli passo dei parametri anzi che void per ottimizzare e chiedere meno volte i vari manager
    }

    public void Unregister()
    {
        MainManager main = MainManager.GetInstance();
        PhaseManager phase = (PhaseManager)MainManager.GetManagerInstance("PhaseManager");

        foreach (StatoController s in main.GetStatesByPlayer(phase.CurrentPlayer))
        {
            s.Clicked -= this.Add;
            //s.RightClicked -= this.Remove;
        }

        _guiController.resetClicked -= this.Rollback;
    }


    public string PhaseName
    {
        get 
        {
            return "Preturno";
        }
    }
}
