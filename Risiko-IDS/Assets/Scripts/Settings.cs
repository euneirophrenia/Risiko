using System.Collections;
using System;
using UnityEngine;

public static class Settings
{
	/* Classe statica che espone i parametri di configurazione.
	 * ogni costante dovrebbe essere sempre riferita da qui 
	 per una futura facilità di modifica.
	 Aggiungete a piacimento tutti i "numeri magici" che trovate nel codice.
	 */

	private static readonly string _borderFile="./Assets/Scripts/confini.txt";
	private static readonly int _armateInizialiPerStato=3;
	private static readonly string[] _phaseOrder= {"PreTurnoManager", "AttackManager", "MoveManager"};
	private static readonly int[] _players= {2,4,5};
	private static readonly Color[] _colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.yellow };

	public static string BorderFile
	{
		get
		{
			return _borderFile;
		}
	}

	public static int InitialTankBonusPerState
	{
		get
		{
			return _armateInizialiPerStato;
		}
	}

	public static string[] PhaseManagers
	{
		get
		{
			return _phaseOrder;
		}
	}

	public static int[] PlayersNumber
	{
		get
		{
			return _players;
		}
	}

	public static Color[] PlayerColors
	{
		get
		{
			return _colors;
		}
	}
	
}
