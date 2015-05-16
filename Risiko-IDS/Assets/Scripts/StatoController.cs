using UnityEngine;
using System.Collections;

public class StatoController : MonoBehaviour 
{
	public GUISkin GameSkin;

    //variabili da visualizzare
	private string stateName;
    private int tankNumber;
    public GameObject tank;
    private Color tankColor;
    private string stringToDisplay;
  

	private Color startColor;
	private bool _displayObjectName;
    private GameObject tk;

    //Funzioni UNITY
	void Start()
	{
		stateName = this.gameObject.name;
        tankNumber = 0;


        //gestione colore tank

        tk = Instantiate(tank);
        Material myNewMaterial = new Material(Shader.Find("Diffuse"));
        this.tankColor = Color.gray;
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
		startColor = GetComponent<Renderer>().material.color;
		Color col = new Color (1f - startColor.r, 1f - startColor.g, 1f - startColor.b);
		GetComponent<Renderer> ().material.color = col;
		_displayObjectName = true;
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = startColor;
		_displayObjectName = false;
	}

    void OnMouseDown()
    {

    }

    //Funzioni standard

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

    public void setTankColor(Color color)
    {
        this.tankColor = color;
        tk.GetComponent<Renderer>().material.color = color;
    }
}
