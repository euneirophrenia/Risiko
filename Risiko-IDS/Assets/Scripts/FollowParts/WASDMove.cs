using UnityEngine;
using System.Collections;

public class WASDMove : MonoBehaviour 
{

	
	public float moveSpeedAmplifier=3;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Userei fixedupdate perché ho letto che usare update per spostare/muovere oggetti può causare instabilità. 
	// Update è chiamato il doppio più frequentemente di fixedupdate, fixed update sembra un po' "laggy" per questo scopo
	void Update () 
	{
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			this.transform.position-=moveSpeedAmplifier*Vector3.forward;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			this.transform.position-=moveSpeedAmplifier*Vector3.back;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			this.transform.position-=moveSpeedAmplifier*Vector3.left;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			this.transform.position-=moveSpeedAmplifier*Vector3.right;
		}
	
	}
	

}
