using UnityEngine;
using System.Collections;

public class DiceRoll : MonoBehaviour 
{
    private bool done;
    private Vector3 zero = new Vector3(0, 0, 0);
    private int number;

    void Start()
    {
        done = false;

        this.gameObject.GetComponent<Transform>().position = new Vector3(Random.Range(-50, 50), Random.Range(20, 50), Random.Range(10, 20));
        this.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 30), Random.Range(0, 30), Random.Range(0, 30)));
        this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, Random.Range(0, 5), ForceMode.Impulse);

    }

	void OnCollisionStay()
    {
        RaycastHit hit;
        Ray ray1 = new Ray(this.gameObject.transform.position, Vector3.up);

        if (this.gameObject.GetComponent<Rigidbody>().velocity == zero && !done && Physics.Raycast(ray1, out hit))
        {
            done = true;    
            //Debug.Log(System.Int32.Parse(hit.collider.name));
            this.Value = System.Int32.Parse(hit.collider.name);
        }
    }

    public int Value 
    {
        get
        {
            return this.number;
        }   

        set
        {
            this.number = value;
        } 

    }

    public bool Done
    {
        get
        {
            return this.done;
        }
    }

}
