using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PreTurnoManager : IPhase, IManager
{
    private List<StatoController> currentChanges;

    public PreTurnoManager ()
    {
        this.currentChanges = new List<StatoController>();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Add(StatoController s)
    {
        PhaseManager phaseman = (PhaseManager) MainManager.GetManagerInstance("PhaseManager");
        
        if(s.Player.Equals(phaseman.CurrentPlayer) && s.Player.ArmateDaAssegnare > 0)
        {
            s.AddTank();
            s.Player.ArmateDaAssegnare--;
            this.currentChanges.Add(s);
            s.PlayAnimation("PlusOne");      
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Rollback()
    {
       foreach (StatoController s in this.currentChanges)
       {
            s.RemoveTank();
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
            s.RemoveTank();
            s.Player.ArmateDaAssegnare++;
            this.currentChanges.Remove(s);
        }
    }

    public void Register()
    {
        MainManager main = MainManager.GetInstance();
        PhaseManager phase = (PhaseManager) MainManager.GetManagerInstance("PhaseManager");

        foreach (StatoController s in main.GetStatesByPlayer(phase.CurrentPlayer))
        {
                s.Clicked += this.Add;
                //s.RightClicked += this.Remove;
        }
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
    }


    public string PhaseName
    {
        get 
        {
            return "Preturno";
        }
    }
}
