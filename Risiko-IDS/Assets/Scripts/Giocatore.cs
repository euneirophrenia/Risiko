using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Giocatore 
{
    private readonly string _name;
    private readonly Color _color;
    private readonly SecretGoal _goal;
    private int _armateDaAssegnare;

    public Giocatore(string name, Color color, SecretGoal goal, int armateDaAssegnare)
    {
        this._name = name;
        this._color = color;
        this._goal = goal;
        this._armateDaAssegnare = armateDaAssegnare;
    }

    public string Name
    {
        get
        {
            return this._name;
        }
    }

    public Color Color
    {
        get
        {
            return this._color;
        }
    }

    public SecretGoal Goal
    {
        get
        {
            return this._goal;
        }
    }

    public int ArmateDaAssegnare
    {
        get
        {
            return this._armateDaAssegnare;
        }

        set
        {
            this._armateDaAssegnare = value;
        }
    }

}
