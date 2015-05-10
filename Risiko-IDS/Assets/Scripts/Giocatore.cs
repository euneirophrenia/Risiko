using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Giocatore 
{
    private string name;
    private Color color;
    private readonly List<StatoController> states;
    private SecretGoal goal;

    public Giocatore(string name, Color color, SecretGoal goal)
    {
        this.name = name;
        this.color = color;
        this.goal = goal;
        this.states = new List<StatoController>();
    }

    public Giocatore(string name, Color color, SecretGoal goal, List<StatoController> states)
        : this(name, color, goal)
    {
        this.states = states;
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

    public List<StatoController> States
    {
        get
        {
            return this.states;
        }
    }

}
