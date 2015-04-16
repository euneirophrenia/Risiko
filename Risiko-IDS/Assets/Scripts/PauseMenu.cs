using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool paused = false;
    private GameObject pm;
    public Texture image;

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
            {
                pm = Instantiate<GameObject>(pauseMenu);
                Time.timeScale = 0;
                
            }
            else 
            {
                Destroy(pm);
                Time.timeScale = 1;
            }
        }
	
	}

/*
    void OnGUI()
    {
        if (paused)
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), new GUIContent(image));
            
    }
*/
}
