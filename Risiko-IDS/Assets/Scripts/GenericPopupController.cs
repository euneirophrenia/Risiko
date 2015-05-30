using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GenericPopupController : MonoBehaviour 
{
    public GameObject titleText, descriptionText,spriteObj;
    //public Sprite sprite; //per testing rapido
    public delegate void Close();
    public event Close ClosePopup;

	void Start () 
    {
        #region debug-test
        initPopup("Generic Popup", "Description", null);
        #endregion

        this.ClosePopup += hide;
	}

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void initPopup(string title, string description, Sprite sprite=null)
    {
        this.Title = title;
        this.Description = description;
        this.Sprite = sprite;
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

    private Sprite Sprite
    {
        set
        {
            if (value != null )
            {
                this.spriteObj.GetComponent<Image>().sprite = value;
            }
            //immagine di default (=null) vuota 
        }
    }

    #endregion

    public void closePopup()
    {
        if( this.ClosePopup != null)
        {
            this.ClosePopup();
        }
        Destroy(this.gameObject);
    }
}
