using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    #region testing
    public Color color;
    public string name, secretGoal;
    public GameObject nameLabel, armateLabel, phaseLabel;
    #endregion

    public GameObject borders;
    public GameObject labels;
    private Giocatore _giocatore;
    private string _phase; 
    
	// Use this for initialization
	void Start ()
    {
        #region testing
        //_giocatore = new Giocatore(name,color,new SecretGoal(secretGoal));
        #endregion
        //MainManager.GetManagerInstance("PhaseManager").phaseChanged += onPhaseChanged;
        //MainManager.GetManagerInstance("PhaseManager").turnChanged += onTurnChanged;
    }
	
    private void onPhaseChanged(string phase)
    {
        this.Phase = phase; 
    }

    private void onTurnChanged(Giocatore g)
    {
        
        this.Giocatore = g;
    }

    private Giocatore Giocatore
    {
        set
        {
            if ( value != null )
            {
                this._giocatore = value;
                borders.GetComponent<Image>().color = _giocatore.Color;
                labels.GetComponent<Image>().color = _giocatore.Color;
                nameLabel.GetComponent<Text>().text = _giocatore.Name;
            }
        }
    }

    private string Phase
    {
        set
        {
            if( value != null)
            {
                this._phase = value;
            }
        }
    }
}
