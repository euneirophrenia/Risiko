using UnityEngine;
using System.Collections;

public interface SecretGoal
{
    bool GoalReached();          //DA RIVEDERE       PATTERN STRATEGY

    Giocatore Player { get; set; }
}
