using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ChoicePopupController : MonoBehaviour 
{
    private List<int> _values;
    private int _index;
    
    public GameObject titleGameObject, inputFieldGameObject, descriptionGameObject;
    
    public delegate void Accept(int value);
    public delegate void Cancel();

    public event Accept AcceptPressed;
    public event Cancel CancelPressed;


	void Start ()
    {
        //#region debug-testing
        //_index = 0;
        //List<int> l = new List<int>();
        //l.Add(11); l.Add(2); l.Add(4); l.Add(7);

        //initPopup("Choice Popup", "Descrizione molto lunga di un popup che spero funzioni", l);
        //#endregion

        this.AcceptPressed += hide;
        this.CancelPressed += hide;
    }

    public void hide(int i)
    {
        this.hide();
        
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void nextValue()
    {
        try
        {
            //Debug.Log("nextValue() called: " + _values[_index]);
            Value = _values[_index+1];
            _index++;
        }
        catch( ArgumentOutOfRangeException)
        {
            //Debug.Log("nextValue() called: " + _values[_index]+"(max Value)");
            Value = _values[_index];
        }
       
    }

    public void previousValue()
    {
        try
        {
            //Debug.Log("previousValue() called: " + _values[_index]);
            Value = _values[_index - 1];
            _index--;
        }
        catch (ArgumentOutOfRangeException)
        {
            //Debug.Log("previousValue() called: " + _values[_index] + "(min Value)");
            Value = _values[_index];
        }
    }

    public void initPopup(string title,string description,List<int> values)
    {
        this.Title = title;
        this.Values = values;
        this.Description = description;
    }

    #region Properties

    private string Description
    {
        set
        {
            if (value != null && value != "")
            {
                this.descriptionGameObject.GetComponent<Text>().text = value;
            }
            else
            {
                throw new ArgumentException("Stringa nulla o vuota");
            }  
        }
    }

    private string Title
    {
        set 
        {
            if( value != null && value != "")
            {
                this.titleGameObject.GetComponent<Text>().text = value ;
            }
            else
            {
                throw new ArgumentException("Stringa nulla o vuota");
            }
        }
    }

    private List<int> Values
    {
        set
        {
            if( value != null && value.Capacity > 0)
            {
                this._values = value;
                this._values.Sort();
                this.inputFieldGameObject.GetComponent<InputField>().text = _values[0].ToString();
            }
            else
            {
                throw new ArgumentException("Lista nulla o vuota");
            }
        }
    }

    public int Value
    {
        get
        {
            return this._values[_index];
        }
        set
        {
            if( value != null)
            {
                this.inputFieldGameObject.GetComponent<InputField>().text = value.ToString();
            }
            else
            {
                throw new ArgumentNullException("value cannot be null");
            }
            
        }
    }

    #endregion

    public void OnAcceptPressed()
    {
        if( this.AcceptPressed != null )
        {
            this.AcceptPressed(this.Value);
        }
        Destroy(this.gameObject);
    }

    public void OnCancelPressed()
    {
        if (this.CancelPressed != null)
        {
            this.CancelPressed();
        }
        Destroy(this.gameObject);
    }

}
