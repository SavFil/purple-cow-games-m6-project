using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Root = null;

    public virtual void TurnOn()
    {
        if (Root == null)
        {
            Debug.LogError("ROOT object not set.");
            return;
        }
        Root.SetActive(true);
    }

    public virtual void TurnOff()
    {
        if (Root == null)
        {
            Debug.LogError("ROOT object not set.");
            return;
        }
        Root.SetActive(false);
    }
}
