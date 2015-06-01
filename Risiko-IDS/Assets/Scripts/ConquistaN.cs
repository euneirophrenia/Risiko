﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConquistaN : SecretGoal
{
	private readonly int _n;
	private Giocatore _player;

	[ConstructorArgumentsInfo("int", Min=22, Max=22, IsUnique=false)]
	public ConquistaN(object n)
	{
		this._n=(int)n;
	}


	public override string ToString ()
	{
		return string.Format ("Conquista "+ _n+ " Territori ");
	}

	public bool GoalReached()
	{
		return (new List<StatoController>(MainManager.GetInstance().GetStatesByPlayer(_player)).Count>_n);
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