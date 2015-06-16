using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Unique]
public class EliminaGiocatore : SecretGoal
{
	private string _target;
	private Giocatore _player;

	#region target generator
	private class TargetGenerator
	{
		private static TargetGenerator _instance=null;
		private readonly List<string> _names;

		private TargetGenerator()
		{
			List<string> original = MainManager.GetInstance().PlayerNames.ToList();
			_names=new List<string>();
			while (original.Count >0)
			{
				int n=UnityEngine.Random.Range(0, original.Count);
				_names.Add(original[n]);
				original.RemoveAt(n);
			}
		}

		public string TargetOf(string name)
		{
			return _names[(_names.IndexOf(name)+1)%_names.Count];
		}

		public static TargetGenerator GetInstance()
		{
			if (_instance==null)
				 _instance=new TargetGenerator();
			return _instance;
		}
	}
	#endregion

	public EliminaGiocatore()
	{
		_target=null;
	}

	public EliminaGiocatore(string t)
	{
		_target=t;
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
			_player=value;
			_target=TargetGenerator.GetInstance().TargetOf(_player.Name);
			/*late binding del target in modo da evitare a priori in un colpo solo:
			-deadlock
			-obiettivi EliminaGiocatore duplicati
			-che un giocatore debba eliminare sé stesso
			*/

		}
	}	

	public override bool Equals (object obj)
	{
		if (!(obj is EliminaGiocatore))
			return false;
		return ((EliminaGiocatore) obj)._target == this._target;
	}

	public override int GetHashCode ()
	{
		return this._target.GetHashCode();
	}
}