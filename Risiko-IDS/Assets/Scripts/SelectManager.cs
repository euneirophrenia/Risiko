using System.Collections.Generic;
public class SelectManager : IManager
{
    private StatoController stateTemp;
    private Giocatore currentPlayer;

    public delegate void statoSelect(StatoController stato1, StatoController stato2);       
    public event statoSelect EndSelection;                                      //evento a cui si registrano Attack/Move Manager 
                                                                                //per sapere quando la selezione è finita con successo 
    public SelectManager()
    {
        this.stateTemp = null;
        PhaseManager phase = (PhaseManager) MainManager.GetManagerInstance("PhaseManager");
        this.currentPlayer = phase.CurrentPlayer;
    }

    public void Register(string moveOrAttack)
    {
        // in base a chi lo ha chiamato registra su tutti i "Clicked" degli StatoController uno dei due metodi sotto
        StatoController.Action selectMethod = this.SelectMethod(moveOrAttack);

        if (selectMethod == null)
            return;

        MainManager main = MainManager.GetInstance();
        IEnumerable<StatoController> states = main.States;

        foreach (StatoController s in states)
        {
            s.Clicked += selectMethod;
        }
    }

    public void UnRegister(string moveOrAttack)
    {
        // in base a chi lo ha chiamato DEregistra su tutti i "Clicked" degli StatoController uno dei due metodi sotto
        StatoController.Action selectMethod = this.SelectMethod(moveOrAttack);

        if (selectMethod == null)
            return;

        MainManager main = MainManager.GetInstance();
        IEnumerable<StatoController> states = main.States;

        foreach (StatoController s in states)
        {
            s.Clicked -= selectMethod;
        }
    }

    private StatoController.Action SelectMethod(string moveOrAttack)
    {
         
        switch (moveOrAttack)
        {
            case "AttackManager":
            {
                return this.SelectionAttack;
            }

            case "MoveManager":
            {
                return this.SelectionMove;
            }

            default :
            {
                return null;
            }
        }
    }



    //ATTACK E MOVE MANAGER SI DOVRANNO OCCUPARE, DOPO AVER SPAWNATO LA BOX DI SELEZIONE E FINITA LA LORO BUSINESS LOGIC, DI TOGLIERE IL TOGGLE
    //AI DUE STATI INTERESSATI






    /// <summary>
    /// Metodo sollevato dall'evento Clicked di StatoController (generato con OnMouseDown)
    /// Gestione della selezione grafica dello StatoController e logica pura di selezione
    /// </summary>
    /// <param name="stato"></param>
    private void SelectionMove(StatoController stato)
    {
        if (this.stateTemp == null && stato.Player.Equals(this.currentPlayer))
        {
            this.stateTemp = stato;
            this.stateTemp.Toggle(true);
        }
        else
        {
            BorderManager border = (BorderManager) MainManager.GetManagerInstance("BorderManager");
            
            if(border.areNeighbours(this.stateTemp, stato) && this.stateTemp.Player.Equals(stato.Player))
            {
                stato.Toggle(true);
                EndSelection(this.stateTemp, stato);
	            this.stateTemp = null;
            }
            else if (stato.Player.Equals(currentPlayer))
            {
                this.stateTemp.Toggle(false);
                this.stateTemp = stato;
                this.stateTemp.Toggle(true);
            }
        }

    }

    /// <summary>
    /// Metodo sollevato dall'evento Clicked di StatoController (generato con OnMouseDown)
    /// Gestione della selezione grafica dello StatoController e logica pura di selezione
    /// </summary>
    /// <param name="stato"></param>
    private void SelectionAttack(StatoController stato)
    {
        if (this.stateTemp == null && stato.Player.Equals(this.currentPlayer))
        {
            this.stateTemp = stato;
            this.stateTemp.Toggle(true);
        }
        else
        {
            BorderManager border = (BorderManager)MainManager.GetManagerInstance("BorderManager");

            if (border.areNeighbours(this.stateTemp, stato) && !this.stateTemp.Player.Equals(stato.Player))
            {
                stato.Toggle(true);
                EndSelection(this.stateTemp, stato);
                this.stateTemp = null;
            }
            else if (stato.Player.Equals(currentPlayer))
            {
                this.stateTemp.Toggle(false);
                this.stateTemp = stato;
                this.stateTemp.Toggle(true);
            }
        }
    }
}