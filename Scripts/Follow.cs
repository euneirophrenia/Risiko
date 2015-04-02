using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	private Vector3 original;
	private Vector3 delta;

	public float zoomSpeed = 2.5f;
	public float moveSpeedAmplifier=1;
	// Use this for initialization
	void Start () 
	{
		original=this.gameObject.GetComponent<Transform>().position;
		delta=new Vector3(0,zoomSpeed,0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			this.gameObject.GetComponent<Transform>().position-=delta;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			this.gameObject.GetComponent<Transform>().position+=delta;
		}
		if (Input.GetAxis("Vertical")>0)
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.forward;
		}
		if (Input.GetAxis ("Vertical") < 0)
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.back;
		}
		if (Input.GetAxis ("Horizontal")<0)
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.left;
		}
		if (Input.GetAxis("Horizontal")>0)
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.right;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.Reset();
		}
	
	}

	public void Reset()
	{
		this.gameObject.GetComponent<Transform>().position=this.original;
	}

}
