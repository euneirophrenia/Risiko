using UnityEngine;
using System.Collections;


public class ScaleToView : MonoBehaviour 
{
	private Camera main;
	private float basex, basey;

	// Use this for initialization
	void Start () 
	{

		main=Camera.main;
		Vector3 bottomleft, topleft, topright;

		float z=main.transform.position.y / Mathf.Cos (Mathf.Deg2Rad*main.transform.rotation.x); 

		bottomleft=main.ViewportToWorldPoint(new Vector3(0,0, z ));
		topleft=main.ViewportToWorldPoint(new Vector3(1,0, z ));
		topright=main.ViewportToWorldPoint(new Vector3(1,1, z));

		basex=Vector3.Distance(topleft,topright) / this.transform.localScale.x;
		basey=Vector3.Distance(bottomleft, topleft) / this.transform.localScale.z;

	}
	
	// Uso fixedupdate perché ho letto che usare update per spostare/muovere oggetti può causare instabilità. 
	// Update è chiamato il doppio più frequentemente di fixedupdate

	void FixedUpdate () 
	{

		float xsize,ysize;

		float z=main.transform.position.y / Mathf.Cos (Mathf.Deg2Rad*main.transform.rotation.x); 

		Vector3 bottomleft, topleft, topright, cameracenter;
		bottomleft=main.ViewportToWorldPoint(new Vector3(0,0, z));
		topleft=main.ViewportToWorldPoint(new Vector3(1,0, z));
		topright=main.ViewportToWorldPoint(new Vector3(1,1, z));

		cameracenter=main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z));

		this.transform.position=cameracenter;

		xsize=Vector3.Distance(topleft, topright)/basex;
		ysize=Vector3.Distance(bottomleft, topleft)/basey;

		Vector3 size= new Vector3(xsize,transform.localScale.y, ysize);
		this.transform.localScale=size;

	}
	

}
