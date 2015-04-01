using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	private Vector3 original;
	private Vector3 delta;
	// Use this for initialization
	void Start () 
	{
		original=this.gameObject.GetComponent<Transform>().position;
		delta=new Vector3(0,2,0.5f);
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
		if (Input.GetKey("up"))
		{
			this.gameObject.GetComponent<Transform>().position-=Vector3.forward;
		}
		if (Input.GetKey("down"))
		{
			this.gameObject.GetComponent<Transform>().position-=Vector3.back;
		}
		if (Input.GetKey("left"))
		{
			this.gameObject.GetComponent<Transform>().position-=Vector3.left;
		}
		if (Input.GetKey("right"))
		{
			this.gameObject.GetComponent<Transform>().position-=Vector3.right;
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
