using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class AttackManager : IPhase
{
    private StatoController _statoAttacco, _statoDifesa;
        
    // Ci si dovrebbe evitare un po' di lavoro così...
    private readonly Transform _guiCanvas ;
    private readonly GameObject _choicePopup ;
    private readonly GameObject _diceResultPopup ;
    private readonly GameObject _gameWinPopup;

	public delegate void State(StatoController s);
	public static event State Conquered;

	private static AttackManager _instance=null;

	public static AttackManager GetInstance()
	{
		if (_instance==null)
			_instance=new AttackManager();
		return _instance;
	}

    private AttackManager()
    {
        _guiCanvas = GameObject.Find("MainScene/GUI").GetComponent<Transform>();
        _choicePopup = Resources.Load<GameObject>("ChoicePopup");
        _diceResultPopup = Resources.Load<GameObject>("DiceResultPopup");
        _gameWinPopup = Resources.Load<GameObject>("GameWinPopup");
        GoalReachedManager.GetInstance().GoalReached += showWinDialog;
    }



    #region IPhase
    public void Register()
    {
        SelectManager.EndSelection += handleSelection; 
        SelectManager.Register(); 
    }

    public void Unregister()
    {
        SelectManager.UnRegister();  
		SelectManager.EndSelection -= handleSelection; 
    }

    public string PhaseName
    {
        get
        {
            return "Attacco";
        }
    }

    #endregion


    private void handleSelection(StatoController statoAttacco, StatoController statoDifesa)
    {
        this._statoAttacco = statoAttacco;
        this._statoDifesa = statoDifesa;

        int numArmatiMax = _statoAttacco.TankNumber > 3 ? 3 : _statoAttacco.TankNumber - 1 ; // Math.Min(statoAttacco.TankNumber, 3);
        if (numArmatiMax == 0)
        {
            this.removeSelection();
            return;
        }
            
        List<int> valuesRange = Enumerable.Range(1, numArmatiMax).ToList<int>();

        GameObject popup = this.myIstantiatePopup(_choicePopup);
        popup.GetComponent<ChoicePopupController>().initPopup("ATTACCO", "Scegli con quanti carri armati attaccare", valuesRange);
        popup.GetComponent<ChoicePopupController>().AcceptPressed += handleChoicePopupAccepted;
        popup.GetComponent<ChoicePopupController>().CancelPressed += handleChoicePopupCancelled;
    }

    private void handleChoicePopupCancelled()
    {
        removeSelection();
    }

    private void handleChoicePopupAccepted(int n)
    {
        //non c'è bisogno di de-registrarsi dal popup
        DiceManager diceManager = DiceManager.GetInstance();
            
        diceManager.ResultReady += handleDiceResult;
        diceManager.Roll(n, _statoDifesa.TankNumber > n ? n : _statoDifesa.TankNumber); //Math.Min(n, _statoDifesa.TankNumber)
    }

    private void handleDiceResult( int[] attack, int[] defense)
    {
        bool statoConquistato = calculateResult(attack, defense);

        DiceManager diceManager = DiceManager.GetInstance();
        diceManager.ResultReady -= handleDiceResult;
		if (MainManager.GetInstance().GUIEnabled)
		{
	        GameObject popup = this.myIstantiatePopup(_diceResultPopup);
	        popup.GetComponent<DiceResultPopupController>().ClosePopup += end;
	        popup.GetComponent<DiceResultPopupController>().initPopup("Risultato lancio dadi", attack, defense, statoConquistato);
		}
    }

    private bool calculateResult(int[] attack, int[] defense)
    {
        // si occupa di gestire il risultato numerico del lancio dei dadi
        // e dell'eventuale conquista di territori 
        // ritorna un bool che indica se lo stato in difesa e' stato conquistato
        bool territorioConquistato = false;
        int attackWins = 0, defenseWins = 0;
        for (int i = 0; i < defense.Length; i++ )
        {
            if( attack[i] <= defense[i])
            {
                defenseWins += 1;
            }
            else
            {
                attackWins += 1;
            }
        }

        _statoAttacco.TankNumber -= defenseWins; 
        _statoDifesa.TankNumber -= attackWins;
        if( _statoDifesa.TankNumber == 0 )
        {
            //territorio conquistato!
            territorioConquistato = true;
                
            _statoDifesa.Player = _statoAttacco.Player;

            _statoDifesa.TankNumber = attack.Length - defenseWins;
            _statoAttacco.TankNumber -= attack.Length - defenseWins;
			if (Conquered!=null)
				Conquered(_statoDifesa);
        }
              
        return territorioConquistato; 
    }
       
    private void end()
    {
        removeSelection();
    }

    private void removeSelection()
    {
        _statoDifesa.Toggle(false);
        _statoAttacco.Toggle(false);
        _statoAttacco = null;
        _statoDifesa = null;
        MainManager.GetInstance().StateClickEnabled = true;
    }



    private void showWinDialog(IEnumerable<Giocatore> giocatori)
    {
        GameObject popup = this.myIstantiatePopup(_gameWinPopup);
		MainManager.GetInstance().StateClickEnabled=false;
		//TODO sulla falsa riga, si potrebbe mettere uno switch per disabilitare tutto il resto dell'interazione grafica.
        string descr;
        if( giocatori.Count<Giocatore>() > 1)
        {
            descr = String.Format("Complimenti {0}, avete vinto!", String.Join(" ", (from g in giocatori select g.Name).ToArray() ));
        }
        else
        {
            descr = String.Format("Complimenti {0}, hai vinto!", giocatori.ToList()[0].Name);
        }
        popup.GetComponent<GameWinPopupController>().initPopup("VITTORIA!",descr);
    }


    private GameObject myIstantiatePopup(GameObject popup)
    {
        MainManager.GetInstance().StateClickEnabled = false;
        GameObject popupIstance = GameObject.Instantiate(popup);
        popupIstance.GetComponent<Transform>().parent = _guiCanvas;
        popupIstance.GetComponent<Transform>().position = _guiCanvas.position;
        return popupIstance;
    }
}

