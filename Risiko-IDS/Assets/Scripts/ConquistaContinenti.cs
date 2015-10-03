using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Unique]
public class ConquistaContinenti : SecretGoal
{
	private readonly string[] _targets;
	private Giocatore _player;

	public ConquistaContinenti(string[] t)
	{
		this._targets=t;
	}

	public ConquistaContinenti()
	{
		string[] continents = MainManager.GetInstance().Continents.ToArray();
		
		List<string> res = new List<string>();
		
		int n=2; // magari recuperato dalle settings?
		for (int i=0; i<n;i++)
		{
			string c;
			do
			{
				c=continents[UnityEngine.Random.Range(0, continents.Length)];
			}
			while (res.Contains(c));
			res.Add(c);
		}
		this._targets=res.ToArray();
	}

	public override string ToString ()
	{
		string s="Conquista ";
		foreach(string x in _targets)
			s+=x+", ";
		return s.Substring(0,  s.Length-2);
	}
	
	public bool GoalReached()
	{
		bool res=true;
		MainManager m = MainManager.GetInstance();
		foreach (string c in _targets)
		{
			foreach(StatoController s in m.GetStatesByContinent(c))
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

	public override bool Equals (object obj)
	{
		if (!(obj is ConquistaContinenti))
			return false;
		ConquistaContinenti other=((ConquistaContinenti) obj);
		List<string> otherConts = new List<string>(other._targets);
		bool res=this._targets.Length==otherConts.Count;
		if (!res)
			return false;
		foreach (string c in this._targets)
		{
			res&=otherConts.Contains(c);
			if (!res)
				return false;
		}
		return true;
	}

	public override int GetHashCode ()
	{
		return this._targets.GetHashCode();
	}
}