using UnityEngine;
using System.Collections;

public class StatoController : MonoBehaviour 
{
	public GUISkin GameSkin;
    public GameObject tank;

    private GameObject animationObject;

    //variabili da visualizzare
	private string stateName;
    private int tankNumber;
    private GameObject tk;
    private string stringToDisplay;
    private Giocatore player = null;
   

	private Color startColor;
	private bool _displayObjectName;

    public delegate void Action(StatoController stato);
    public event Action Clicked;
    

    //Funzioni UNITY
	void Start()
	{
		stateName = this.gameObject.name;
        tankNumber = 0;


        //gestione colore tank

        tk = Instantiate(tank);
        Material myNewMaterial = new Material(Shader.Find("Diffuse"));
        myNewMaterial.color = Color.gray;
        tk.GetComponentInChildren<Renderer>().material = myNewMaterial;
        tk.GetComponent<Transform>().position = new Vector3(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y + 2.5f, this.gameObject.GetComponent<Transform>().position.z);
	}
	void OnGUI()
	{
		GUI.skin = GameSkin;
		Display();
	}

	void OnMouseEnter()
	{
        this.Toggle(true);
        _displayObjectName = true;
	}

	void OnMouseExit()
	{
        this.Toggle(false);
		_displayObjectName = false;
	}

    void OnMouseDown()
    {
        if (Clicked != null)
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
            this.setTankColor(this.player.Color);
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

        set                                 //Da valutare se può essere utile o da cancellare
        {
            this.tankNumber = value;
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

    
    public void AddTank()
    {
        this.tankNumber++;
    }

    public bool RemoveTank()
    {
        if (this.tankNumber < 2)
            return false;

        this.tankNumber--;
        return true;
    }

    public bool RemoveTank(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (!this.RemoveTank())
                return false;
        }
        return true;
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
        if(selected)
        {
            startColor = GetComponent<Renderer>().material.color;
            Color col = new Color(1f - startColor.r, 1f - startColor.g, 1f - startColor.b);
            GetComponent<Renderer>().material.color = col;
        }
        else
        {
            GetComponent<Renderer>().material.color = startColor;
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
        ob.GetComponent<Animator>().Play(animationName);
        yield return new WaitForSeconds(1);
        Destroy(ob);

    }

}
