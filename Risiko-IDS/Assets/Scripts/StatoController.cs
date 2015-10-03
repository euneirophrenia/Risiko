using UnityEngine;
using System.Collections;

public class StatoController : MonoBehaviour 
{
	public GUISkin GameSkin;
    public GameObject tank;

    private GameObject _animationObject;

    //variabili da visualizzare
	private string _stateName;
    private int _tankNumber = 0;
    private GameObject _tk;
    private string _stringToDisplay;
    private Giocatore _player = null;
   

	private Color _startColor;
	private bool _displayObjectName;
    private bool _toggled = false;

    public delegate void Action(StatoController stato);
    public event Action Clicked;
    

    //Funzioni UNITY
	void Start()
	{
        _stateName = this.gameObject.name;
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
            if (!this._toggled)
                this.Toggle(true, false);

            _displayObjectName = true;
        }
	}

	void OnMouseExit()
	{
        if (MainManager.GetInstance().StateClickEnabled && !this._toggled)
        {
            this.Toggle(false, false);
        }

        _displayObjectName = false;
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
            return this._player;
        }

        set
        {
            this._player = value;
            if (this._tk == null)
            {
                _tk = Instantiate(tank);
                Material myNewMaterial = new Material(Shader.Find("Diffuse"));
                _tk.GetComponentInChildren<Renderer>().material = myNewMaterial;
                _tk.GetComponent<Transform>().position = new Vector3(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y + 2.5f, this.gameObject.GetComponent<Transform>().position.z);
            }
            this.setTankColor(value.Color);
        }
    }

    public string NomeStato
    {
        get
        {
            return this._stateName;
        }
    }

    public int TankNumber
    {
        get
        {
            return this._tankNumber;
        }

        set                                 
        {
            this._tankNumber = value > 0 ? value : 0;
        }
    }

    public void Display()
    {
        if (_displayObjectName)
        {
            _stringToDisplay = "Name: " + _stateName + "\nTank number: " + _tankNumber;
            GUI.Box(new Rect(Event.current.mousePosition.x - 170, Event.current.mousePosition.y, 170, 40), _stringToDisplay, "BoxGUI");
            
        }  
    }

 

    private void setTankColor(Color color)
    {
        _tk.GetComponentInChildren<Renderer>().material.color = color;
    }

    /// <summary>
    /// Metodo per selezionare visivamente lo stato 
    /// </summary>
    /// <param name="selected">true per selezionare, false per deselezionare</param>
    public void Toggle(bool selected, bool permanent = true)       
    {
        if (selected)
        {
            Color col = new Color(1f - _startColor.r, 1f - _startColor.g, 1f - _startColor.b);
            GetComponent<Renderer>().material.color = col;
            if(permanent)
                this._toggled = true;
        }
        else
        {
            GetComponent<Renderer>().material.color = _startColor;
            _displayObjectName = false;
            if(permanent)
                this._toggled = false;
        }
    }

    public void PlayAnimation(string animationName)
    {
        StartCoroutine(this.Spawn(animationName));
    }

    private IEnumerator Spawn(string animationName)
    {
        Transform trans = this.GetComponent<Transform>();
        _animationObject = Resources.Load<GameObject>(animationName);
        GameObject ob = Instantiate(_animationObject);
        ob.GetComponent<Transform>().rotation = Camera.main.GetComponent<Transform>().rotation;
        ob.GetComponent<Transform>().position = new Vector3(trans.position.x, trans.position.y + 10, trans.position.z);
        //ob.GetComponent<Transform>().parent = GameObject.Find("MainScene/GUI").transform;
        ob.GetComponent<Animator>().Play(animationName);
        yield return new WaitForSeconds(1);
        Destroy(ob);
    }

}
