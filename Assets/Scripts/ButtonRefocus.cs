using UnityEngine;
using UnityEngine.EventSystems;

// If there is no selected item, set the selected item to the event system's first selected item
public class ButtonRefocus : MonoBehaviour
{
    GameObject lastselect;

    void Start()
    {
        lastselect = gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current == null) return;//!gia ControlsOptionsMenu
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

}