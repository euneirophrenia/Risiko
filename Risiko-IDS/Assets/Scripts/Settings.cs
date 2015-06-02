using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

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
	private static readonly int[] _players= {2,3,6};
	private static readonly Color[] _colors = { Color.red, Color.blue, Color.cyan, Color.green, Color.yellow, Color.black};
	private static readonly int _minStati=24, _maxStati=27;
	private static readonly int _statiPerArmata=3;
    private static readonly Dictionary<string, int> _armatePerContinente = new Dictionary<string, int>()
    {
        {"North_America", 4},
        {"South_America", 3},
        {"Asia", 7},
        {"Africa", 5},
        {"Oceania", 2},
        {"Europa", 4}
    };

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

	public static int MinGoalStatesNumber
	{
		get
		{
			return _minStati;
		}
	}

	public static int MaxGoalStatesNumber
	{
		get
		{
			return _maxStati;
		}
	}

    public static int ArmatePerContinente(string continent)
    {
        
            return _armatePerContinente[continent];
    }

	public static int StatiPerArmataBonus
	{
		get
		{
			return _statiPerArmata;
		}
	}
	
}
