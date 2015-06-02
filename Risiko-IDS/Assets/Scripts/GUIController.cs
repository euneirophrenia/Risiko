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
    public GameObject nameLabel, armateLabel, phaseLabel,secretGoalText;

    //GameObject-panelli per formazione GUI
    public GameObject cardPanel, textPanel, nextButton, resetButton;


    public delegate void ButtonClicked();
    public event ButtonClicked resetClicked;
    public event ButtonClicked nextClicked;

    private GameObject chagePhasePopup;
    private Giocatore currentPlayer;
    
	// Use this for initialization
	void Start ()
    {
        #region testing
        Giocatore = new Giocatore("MarzadureX",color,new ConquistaN(666), 666);
        Phase = "PhaseDiDebug, Pirla";
        #endregion

        this.chagePhasePopup = Resources.Load<GameObject>("GenericPopup");           
        this.currentPlayer = ((PhaseManager)MainManager.GetManagerInstance("PhaseManager")).CurrentPlayer;

        ((PhaseManager) MainManager.GetManagerInstance("PhaseManager")).phaseChanged += onPhaseChanged;
        ((PhaseManager) MainManager.GetManagerInstance("PhaseManager")).turnChanged += onTurnChanged;
        scalePanels();
        Refresh();
    }
	
    private void onPhaseChanged(string phase)
    {
        //this.Phase = phase; 
        this.Refresh();
    }

    private void onTurnChanged(Giocatore g)
    {
        //this.Giocatore = g;
        this.Refresh();
    }

    public void Refresh()
    {
        Giocatore = ((PhaseManager)MainManager.GetManagerInstance("PhaseManager")).CurrentPlayer;
        Phase = ((PhaseManager)MainManager.GetManagerInstance("PhaseManager")).CurrentPhaseName;
        this.enabledReset = this.resetClicked != null;
    }

    private Giocatore Giocatore
    {
        set
        {
            if ( value != null )
            {
                borders.GetComponent<Image>().color = value.Color;
                labels.GetComponent<Image>().color = value.Color;
                nameLabel.GetComponent<Text>().text = value.Name;
                armateLabel.GetComponent<Text>().text = value.ArmateDaAssegnare.ToString();
                secretGoalText.GetComponent<Text>().text = value.Goal.ToString();
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
        scale(0.13f, 0.09f, "TopRight", this.nextButton, marginX:-0.02f, marginY:-0.01f);
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

    private bool enabledReset
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
        this.Refresh();
    }

    public void OnNextClicked()
    {
        Debug.Log("NextClicked");
        if( this.nextClicked != null )
        {
            this.nextClicked();
            this.ChangePhaseAlert();
        }
    }

    public void ChangePhaseAlert()
    {
        
        StartCoroutine(this.Spawn());
    }

    private IEnumerator Spawn()
    {
        Transform trans = this.GetComponent<Transform>();
        GameObject ob = GameObject.Instantiate(this.chagePhasePopup);

        ob.GetComponent<Transform>().parent = trans.transform;
        ob.GetComponent<Transform>().position = trans.transform.position;

        PhaseManager man = ((PhaseManager) MainManager.GetManagerInstance("PhaseManager"));

        if (!this.currentPlayer.Equals(man.CurrentPlayer))
        {
            ob.GetComponent<GenericPopupController>().initPopup("Cambio turno", "Turno del giocatore " + man.CurrentPlayer.Name);
            this.currentPlayer = man.CurrentPlayer;
        }
        else
        {
            ob.GetComponent<GenericPopupController>().initPopup("Cambio fase", man.CurrentPhaseName);
        }
        
        yield return new WaitForSeconds(1);
        Destroy(ob);

    }
}
