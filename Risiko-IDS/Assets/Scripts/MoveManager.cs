using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class MoveManager : IManager, IRegistration
    {
        private StatoController _statoFrom, _statoTo;

        // Ci si dovrebbe evitare un po' di lavoro così...
        private static readonly Transform _guiCanvas = GameObject.Find("GUI").GetComponent<Transform>();
        private static readonly GameObject _choicePopup = Resources.Load<GameObject>("ChoicePopup");

        #region IRegistration
        public void Register()
        {
            ((SelectManager)MainManager.GetManagerInstance("SelectManager")).Register("MoveManager");
            ((SelectManager)MainManager.GetManagerInstance("SelectManager")).EndSelection += handleSelection;
        }

        public void Unregister()
        {
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

            this.myIstantiatePopup(_choicePopup);
            _choicePopup.GetComponent<ChoicePopupController>().initPopup("MUOVI(TI)", "Scegli quanti carri armati spostare", valuesRange);
            _choicePopup.GetComponent<ChoicePopupController>().AcceptPressed += handleChoicePopupAccepted;
            _choicePopup.GetComponent<ChoicePopupController>().CancelPressed += handleChoicePopupCancelled;

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
            //se è possibile soltato spostare una volta le armate qui bisogna avanzare di fase....
        }


        private void removeSelection()
        {
            _statoFrom.Toggle(false);
            _statoTo.Toggle(false);
            _statoFrom = null;
            _statoTo = null;
        }

        private void myIstantiatePopup(GameObject popup)
        {
            GameObject popupIstance = GameObject.Instantiate(popup); 
            popupIstance.GetComponent<Transform>().parent = _guiCanvas;

        }
    }
}
