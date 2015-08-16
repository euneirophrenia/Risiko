using UnityEngine;
using System.Collections;

public class DiceRoll : MonoBehaviour 
{
    private bool _done;
    private Vector3 _zero = new Vector3(0, 0, 0);
    private int _number;

	public delegate void Done(DiceRoll dice);
	public event Done ResultReady;

    void Start()
    {
        _done = false;
		float z=(Camera.main.transform.position - this.transform.position).magnitude; 
		Vector3 refer = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z));
		refer.y=0;
		this.gameObject.transform.position=refer;
        this.gameObject.GetComponent<Transform>().position+= new Vector3(Random.Range(-50, 50), Random.Range(20, 50), Random.Range(20, 30));
        this.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(20, 50), Random.Range(20, 50), Random.Range(20, 50)));
        this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, Random.Range(2, 7), ForceMode.Impulse);
    }

	void OnCollisionStay()
    {
        RaycastHit hit;
        Ray ray1 = new Ray(this.gameObject.transform.position, Vector3.up);

        if (this.gameObject.GetComponent<Rigidbody>().velocity == _zero && !_done && Physics.Raycast(ray1, out hit))
        {
            //Debug.Log(System.Int32.Parse(hit.collider.name));
            this._number = System.Int32.Parse(hit.collider.name);
			_done=true;
			ResultReady(this);
        }
    }

    public int Value 
    {
        get
        {
            return this._number;
        }   
    }

}
