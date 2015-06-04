using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputTab : MonoBehaviour
{
    EventSystem system;

    void Start()
    {
        system = EventSystem.current;// EventSystemManager.currentSystem;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");

        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            MenuManager menuManager = GameObject.Find("MainManager/MenuManager").GetComponent<MenuManager>();
            
            if(menuManager.currentMenu.Equals(GameObject.Find("InitialMenu/Menu/SelectMenu").GetComponent<Menu>()))
            {
                menuManager.Play();
            }
        }
    }
}