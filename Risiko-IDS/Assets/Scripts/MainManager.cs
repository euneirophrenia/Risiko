using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainManager : MonoBehaviour 
{
	private readonly Dictionary<string, List<StatoController>> _world = new Dictionary<string, List<StatoController>>();
	private static readonly Dictionary<string, IPhase> _factory=new Dictionary<string,IPhase>();
	public GameObject world;

    private IEnumerable<Giocatore> players;
    private static MainManager instance; 			

	private string[] _playerNames;

    private bool _clickenable=true;


	// Use this for initialization
	void Start () 
    {
        this.players = new List<Giocatore>();
        instance = this; 							

		foreach(Transform continent in this.world.transform)
		{
			_world[continent.gameObject.name]=new List<StatoController>();
			foreach(Transform stato in continent)
			{
				_world[continent.gameObject.name].Add(stato.GetComponent<StatoController>());
			}
		}

	}


    public static IPhase GetManagerInstance(string manager)
    {
        if (_factory.ContainsKey(manager))
			return _factory[manager];

		Type managerType = Type.GetType(manager);

		if (! (typeof(IPhase).IsAssignableFrom(managerType)) || managerType.IsInterface)
			throw new ArgumentException("Manager ignoto");

		_factory[manager]= (IPhase) managerType.GetMethod("GetInstance").Invoke(null, null); 
		return _factory[manager];
	}

    public static MainManager GetInstance()
    {
        return instance;                            
    }

    public void Init(string[] playerNames)
    {
		this._playerNames=playerNames;
        InitialPhaseManager init = new InitialPhaseManager();
        this.players = init.Create(playerNames, new List<StatoController>(this.States));
       	PhaseManager.GetInstance().Begin();
    }

    public IEnumerable<StatoController> States
    {
		get
		{
			return from StatoController s in (this._world.Values.SelectMany(x => x).ToList()) select s;
        }
    }

	public IEnumerable<String> Continents
	{
		get
		{
			return this._world.Keys;
		}
	}

	public IEnumerable<StatoController> GetStatesByPlayer(Giocatore g)
	{
		return from StatoController c in States where c.Player.Equals(g) select c;
	}

	public IEnumerable<StatoController> GetStatesByPlayer(string name)
	{
		return from StatoController c in States where c.Player.Name == name select c;
	}


	public IEnumerable<StatoController> GetStatesByContinent(string name)
	{
		if (!_world.ContainsKey(name))
			throw new KeyNotFoundException("No such continent");
		return _world[name];
	}

	public IEnumerable<Giocatore> Players
	{
		get
		{
			return this.players;
		}
	}

	public IEnumerable<string> PlayerNames
	{
		get
		{
			return this._playerNames;
		}
	}

    public bool StateClickEnabled
    {
        get
        {
            return this._clickenable;
        }
        set
        {
            this._clickenable = value;
        }
    }

    public void NewGame()
    {
        _factory.Clear();
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
