using UnityEngine;
using System.Collections;

public class DiceManager : MonoBehaviour 
{
    private bool finito;
    private bool app;
    public Transform attack;
    public Transform defense;
	public float speedUp=2;
    private Transform [] attackList;
    private Transform [] defenseList;
    //private int [] attackResult;
    //private int [] defenseResult;

	// Use this for initialization
	void Start ()
    {
        app = false;

        this.attackList = new Transform[3];
        this.defenseList = new Transform[3];
		Time.timeScale=speedUp;
        for (int i = 0; i < 3; i++)
        {
            this.attackList[i] = Instantiate(attack);
            this.defenseList[i] = Instantiate(defense);
            
        }

        
 
	}
	
	void Update()
    {
        finito = true;
        for (int i = 0; i < 3; i++)
        {
            finito &= this.attackList[i].gameObject.GetComponent<DiceRoll>().Done;
            finito &= this.defenseList[i].gameObject.GetComponent<DiceRoll>().Done;
        }

        if (finito && !app)
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(this.attackList[i].gameObject.GetComponent<DiceRoll>().Value);
                Debug.Log(this.defenseList[i].gameObject.GetComponent<DiceRoll>().Value);
            }
            app = true;
			Time.timeScale=1;
        }
    }
    
}
