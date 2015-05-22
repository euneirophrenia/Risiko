using UnityEngine;
using System.Collections;

public class CardPanelAutoScale : MonoBehaviour 
{

	private float scaleX=0.16f,scaleY=0.38f;

	// Use this for initialization
	void Start () 
	{
	
	}

	// Update is called once per frame
	void Update () 
	{
		Vector2 panelSize;
		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);
		panelSize = new Vector2 ((float)(screenSize.x * scaleX), (float)(screenSize.y*scaleY));
		this.GetComponent<RectTransform> ().sizeDelta = panelSize;
		this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 ((float)(screenSize.x/2.0 - panelSize.x/2),-(float)(screenSize.y/2.0 - panelSize.y/2));
	}
}
