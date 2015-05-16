using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class MenuManager : MonoBehaviour 
{
    public Menu currentMenu;
    public AudioSource audio;
    public GameObject cross;

    private bool isMuted = false;
    private bool isPaused = false;
    private float timeScale;
    private readonly int maxplayers = 5;
    private int playerNumber = 0;
    private string[] playernames = new string[5];
	
    public void Start()
    {
        ShowMenu(currentMenu);
    }

    public void ShowMenu(Menu menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;

        if (menu == null)
        {
            this.isPaused = false;
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
            cross.SetActive(true);
        }
        else
        {
            this.audio.UnPause();
            this.isMuted = false;
            this.cross.SetActive(false);
        }
    }

    public void VolumeChange(float value)
    {
        this.audio.volume = value;
    }
   
    public void InputSpawn(int number)
    {
        this.playerNumber = number;

        for (int i = 1; i < maxplayers+1;  i++)
        {
            string s = "Menu/SelectMenu/Panel/Buttons/Input" + i;
            GameObject.Find(s).SetActive(false);
        }

        for (int i = 1; i < number + 1; i++)
        {
            string s = "Menu/SelectMenu/Panel/Buttons/Input" + i;
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
        if(Application.loadedLevelName.Contains("BaseMap"))
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!this.isPaused)
                {
                    this.ShowMenu(GameObject.Find("Menu/MainMenu").GetComponent<Menu>());
                    this.timeScale = Time.timeScale;
                    Time.timeScale = 0;
                    this.isPaused = true;                                  
                }
                else
                {
                    this.ShowMenu(null);
                    Time.timeScale = this.timeScale;
                    //this.isPaused = false;

                }
            }
        }
    }


	public void ChangeToScene(string sceneToChange)
	{
        if (this.playerNumber != 0)
        {
            for (int i = 1; i < this.playerNumber + 1; i++)
            {
                string s = "Menu/SelectMenu/Panel/Buttons/Input" + i;
                InputField input = GameObject.Find(s).GetComponent<InputField>();

                if (string.IsNullOrEmpty(input.text))
                {
                    EditorUtility.DisplayDialog("Error", "Player names are not valid", "Ok", null);
                    return;
                }
                   

                playernames[i - 1] = input.text.Trim();
            }
        }

        Debug.Log(playernames[0] + " " + playernames[1]);

        GameObject data = GameObject.Find("GameData");
        data.GetComponent<Text>().text = this.playerNumber+"\n";

        for (int i = 0; i < this.playerNumber; i++)
        {
            data.GetComponent<Text>().text += this.playernames[i]+"\n";
        }
       
        Object.DontDestroyOnLoad(data);

		Application.LoadLevel(sceneToChange);
       
        currentMenu = null;
	} 
}
