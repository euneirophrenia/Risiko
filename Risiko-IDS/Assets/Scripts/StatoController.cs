using UnityEngine;
using System.Collections;

public class StatoController : MonoBehaviour 
{
	public GUISkin GameSkin;
    public GameObject tank;

    private GameObject animationObject;

    //variabili da visualizzare
	private string stateName;
    private int tankNumber = 0;
    private GameObject tk;
    private string stringToDisplay;
    private Giocatore player = null;
   

	private Color _startColor;
	private bool _displayObjectName;

    public delegate void Action(StatoController stato);
    public event Action Clicked;
    

    //Funzioni UNITY
	void Start()
	{
        stateName = this.gameObject.name;
        _startColor = GetComponent<Renderer>().material.color;
      
	}
	void OnGUI()
	{
		GUI.skin = GameSkin;
		Display();
	}

	void OnMouseEnter()
	{
        if (MainManager.GetInstance().StateClickEnabled)
        {
            this.Toggle(true);
            _displayObjectName = true;
        }
	}

	void OnMouseExit()
	{
        if (MainManager.GetInstance().StateClickEnabled)
        {
            this.Toggle(false);
		    _displayObjectName = false;
        }
	}

    void OnMouseDown()
    {
        if (Clicked != null && MainManager.GetInstance().StateClickEnabled)
            Clicked(this);
 
        _displayObjectName = false;

    }

    //Funzioni standard

    public Giocatore Player
    {
        get
        {
            return this.player;
        }

        set
        {
            this.player = value;
            if (this.tk == null)
            {
                tk = Instantiate(tank);
                Material myNewMaterial = new Material(Shader.Find("Diffuse"));
                tk.GetComponentInChildren<Renderer>().material = myNewMaterial;
                tk.GetComponent<Transform>().position = new Vector3(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y + 2.5f, this.gameObject.GetComponent<Transform>().position.z);
            }
            this.setTankColor(value.Color);
        }
    }

    public string NomeStato
    {
        get
        {
            return this.stateName;
        }
    }

    public int TankNumber
    {
        get
        {
            return this.tankNumber;
        }

        set                                 
        {
            this.tankNumber = value > 0 ? value : 0;
        }
    }

    public void Display()
    {
        if (_displayObjectName)
        {
            stringToDisplay = "Name: " + stateName + "\nTank number: " + tankNumber;
            GUI.Box(new Rect(Event.current.mousePosition.x - 170, Event.current.mousePosition.y, 170, 40), stringToDisplay, "BoxGUI");
            
        }  
    }

 

    private void setTankColor(Color color)
    {
        tk.GetComponentInChildren<Renderer>().material.color = color;
    }

    /// <summary>
    /// Metodo per selezionare visivamente lo stato 
    /// </summary>
    /// <param name="selected">true per selezionare, false per deselezionare</param>
    public void Toggle(bool selected)       
    {
        if (selected)
        {
            Color col = new Color(1f - _startColor.r, 1f - _startColor.g, 1f - _startColor.b);
            GetComponent<Renderer>().material.color = col;
        }
        else
        {
            GetComponent<Renderer>().material.color = _startColor;
        }
    }

    public void PlayAnimation(string animationName)
    {
        StartCoroutine(this.Spawn(animationName));
    }

    private IEnumerator Spawn(string animationName)
    {
        Transform trans = this.GetComponent<Transform>();
        animationObject = Resources.Load<GameObject>(animationName);
        GameObject ob = Instantiate(animationObject);
        ob.GetComponent<Transform>().rotation = Camera.main.GetComponent<Transform>().rotation;
        ob.GetComponent<Transform>().position = new Vector3(trans.position.x, trans.position.y + 10, trans.position.z);
        //ob.GetComponent<Transform>().parent = GameObject.Find("MainScene/GUI").transform;
        ob.GetComponent<Animator>().Play(animationName);
        yield return new WaitForSeconds(1);
        Destroy(ob);

    }

}
