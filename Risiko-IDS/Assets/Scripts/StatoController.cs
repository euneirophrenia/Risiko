using UnityEngine;
using System.Collections;

public class StatoController : MonoBehaviour 
{
	public GUISkin GameSkin;
	private string objectname ;

	private Color startColor;
	private bool _displayObjectName;

	void Start()
	{
		objectname = this.gameObject.name;
	}
	void OnGUI()
	{
		GUI.skin = GameSkin;
		DisplayName ();
	}

	void OnMouseEnter()
	{
		startColor = GetComponent<Renderer>().material.color;
		Color col = new Color (1f - startColor.r, 1f - startColor.g, 1f - startColor.b);
		GetComponent<Renderer> ().material.color = col;
		_displayObjectName = true;
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = startColor;
		_displayObjectName = false;
	}

	public void DisplayName()
	{
		if (_displayObjectName) 
		{
			GUI.Box(new Rect(Event.current.mousePosition.x -155,Event.current.mousePosition.y, 150,25),objectname, "BoxGUI");
		}
	}
}
