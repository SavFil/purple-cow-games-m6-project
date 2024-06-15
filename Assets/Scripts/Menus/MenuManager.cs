using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    internal Menu activeMenu = null;
    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 MenuManager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("MenuManager Created!");
    }

    public void SwitchToGameplay()
    {
        
    }

    public void SwitchToMainMenus()
    {

    }
}
