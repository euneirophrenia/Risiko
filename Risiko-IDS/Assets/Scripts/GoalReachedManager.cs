using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class GoalReachedManager
{
	private readonly List<Type> _tipi;
	private readonly List<SecretGoal> _obiettivi;

	private static GoalReachedManager _instance=null;

	public delegate void GetWinner(IEnumerable<Giocatore> g);
	public event GetWinner GoalReached;
	
	private GoalReachedManager()
	{
		_tipi=  (	from tipo in this.GetType().Assembly.GetTypes() 
					where typeof(SecretGoal).IsAssignableFrom(tipo) && !tipo.IsInterface
		         		  && tipo.GetConstructor(Type.EmptyTypes)!=null
					select tipo
		         ).ToList();

		_obiettivi= new List<SecretGoal>();
		AttackManager.Conquered+=HandleConquered;
	}


	public static GoalReachedManager GetInstance()
	{
		if (_instance==null)
			_instance=new GoalReachedManager();
		return _instance;
	}

    public SecretGoal GenerateGoal()
    {
		if (_tipi.Count<1)
			throw new TypeLoadException("Non sono state trovate classi che implementino SecretGoal. " +
				"Assicurarsi che siano in questo stesso assembly.");
		int _next;
		SecretGoal g;
		_next=UnityEngine.Random.Range(0, _tipi.Count);
		bool unique=_tipi[_next].GetCustomAttributes(typeof(Unique), false).Length>0;
		do
		{	
			g=(SecretGoal)Activator.CreateInstance(_tipi[_next]);
		}
		while (unique && _obiettivi.Contains(g));
		_obiettivi.Add(g);
		return g;
    }

	private void HandleConquered(StatoController s)
	{
		Check ();
	}

	private void Check()
	{
		bool gameover=false;
		List<Giocatore> winners=new List<Giocatore>();
		for (int i=0; i< _obiettivi.Count && !gameover; i++)
		{
			gameover|=_obiettivi[i].GoalReached();
			if (_obiettivi[i].GoalReached())
				winners.Add(_obiettivi[i].Player);
		}

		if (gameover && GoalReached != null)
		{
			MainManager.GetInstance().GUIEnabled=false;
			GoalReached(winners);
		}

	}

}
