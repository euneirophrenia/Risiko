using System.Collections.Generic;
public class SelectManager : IManager
{
    private StatoController stateTemp;

    public delegate void statoSelect(StatoController stato1, StatoController stato2);       
    public event statoSelect EndSelection;                                      //evento a cui si registrano Attack/Move Manager 
                                                                                //per sapere quando la selezione è finita con successo 
    public SelectManager()
    {
        this.stateTemp = null;
    }

    public void Register(string moveOrAttack)
    {
        // in base a chi lo ha chiamato registra su tutti i "Clicked" degli StatoController uno dei due metodi sotto
        StatoController.Action selectMethod = null;

        if (moveOrAttack.Equals("AttackManager"))
            selectMethod = this.SelectionAttack;

        else if (moveOrAttack.Equals("MoveManager"))
            selectMethod = this.SelectionMove;
        else
            return;

        MainManager main = MainManager.GetInstance();
        List<StatoController> states = main.States;

        foreach (StatoController s in states)
        {
            s.Clicked += selectMethod;
        }
    }

    public void UnRegister(string moveOrAttack)
    {
        // in base a chi lo ha chiamato DEregistra su tutti i "Clicked" degli StatoController uno dei due metodi sotto
        StatoController.Action selectMethod = null;

        if (moveOrAttack.Equals("AttackManager"))
            selectMethod = this.SelectionAttack;

        else if (moveOrAttack.Equals("MoveManager"))
            selectMethod = this.SelectionMove;
        else
            return;

        MainManager main = MainManager.GetInstance();
        List<StatoController> states = main.States;

        foreach (StatoController s in states)
        {
            s.Clicked -= selectMethod;
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
        if (this.stateTemp == null)
        {
            this.stateTemp = stato;
            this.stateTemp.Toggle(true);
        }
        else
        {
            BorderManager border = (BorderManager) MainManager.GetManagerInstance("BorderManager");
            
            if(border.areNeighbours(this.stateTemp, stato) && this.stateTemp.Player.Equals(stato.Player))
            {
                EndSelection(this.stateTemp, stato);
	this.stateTemp = null;
            }
            else
            {
                this.stateTemp.Toggle(false);
                this.stateTemp = stato;
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
        if (this.stateTemp == null)
        {
            this.stateTemp = stato;
            this.stateTemp.Toggle(true);
        }
        else
        {
            BorderManager border = (BorderManager) MainManager.GetManagerInstance("BorderManager");

            if (border.areNeighbours(this.stateTemp, stato) && !this.stateTemp.Player.Equals(stato.Player))
            {
                EndSelection(this.stateTemp, stato);
	this.stateTemp = null;
            }
            else
            {
                this.stateTemp.Toggle(false);
                this.stateTemp = stato;
            }
        }
    }
}