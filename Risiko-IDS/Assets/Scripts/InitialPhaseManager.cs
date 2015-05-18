using System;
using System.Collections.Generic;
using UnityEngine;

public class InitialPhaseManager : IManager
{
    private System.Random random = new System.Random();
    private System.Random randomStates = new System.Random();

    private List<Giocatore> players = new List<Giocatore>();
    private List<int> temp = new List<int>();

    private int armatePerStato = 3;             //DA RIVEDERE
    private int armateIniziali;

    public InitialPhaseManager()
    {
       
    }

    public List<Giocatore> Create(string[] playerNames, List<StatoController> states)
    {
        this.armateIniziali = (armatePerStato-1) * states.Count / playerNames.Length;
        //Ne metto una in meno da assegnare perchè ne inserisco una direttamente per evitare inconsistenze

        for(int i=0; i<playerNames.Length; i++)
        {
            Color color =  new UnityEngine.Color(random.Next(500) < 255 ? 0 : 255, random.Next(500) < 255 ? 0 : 255 , random.Next(500) < 255 ? 0 : 255);
            GoalReachedManager goalReachedManager = (GoalReachedManager) MainManager.GetManagerInstance("GoalReachedManager");

            SecretGoal secret = goalReachedManager.GenerateGoal();
            Giocatore giocatore = new Giocatore(playerNames[i], color, secret, this.armateIniziali);
            secret.Player = giocatore;
            

            players.Add(giocatore);
        }

        int num, cont = 0, j= 0;

        while (temp.Count < states.Count)
        {
            num = randomStates.Next(states.Count);
            
            if(!temp.Contains(num))
            {
                temp.Add(num);

                states[num].AddTank();
                states[num].Player = players[j];
                cont++;

                if(cont == (states.Count/playerNames.Length))
                {
                    cont = 0;
                    j++;
                }
            }
        }

        return players;
    }
	
}
