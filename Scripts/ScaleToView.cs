using UnityEngine;
using System.Collections;

/* Questo codice produce uno scaling non matematicamente perfetto.
 * L'imperfezione discende dal modo (approssimato) in cui ho calcolato la distanza camera-piano da visualizzare.
 * Si potrebbe fare di meglio, forse, però al momento questa è l'unica soluzione che mi venga in mente indipendente dalla conoscenza della velocità di zoom.
 * Potenzialmente, nel caso di zoommate veramente importanti ci potrebbe essere qualche bug.. ma premendo "spazio" torna sempre tutto a posto
*/


public class ScaleToView : MonoBehaviour 
{
	private Vector3 original;
	private Camera main;
	private float basex, basey;

	// Use this for initialization
	void Start () 
	{
		original=this.gameObject.GetComponent<Transform>().position;

		main=Camera.main;
		Vector3 bottomleft, topleft, topright;

		float z=main.transform.position.y; //questa andrebbe calcolata meglio per risultati più precisi

		bottomleft=main.ViewportToWorldPoint(new Vector3(0,0, z ));
		topleft=main.ViewportToWorldPoint(new Vector3(1,0, z ));
		topright=main.ViewportToWorldPoint(new Vector3(1,1, z));

		basex=Vector3.Distance(topleft,topright);
		basey=Vector3.Distance(bottomleft, topleft);

	}
	
	// Uso fixedupdate perché ho letto che usare update per spostare/muovere oggetti può causare instabilità. 
	// Update è chiamato il doppio più frequentemente di fixedupdate

	void FixedUpdate () 
	{

		float xsize,ysize;

		float z=main.transform.position.y; //questa andrebbe calcolata meglio per risultati più precisi

		Vector3 bottomleft, topleft, topright;
		bottomleft=main.ViewportToWorldPoint(new Vector3(0,0, z));
		topleft=main.ViewportToWorldPoint(new Vector3(1,0, z));
		topright=main.ViewportToWorldPoint(new Vector3(1,1, z));

		xsize=Vector3.Distance(topleft, topright)/basex;
		ysize=Vector3.Distance(bottomleft, topleft)/basey;

		Vector3 size= new Vector3(xsize,transform.localScale.y, ysize);
		this.transform.localScale=size;

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
