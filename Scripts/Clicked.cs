using UnityEngine;
using System.Collections;

public class Clicked : MonoBehaviour 
{
	void OnMouseDown () 
	{
		GameObject co = this.gameObject.transform.parent.gameObject;
		string parent = co.name;

		Debug.Log ("Mi chiamo "+ this.gameObject.name + " e sono nel continente " 
		           + parent );
	}
}
