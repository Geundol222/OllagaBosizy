using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable IDInput;
    public Selectable PWDInput;
    /*public Selectable IDCreateInput;
    public Selectable PWDCreateInput;
    public Selectable PWDAgainInput;
    public Selectable NICKInput;*/
    public Button submitButton;


    void OnEnable()
    {
        system = EventSystem.current;
        IDInput.Select();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == PWDInput.gameObject)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (next != null)
                {
                    next.Select();
                }
            }
            else if (EventSystem.current.currentSelectedGameObject == IDInput.gameObject)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }
            /*else if (EventSystem.current.currentSelectedGameObject == IDCreateInput.gameObject || EventSystem.current.currentSelectedGameObject == PWDCreateInput.gameObject || EventSystem.current.currentSelectedGameObject == PWDAgainInput || EventSystem.current.currentSelectedGameObject == NICKInput.gameObject)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }*/
            else
                return;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            submitButton.onClick.Invoke();
            Debug.Log("Button pressed!");
        }
    }
}