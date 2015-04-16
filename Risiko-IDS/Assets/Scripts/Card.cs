using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
    public GameObject front;
    public GameObject retro;
    public GameObject button;
	
    public void Turn()
    {
        if (button.GetComponentInChildren<Text>().text == "Show")
        {
            this.retro.GetComponent<Animation>().Play("RetroShow");
            this.front.GetComponent<Animation>().Play("FrontShow");
            button.GetComponentInChildren<Text>().text = "Hide";
        }
        else
        {
            this.front.GetComponent<Animation>().Play("FrontHide");
            this.retro.GetComponent<Animation>().Play("RetroHide");
            button.GetComponentInChildren<Text>().text = "Show";
        }
    }
	
}
