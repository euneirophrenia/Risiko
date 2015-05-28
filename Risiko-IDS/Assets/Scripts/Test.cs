using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
    public GameObject plusOne;
    public StatoController stato;
    public GameObject camera;

    private Giocatore g;

	// Use this for initialization
	void Start () 
    {
        PhaseManager phase = ((PhaseManager)MainManager.GetManagerInstance("PhaseManager"));
        g = new Giocatore("pippo", Color.blue, null, 4);
        phase.CurrentPlayer = g;
        
        //this.PlusOne(stato);
	}

    public void PlusOne(StatoController s)
    {
        s.Player = g;
        PreTurnoManager pret = (PreTurnoManager)MainManager.GetManagerInstance("PreTurnoManager");
        pret.Add(stato);

        StartCoroutine(this.Spawn(s));
    }

    public IEnumerator Spawn(StatoController s)
    {
        Transform trans = s.GetComponent<Transform>();

        GameObject plus = Instantiate(plusOne);
        plus.GetComponent<Transform>().rotation = Camera.main.GetComponent<Transform>().rotation;
        plus.GetComponent<Transform>().position = new Vector3(trans.position.x, trans.position.y + 10, trans.position.z);
        plus.GetComponent<Animator>().Play("PlusOne");
        yield return new WaitForSeconds(1);

        Destroy(plus);

    }
	
}
