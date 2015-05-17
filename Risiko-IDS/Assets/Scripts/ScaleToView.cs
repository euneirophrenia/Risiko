using UnityEngine;
using System.Collections;


public class ScaleToView : MonoBehaviour 
{
	private Camera main;
	private float basex, basey;
	private Vector2 safety = new Vector2(1.5f, 1.2f); //coefficienti per sopperire ad effetti prospettici e di restringimento schermo in 16:9. 
	//Ho cercato di trovare una via analitica per calcolarli, ma non è per nulla per nulla facile.

	private Vector3 bottomleft, topleft, topright, cameracenter;

	// Use this for initialization
	void Start () 
	{

		main=Camera.main;
		float z=(main.transform.position - this.transform.position).magnitude; 

		bottomleft=main.ViewportToWorldPoint(new Vector3(0,0, z ));
		topleft=main.ViewportToWorldPoint(new Vector3(1,0, z ));
		topright=main.ViewportToWorldPoint(new Vector3(1,1, z));

		basex=safety.x*Vector3.Distance(topleft,topright) / this.transform.localScale.x;
		basey=safety.y*Vector3.Distance(bottomleft, topleft) / this.transform.localScale.z;

	}
	
	// Uso fixedupdate perché ho letto che usare update per spostare/muovere oggetti può causare instabilità. 
	// Update è chiamato il doppio più frequentemente di fixedupdate

	void FixedUpdate () 
	{

		float xsize,ysize;

		float z=(main.transform.position - this.transform.position).magnitude; 
	
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
