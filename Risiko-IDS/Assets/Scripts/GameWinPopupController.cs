using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class GameWinPopupController : MonoBehaviour {

    public GameObject titleText, descriptionText;
    public delegate void NewGame();
    public delegate void QuitGame();

    public event NewGame NewGamePressed;
    public event QuitGame QuitGamePressed;

	void Start ()
    {
        #region debug-test
        //initPopup("VITTORIA!!", "Complimenti, hai vinto!");
        #endregion

        this.NewGamePressed += hide;
        this.NewGamePressed += MainManager.GetInstance().NewGame;
        this.QuitGamePressed += hide;
        this.QuitGamePressed += MainManager.GetInstance().Quit;
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void initPopup(string title, string description)
    {
        this.Title = title;
        this.Description = description;
    }

    #region Properties
    private string Title
    {
        set
        {
            if (value != null && value != "")
            {
                this.titleText.GetComponent<Text>().text = value;
            }
            else
            {
                throw new ArgumentException("Titolo vuoto o nullo!");
            }
        }
    }

    private string Description
    {
        set
        {
            if (value != null && value != "")
            {
                this.descriptionText.GetComponent<Text>().text = value;
            }
            else
            {
                throw new ArgumentException("Descrizione vuota o nulla!");
            }
        }
    }

    #endregion

    public void onQuitPressed()
    {
        if( this.QuitGamePressed != null)
        {
            this.QuitGamePressed();
        }
        Destroy(this.gameObject);
    }

    public void onNewGamePressed()
    {
        if( this.NewGamePressed != null)
        {
            this.NewGamePressed();
        }
        Destroy(this.gameObject);
    }
}
