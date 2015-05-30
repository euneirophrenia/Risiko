using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class AttackManager : IRegistration, IManager
    {
        private StatoController _statoAttacco, _statoDifesa;
        
        // Ci si dovrebbe evitare un po' di lavoro così...
        private static readonly Transform _guiCanvas = GameObject.Find("GUI").GetComponent<Transform>();
        private static readonly GameObject _choicePopup = Resources.Load<GameObject>("ChoicePopup");
        private static readonly GameObject _diceResultPopup = Resources.Load<GameObject>("DiceResultPopup");

        #region IRegistration
        public void Register()
        {
            ((SelectManager) MainManager.GetManagerInstance("SelectManager")).Register("AttackManager"); 
            ((SelectManager) MainManager.GetManagerInstance("SelectManager")).EndSelection += handleSelection; 
        }

        public void Unregister()
        {
            ((SelectManager)MainManager.GetManagerInstance("SelectManager")).UnRegister("AttackManager");  
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

            int numArmatiMax = _statoAttacco.TankNumber > 3 ? 3 : _statoAttacco.TankNumber; // Math.Min(statoAttacco.TankNumber, 3);
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
            DiceManager diceManager = ((DiceManager)MainManager.GetManagerInstance("DiceManager"));
            
            diceManager.ResultReady += handleDiceResult;
            diceManager.Roll(n, _statoDifesa.TankNumber > n ? n : _statoDifesa.TankNumber); //Math.Min(n, _statoDifesa.TankNumber)
        }

        private void handleDiceResult( int[] attack, int[] defense)
        {
            bool statoConquistato = calculateResult(attack, defense);

            DiceManager diceManager = ((DiceManager)MainManager.GetManagerInstance("DiceManager"));
            diceManager.ResultReady -= handleDiceResult;

            GameObject popup = this.myIstantiatePopup(_diceResultPopup);
            popup.GetComponent<DiceResultPopupController>().ClosePopup += end;
            popup.GetComponent<DiceResultPopupController>().initPopup("Risultato lancio dadi", attack, defense, statoConquistato);
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

            _statoAttacco.RemoveTank(defenseWins); 
            _statoDifesa.RemoveTank(attackWins);
            if( _statoDifesa.TankNumber == 0 )
            {
                //territorio conquistato!
                territorioConquistato = true;
                
                _statoDifesa.Player = _statoAttacco.Player;

                _statoDifesa.TankNumber = attack.Length - defenseWins;
                _statoAttacco.RemoveTank(attack.Length - defenseWins);
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
        }

        private GameObject myIstantiatePopup(GameObject popup)
        {
            GameObject popupIstance = GameObject.Instantiate(popup);
            popupIstance.GetComponent<Transform>().parent = _guiCanvas;
            return popupIstance;
        }
    }
}
