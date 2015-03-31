using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour 
{
    public Transform attack;
    public Transform defense;

	// Use this for initialization
	void Start ()
    {
        Instantiate(attack);
        Instantiate(defense);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
