using UnityEngine;
using System.Collections;

public class TextPanelAutoScale : MonoBehaviour 
{

	private float scaleX=0.12f,scaleY=0.19f;

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
		this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-(float)(screenSize.x/2.0 - panelSize.x/2)+screenSize.x*0.09f,-(float)(screenSize.y/2.0 - panelSize.y/2));
	}
}
