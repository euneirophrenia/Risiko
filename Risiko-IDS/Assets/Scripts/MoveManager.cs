using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class MoveManager : IManager, IPhase
{
    private StatoController _statoFrom, _statoTo;

    // Ci si dovrebbe evitare un po' di lavoro così...
    private readonly Transform _guiCanvas;
    private readonly GameObject _choicePopup ;

    public MoveManager()
    {
        _guiCanvas = GameObject.Find("MainScene/GUI").GetComponent<Transform>();
        _choicePopup = Resources.Load<GameObject>("ChoicePopup");
    }


    #region IPhase
    public void Register()
    {
        ((SelectManager)MainManager.GetManagerInstance("SelectManager")).EndSelection += handleSelection;
        ((SelectManager)MainManager.GetManagerInstance("SelectManager")).Register("MoveManager");
    }

    public void Unregister()
    {
        ((SelectManager)MainManager.GetManagerInstance("SelectManager")).EndSelection -= handleSelection;
        ((SelectManager)MainManager.GetManagerInstance("SelectManager")).UnRegister("MoveManager");
    }

    public string PhaseName
    {
        get
        {
            return "Sposta Armate";
        }
    }

    #endregion


    private void handleSelection(StatoController statoFrom, StatoController statoTo)
    {
        this._statoFrom = statoFrom;
        this._statoTo = statoTo;

        List<int> valuesRange = Enumerable.Range(1, _statoFrom.TankNumber - 1).ToList();

        if (statoFrom.TankNumber == 1)
        {
            this.removeSelection();
            return;
        }

        GameObject popup = this.myIstantiatePopup(_choicePopup);
        popup.GetComponent<ChoicePopupController>().initPopup("MUOVI(TI)", "Scegli quanti carri armati spostare", valuesRange);
        popup.GetComponent<ChoicePopupController>().AcceptPressed += handleChoicePopupAccepted;
        popup.GetComponent<ChoicePopupController>().CancelPressed += handleChoicePopupCancelled;

    }

    private void handleChoicePopupCancelled()
    {
        this.removeSelection();
    }

    private void handleChoicePopupAccepted(int value)
    {
        _statoFrom.TankNumber -= value;
        _statoTo.TankNumber += value;
        this.removeSelection();
        //se è possibile soltato spostare una volta le armate qui bisogna avanzare di fase -> mi unregistro
        this.Unregister(); 
    }


    private void removeSelection()
    {
        _statoFrom.Toggle(false);
        _statoTo.Toggle(false);
        _statoFrom = null;
        _statoTo = null;
        MainManager.GetInstance().StateClickEnabled = true;
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
