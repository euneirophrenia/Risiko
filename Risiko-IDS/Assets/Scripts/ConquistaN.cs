using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConquistaN : SecretGoal
{
	private readonly int _n;
	private Giocatore _player;
	
	public ConquistaN(int n)
	{
		this._n=n;
	}

	public ConquistaN()
	{
		this._n=UnityEngine.Random.Range(Settings.MinGoalStatesNumber, Settings.MaxGoalStatesNumber);
	}

	public override string ToString ()
	{
		return "Conquista "+ _n+ " Territori ";
	}

	public bool GoalReached()
	{
		return (MainManager.GetInstance().GetStatesByPlayer(_player)).Count()>=_n;
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

	public override bool Equals(object other)
	{
		if (!(other is ConquistaN))
			return false;
		return ((ConquistaN) other)._n==this._n;
	}

	public override int GetHashCode ()
	{
		return this._n.GetHashCode();
	}
}