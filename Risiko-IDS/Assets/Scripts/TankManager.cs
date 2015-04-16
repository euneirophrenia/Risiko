using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TankManager : MonoBehaviour 
{
    public List<GameObject> continents;
    public GameObject tank;

    private List<Transform> nations = new List<Transform>();
    private List<Transform> app = new List<Transform>();
    private List<GameObject> tanks = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
	    foreach (GameObject obj in continents)
        {
            obj.GetComponentsInChildren<Transform>(app);
            app.RemoveAt(0);
            nations = nations.Concat(app).ToList<Transform>();
        }

        foreach (Transform obj in nations)
        {
            //Debug.Log("" + obj.name);
            GameObject tk = Instantiate(tank);
            Material myNewMaterial = new Material(Shader.Find("Diffuse"));
            myNewMaterial.color = Color.red;
            tk.GetComponentInChildren<Renderer>().material = myNewMaterial;
            tk.GetComponent<Transform>().position = new Vector3(obj.position.x, obj.position.y + 2.5f, obj.position.z);
            tanks.Add(tk);
        }
       
        
	}
	
}
