using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
    private List<string> playernames;
    private GameObject canvas;
    private GameObject errorPopup;
	
    public void Start()
    {
        playernames = new List<string>();
        this.canvas = GameObject.Find("InitialMenu/Menu");
        this.errorPopup = Resources.Load<GameObject>("GenericPopup");
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
            MainManager.GetInstance().StateClickEnabled = true;
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
        MainManager.GetInstance().Quit();
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
                this.ShowMenu(GameObject.Find("MainScene/GUI/Menu/MainMenu").GetComponent<Menu>());
                this.timeScale = Time.timeScale;
                Time.timeScale = 0;
                this.isPaused = true;
                MainManager.GetInstance().StateClickEnabled = false;

                foreach (StatoController s in MainManager.GetInstance().States)
                {
                    s.Toggle(false);
                }
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
                    //EditorUtility.DisplayDialog("Error", "Player names are not valid", "Ok", null);
                    
                    GameObject ob = GameObject.Instantiate(this.errorPopup);

                    ob.GetComponent<Transform>().parent = this.canvas.transform;
                    ob.GetComponent<Transform>().position = this.canvas.transform.position;

                    ob.GetComponent<GenericPopupController>().initPopup("Error", "Player names are not valid");
                    return;
                }
                   
                if (playernames.Contains(input.text))
                {
                    //EditorUtility.DisplayDialog("Error", "Player names cannot be duplicated", "Ok", null);
                    GameObject ob = GameObject.Instantiate(this.errorPopup);

                    ob.GetComponent<Transform>().parent = this.canvas.transform;
                    ob.GetComponent<Transform>().position = this.canvas.transform.position;

                    ob.GetComponent<GenericPopupController>().initPopup("Error", "Player names cannot be duplicated");
                    playernames = new List<string>();
                    return;
                }

                playernames.Add(input.text.Trim());
            }

        }

        InputField inp = GameObject.Find("InitialMenu/Menu/SelectMenu/Panel/Buttons/Input1").GetComponent<InputField>();

        if (!inp.isActiveAndEnabled)
        {
            //EditorUtility.DisplayDialog("Error", "Decide how many players", "Ok", null);

            GameObject ob = GameObject.Instantiate(this.errorPopup);

            ob.GetComponent<Transform>().parent = this.canvas.transform;
            ob.GetComponent<Transform>().position = this.canvas.transform.position;

            ob.GetComponent<GenericPopupController>().initPopup("Error", "Decide how many players");
            return;
        }

        //Debug.Log(playernames[0] + " " + playernames[1]);

		MainScene.SetActive(true);
		GameObject.Find("InitialMenu").SetActive(false);
        MainManager.GetInstance().Init(playernames.ToArray());    

     

                
        
        
        currentMenu = null;                             
	} 
}
