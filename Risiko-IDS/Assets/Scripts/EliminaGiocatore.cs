﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EliminaGiocatore : SecretGoal
{
	private string _target;
	private Giocatore _player;
	
	[ConstructorArgumentsInfo("player", IsUnique=true)]
	public EliminaGiocatore(object t)
	{
		_target=(string)t;
	}
	
	
	public override string ToString ()
	{
		return string.Format ("Elimina "+ _target+ " dalla faccia della terra.");
	}
	
	public bool GoalReached()
	{
		return (new List<StatoController>(MainManager.GetInstance().GetStatesByPlayer(_target)).Count==0);
	}
	
	public Giocatore Player
	{
		get
		{
			return _player;
		}
		set
		{
			if (value.Name == _target)
			{
				GoalReachedManager gm = (GoalReachedManager)MainManager.GetManagerInstance("GoalReachedManager");
				gm.RebindPlayer(ref _target);
			}
			_player=value;
		}
	}	

	public override bool Equals (object obj)
	{
		if (!(obj is EliminaGiocatore))
			return false;
		return ((EliminaGiocatore) obj)._target.Equals(this._target);
	}

	public override int GetHashCode ()
	{
		return this._target.GetHashCode();
	}
}