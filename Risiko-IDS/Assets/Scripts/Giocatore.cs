using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Giocatore 
{
    private readonly string name;
    private readonly Color color;
    private readonly SecretGoal goal;
    private int armateDaAssegnare;

    public Giocatore(string name, Color color, SecretGoal goal, int armateDaAssegnare)
    {
        this.name = name;
        this.color = color;
        this.goal = goal;
        this.armateDaAssegnare = armateDaAssegnare;
    }

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public Color Color
    {
        get
        {
            return this.color;
        }
    }

    public SecretGoal Goal
    {
        get
        {
            return this.goal;
        }
    }

    public int ArmateDaAssegnare
    {
        get
        {
            return this.armateDaAssegnare;
        }

        set
        {
            this.armateDaAssegnare = value;
        }
    }

}
