using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
    public GameObject front;
    public GameObject retro;

    private bool isHidden;

    public void Start()
    {
        this.isHidden = true;
    }
	
    public void Turn()
    {
        if (this.retro.GetComponent<Animation>().isPlaying || this.front.GetComponent<Animation>().isPlaying)
            return;

        if (this.isHidden)
        {
            this.retro.GetComponent<Animation>().Play("HideCard");
            this.front.GetComponent<Animation>().Play("ShowCardDelay");
        }
        else
        {
            this.front.GetComponent<Animation>().Play("HideCard");
            this.retro.GetComponent<Animation>().Play("ShowCardDelay");
        }

        this.isHidden = !this.isHidden;
    }
	
}
