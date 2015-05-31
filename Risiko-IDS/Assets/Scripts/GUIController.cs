using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class GUIController : MonoBehaviour
{
    
    #region testing
    public Color color;
    #endregion
    
    public GameObject borders;
    public GameObject labels;
    public GameObject nameLabel, armateLabel, phaseLabel;
    public GameObject cardPanel, textPanel, nextButton, resetButton;
    public GameObject secretGoalText;
    private Giocatore _giocatore;
    private string _phase;

    public delegate void ButtonClicked();
    public event ButtonClicked resetClicked;
    public event ButtonClicked nextClicked;

	// Use this for initialization
	void Start ()
    {
        #region testing
        Giocatore = new Giocatore("MarzadureX",color,new EliminaGiocatore("VittoPirla"), 666);
        Phase = "PhaseDiDebug, Pirla";
        #endregion
        //((PhaseManager) MainManager.GetManagerInstance("PhaseManager")).phaseChanged += onPhaseChanged;
        //((PhaseManager) MainManager.GetManagerInstance("PhaseManager")).turnChanged += onTurnChanged;
        scalePanels();
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
                armateLabel.GetComponent<Text>().text = _giocatore.ArmateDaAssegnare.ToString();
                secretGoalText.GetComponent<Text>().text = _giocatore.Goal.ToString();
            }
        }
    }

    private string Phase
    {
        set
        {
            if( value != null)
            {
                phaseLabel.GetComponent<Text>().text = value;
            }
        }
    }

    public void scalePanels()
    {
        scale(0.16f, 0.38f, "BottomRight", this.cardPanel);
        scale(0.12f, 0.19f, "BottomLeft", this.textPanel, marginX: 0.09f);
        scale(0.16f, 0.10f, "TopRight", this.nextButton);
        scale(0.07f, 0.06f, "BottomLeft", this.resetButton, marginX: 0.21f, marginY:0.02f);    
            
            
    }
   
    private void scale(float scaleX, float scaleY, string align, GameObject panel, float marginX = 0f, float marginY = 0f)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 panelSize = new Vector2((float)(screenSize.x * scaleX), (float)(screenSize.y * scaleY));

        int xAlign, yAlign;
        switch (align)
        {
            case "TopLeft":
                xAlign = -1;
                yAlign = 1;
                break;
            case "TopRight":
                xAlign = 1;
                yAlign = 1;
                break;
            case "BottomLeft":
                xAlign = -1;
                yAlign = -1;
                break;
            case "BottomRight":
                xAlign = 1;
                yAlign = -1;
                break;
            default:
                throw new ArgumentException("Align non valido");
        }
        panel.GetComponent<RectTransform>().sizeDelta = panelSize;
        panel.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2 (
            xAlign*(float)(screenSize.x / 2.0 - panelSize.x / 2) + screenSize.x * marginX,
            yAlign*(float)(screenSize.y / 2.0 - panelSize.y / 2) + screenSize.y * marginY
            );
    }

    public bool enabledReset
    {
        set
        {
            this.resetButton.gameObject.SetActive(value);
        }

        get
        {
            return this.resetButton.gameObject.activeSelf;
        }
    }

    public void OnResetClicked()
    {
        Debug.Log("ResetClicked");
        if( this.resetClicked != null)
        {
            this.resetClicked();
        }
    }

    public void OnNextClicked()
    {
        Debug.Log("NextClicked");
        if( this.nextClicked != null )
        {
            this.nextClicked();
        }
    }
}
