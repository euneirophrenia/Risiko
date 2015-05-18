using UnityEngine;
using System.Collections;

public interface SecretGoal
{
    bool goalReached();          //DA RIVEDERE       PATTERN STRATEGY

    Giocatore Player { get; set; }
}
