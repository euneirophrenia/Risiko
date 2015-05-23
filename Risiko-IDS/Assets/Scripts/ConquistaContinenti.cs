using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConquistaContinenti : SecretGoal
{
	private readonly string[] _targets;
	private Giocatore _player;
	
	[ConstructorArgumentsInfo("continents", Min=2, Max=2, IsUnique=true)]
	public ConquistaContinenti(object t)
	{
		this._targets=(string[])t;
	}
	
	
	public override string ToString ()
	{
		string s="Conquista ";
		foreach(string x in _targets)
			s+=x+", ";
		return string.Format (s);
	}
	
	public bool GoalReached()
	{
		bool res=true;
		MainManager m = MainManager.GetInstance();
		foreach (string c in _targets)
		{
			foreach(StatoController s in m.GetStatesyByContinent(c))
			{
				res&=s.Player.Equals(_player);
				if (!res)
					return false;
			}
		}
		return true;

	}
	
	public Giocatore Player
	{
		get
		{
			return _player;
		}
		set
		{
			_player=value;
		}
	}
}