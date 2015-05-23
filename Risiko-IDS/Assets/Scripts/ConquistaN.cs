using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConquistaN : SecretGoal
{
	private readonly int _n;
	private Giocatore _player;

	[ConstructorArgumentsInfo("int", Min=24, Max=27, IsUnique=false)]
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
}