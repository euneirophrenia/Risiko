﻿using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	public void ChangeToScene(string sceneToChange)
	{
		Application.LoadLevel(sceneToChange);
	}
}