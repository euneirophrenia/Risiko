using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour 
{
    public Menu currentMenu;
    public AudioSource audio;
    public GameObject crossMenu;
    public GameObject crossPauseMenu;
    public GameObject MainScene;
    public GameObject directionalLight;

    private bool isMuted = false;
    private bool isPaused = false;
    private float timeScale;         
    private int playerNumber = 0;
    private string[] playernames;
	
    public void Start()
    {
        playernames = new string[Mathf.Max(Settings.PlayersNumber)];
        ShowMenu(currentMenu);
    }

    public void ShowMenu(Menu menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;

        if (menu == null)
        {
            this.isPaused = false;
            Time.timeScale = this.timeScale;
           // GameObject.Find("MainScene/Menu/PausePlane").SetActive(false);              //PLANE 
        }

        if (menu != null)
        {
            currentMenu = menu;
            currentMenu.IsOpen = true;
        }
    }

    public void Mute()
    {
        if (!isMuted)
        {
            this.audio.Pause();
            this.isMuted = true;
            
           
            crossPauseMenu.SetActive(true);
            crossMenu.SetActive(true);
        }
        else
        {
            this.audio.UnPause();
            this.isMuted = false;

            
             crossPauseMenu.SetActive(false);
             crossMenu.SetActive(false);
        }
    }

    public void VolumeChange(float value)
    {
        this.audio.volume = value;
    }
   
    public void InputSpawn(int number)
    {
        this.playerNumber = number;

        for (int i = 1; i < Mathf.Max(Settings.PlayersNumber) + 1; i++)
        {
            string s = "InitialMenu/Menu/SelectMenu/Panel/Buttons/Input" + i;
            GameObject.Find(s).SetActive(false);
        }

        for (int i = 1; i < number + 1; i++)
        {
            string s = "InitialMenu/Menu/SelectMenu/Panel/Buttons/Input" + i;
            GameObject.Find(s).SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UnPause()
    {
        this.ShowMenu(null);
        Time.timeScale = this.timeScale;
    }

    public void Update()
    {
        
       if (Input.GetKeyUp(KeyCode.Escape) && MainScene.activeInHierarchy)           //Esegue solo se MainScene è attiva (InitMenu disattivata)
       {
            if (!this.isPaused)
            {
                //this.ShowMenu(GameObject.Find("MainScene/Menu/MainMenu").GetComponent<Menu>());
                this.ShowMenu(GameObject.Find("MainScene/GUI/Menu/MainMenu").GetComponent<Menu>());

                //GameObject.Find("MainScene/Menu/PausePlane").SetActive(true);                   //PLANE 
                this.timeScale = Time.timeScale;
                Time.timeScale = 0;
                this.isPaused = true;                                  
            }
            else
            {
                this.ShowMenu(null);

            }
        }
        
    }

	public void Play()
	{
        if (this.playerNumber != 0)
        {
            for (int i = 1; i < this.playerNumber + 1; i++)
            {
                string s = "InitialMenu/Menu/SelectMenu/Panel/Buttons/Input" + i;
                InputField input = GameObject.Find(s).GetComponent<InputField>();

                if (string.IsNullOrEmpty(input.text))
                {
                    EditorUtility.DisplayDialog("Error", "Player names are not valid", "Ok", null);
                    return;
                }
                   
                if (new List<string>(playernames).Contains(input.text))
                {
                    EditorUtility.DisplayDialog("Error", "Player names cannot be duplicated", "Ok", null);
                    playernames = new string[Mathf.Max(Settings.PlayersNumber)];
                    return;
                }

                playernames[i - 1] = input.text.Trim();
            }
        }

        //Debug.Log(playernames[0] + " " + playernames[1]);


       /*
        * 
        * Invocazione di InitialPhaseManager per la creazione dei giocatori ecc
        * 
        */

        //MainManager.GetInstance().Init(playernames);    

        MainScene.SetActive(true);

                
        GameObject.Find("InitialMenu").SetActive(false);
        
        currentMenu = null;                             
	} 
}
