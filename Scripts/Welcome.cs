using UnityEngine;
using System.Collections;

public class Welcome : MonoBehaviour
{
	public float timeToLive=5f;
	public bool finalState=false;
	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("Activate", .5f, .5f);
		
	}
	
	void Activate()
	{
		if (this.gameObject.activeSelf)
			this.gameObject.SetActive (false);
		else
			this.gameObject.SetActive (true);

		if (Time.time > timeToLive)
		{
			CancelInvoke("Activate");
			this.gameObject.SetActive(finalState);
		}
	}
	
	
}
