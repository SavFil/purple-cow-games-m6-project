using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject ROOT = null;
    public Menu previousMenu = null;
    public GameObject previousItem = null;

    public virtual void TurnOn(Menu previous)
    {
        if (ROOT)
        {
            if (previousMenu != null)
            {
                previousMenu = previous;
            }
            ROOT.SetActive(true);
            if (previousItem)
            {
                EventSystem.current.SetSelectedGameObject(previousItem);
            }
            Debug.LogError("ROOT object not set.");
            return;
        }
        ROOT.SetActive(true);
    }

    public virtual void TurnOff(bool returnToPrevious)
    {
        if (ROOT)
        {
            if (EventSystem.current)
            {
                previousItem = EventSystem.current.currentSelectedGameObject;
            }

            ROOT.SetActive(false);

            if (previousMenu & returnToPrevious) {
                previousMenu.TurnOn(null);
            }
           
        }
        else
        {
            Debug.LogError("ROOT object not set.");
        }
        
    }
}
