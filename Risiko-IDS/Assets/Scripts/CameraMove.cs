using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour 
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
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.forward;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.back;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			this.gameObject.GetComponent<Transform>().position-=moveSpeedAmplifier*Vector3.left;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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
