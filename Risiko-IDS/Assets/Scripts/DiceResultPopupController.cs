using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class DiceResultPopupController : MonoBehaviour 
{
    private int[] _attackDices, _defenseDices;
    private bool _statoConquistato;
    private Color _greenColor = new Color(0,0.9f,0);
    private Color _redColor = new Color(0.9f, 0, 0);

    public GameObject[] attackDices = new GameObject[3];
    public GameObject[] defenseDices = new GameObject[3];
    public Sprite[] diceFaces = new Sprite[6];
    public GameObject territorioConquistatoPanel;
    public GameObject titleText;

    public delegate void Close();
    public event Close ClosePopup;

	void Start ()
    {
        //#region debug-test
        //int[] attack = {6,5,3};
        //int[] defense = {6,4};

        //initPopup("Risultato lancio dadi", attack, defense, true);
        //#endregion

        this.ClosePopup += hide;
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void initPopup(string title,int[] attackDices, int[] defenseDices, bool statoConquistato)
    {
        this.AttackDices = attackDices;
        this.DefenseDices = defenseDices;
        this.StatoConquistato = statoConquistato;
        this.Title = title;
        this.refreshView();
        
    }

    public void refreshView()
    {
        try
        {
            for (int i = 0; i < _attackDices.Length; i++)
            {
                attackDices[i].GetComponent<Image>().sprite = diceFaces[_attackDices[i] - 1];
            }

            for (int i = 0; i < _defenseDices.Length; i++)
            {
                defenseDices[i].GetComponent<Image>().sprite = diceFaces[_defenseDices[i] - 1];
                if (_attackDices[i] > _defenseDices[i])
                {
                    defenseDices[i].GetComponent<Image>().color = _redColor;
                    attackDices[i].GetComponent<Image>().color = _greenColor;
                }
                else
                {
                    defenseDices[i].GetComponent<Image>().color = _greenColor;
                    attackDices[i].GetComponent<Image>().color = _redColor;
                }
            }

            territorioConquistatoPanel.SetActive(this._statoConquistato);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("Valore non valido in lista dadi attacco/difesa!");
        }
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

    private int[] AttackDices
    {
        set
        {
            if( value != null && value.Length > 0)
            {
                this._attackDices = value;
            }
            else
            {
                throw new ArgumentException("Lista dadi attacco vuota o nulla!");
            }
        }
    }

    private int[] DefenseDices
    {
        set
        {
            if (value != null && value.Length > 0)
            {
                this._defenseDices = value;
            }
            else
            {
                throw new ArgumentException("Lista dadi difesa vuota o nulla!");
            }
        }
    }

    private bool StatoConquistato
    {
        set
        {
            //bool non puo' essere null...
            this._statoConquistato = value;
        }
    }

    #endregion

    public void closePopup()
    {
        if (this.ClosePopup != null)
        {
            this.ClosePopup();
        }
        Destroy(this.gameObject);
    }
}
