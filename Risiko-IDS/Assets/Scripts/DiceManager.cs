﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiceManager
{
    private Transform _attack;
    private Transform _defense;
	private float _speedUp;
    private Transform [] _attackList;
    private Transform [] _defenseList;

	private int[] _attackRes, _defenceRes;

	private GameObject _envPrefab;
	private GameObject _env;

	private int _iAttack, _iDefence, total;

	public delegate void GetResult(int[] attackRes, int[] defenceRes);
	public event GetResult ResultReady;
	
	
	public DiceManager(Transform attack, Transform defence, GameObject environment, float speedup=2)
    {
		this._attack=attack;
		this._defense=defence;
		this._envPrefab=environment;
		this._speedUp=speedup;
	}

	private void GetAttackDice(DiceRoll dice)
	{
		this._attackRes[_iAttack]=dice.Value;
		_iAttack++;	
		if (_iAttack + _iDefence == total)
			Ready();
	}
	private void GetDefenceDice(DiceRoll dice)
	{
		this._defenceRes[_iDefence]=dice.Value;
		_iDefence++;
		if (_iAttack + _iDefence == total)
			Ready();
	}

	private void Ready()
	{
		System.Array.Sort<int>(_attackRes, (n,m) => m.CompareTo(n));
		System.Array.Sort<int>(_defenceRes, (n,m) => m.CompareTo(n));
		this.ResultReady(this._attackRes, this._defenceRes);
		Reset ();

	}

	public void Roll(int attack, int defence)
	{
		if (attack<=0 || defence <=0 )
			return;

		this._attackList = new Transform[attack];
		this._defenseList = new Transform[defence];
		_iAttack=0;
		_iDefence=0;
		total=attack+defence;

		this._attackRes = new int[attack];
		this._defenceRes = new int[defence];

		/* settaggi per il lancio */
		Time.timeScale=_speedUp;
		Camera.main.GetComponent<CameraMove>().enabled=false;
		_env = GameObject.Instantiate<GameObject>(_envPrefab);


		for (int i = 0; i < attack; i++)
		{
			this._attackList[i] = GameObject.Instantiate(_attack);
			this._attackList[i].GetComponent<DiceRoll>().ResultReady+=this.GetAttackDice;
		}
		for (int i=0; i< defence; i++)
		{
			this._defenseList[i] = GameObject.Instantiate(_defense);
			this._defenseList[i].GetComponent<DiceRoll>().ResultReady+=this.GetDefenceDice;
		}      	
    }

	private void Reset()
	{
		/* ripristino delle configurazioni normali */
		Time.timeScale=1;
		for (int i =0; i<this._attackList.Length; i++)
			GameObject.Destroy(this._attackList[i].gameObject);
		
		for (int i=0; i<this._defenseList.Length; i++)
			GameObject.Destroy (this._defenseList[i].gameObject);

		GameObject.Destroy(_env);
		Camera.main.GetComponent<CameraMove>().enabled=true;
	}
    
}
