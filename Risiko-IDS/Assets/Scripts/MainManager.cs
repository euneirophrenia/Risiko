using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainManager : MonoBehaviour 
{
	private readonly Dictionary<string, List<StatoController>> _world = new Dictionary<string, List<StatoController>>();
	private static readonly Dictionary<string, IManager> _factory=new Dictionary<string,IManager>();
	public GameObject world;

    private IEnumerable<Giocatore> players;
    private static MainManager instance; 			//DA TESTARE

	private string[] _playerNames;


	// Use this for initialization
	void Start () 
    {
        this.players = new List<Giocatore>();
        instance = this; 							//DA TESTARE

		foreach(Transform continent in this.world.transform)
		{
			_world[continent.gameObject.name]=new List<StatoController>();
			foreach(Transform stato in continent)
			{
				_world[continent.gameObject.name].Add(stato.GetComponent<StatoController>());
			}
		}

	}


    public static IManager GetManagerInstance(string manager)
    {
        if (_factory.ContainsKey(manager))
			return _factory[manager];

		Type managerType = Type.GetType(manager);

		if (! (managerType is IManager))
			throw new ArgumentException("Manager ignoto");

		_factory[manager]= (IManager) Activator.CreateInstance(managerType); 
		//buono se ogni manager espone il costruttore di default
		/* una cosa molto tranquilla che potremmo fare è: ogni manager ha il costruttore di default e se ha bisogno di altre info le chiede 
		 * a qualche altro manager, come d'altronde abbiamo sempre fatto fin'ora, si può continuare così e questo codice funziona.
		 Alternativa -> il mainmanager si tiene una mappa manager-argomenti opportuni, e poi si usano quelli. Possiamo scegliere*/

		return _factory[manager];
	}

    public static MainManager GetInstance()
    {
        return instance;                            //DA TESTARE
    }

    public void Init(string[] playerNames)
    {
		this._playerNames=playerNames;
        InitialPhaseManager init = new InitialPhaseManager();
        this.players = init.Create(playerNames, new List<StatoController>(this.States)); 
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
		return from StatoController c in States where c.Player.Name.Equals(name) select c;
	}


	public IEnumerable<StatoController> GetStatesyByContinent(string name)
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


}
