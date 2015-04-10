using UnityEngine;
using System.Collections;

public class WheelZoom : MonoBehaviour {

	private Vector3 original;
	private Vector3 delta;
	public float zoomspeed=5;

	// Use this for initialization
	void Start () 
	{
		this.original=this.gameObject.GetComponent<Transform>().position;
		delta=new Vector3(0,zoomspeed, 0.5f);
	}
	
	// Uso fixedupdate perché ho letto che usare update per spostare/muovere oggetti può causare instabilità. 
	// Update è chiamato il doppio più frequentemente di fixedupdate
	void FixedUpdate () 
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			this.transform.position-=delta;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			this.transform.position+=delta;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.Reset();
		}
	}

	public void Reset()
	{
		this.transform.position=this.original;
	}
}
