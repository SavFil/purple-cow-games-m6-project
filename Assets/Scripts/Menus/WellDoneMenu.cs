using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WellDoneMenu : Menu
{
    public static WellDoneMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 WellDoneMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("WellDoneMenu Created!");
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("MainMenusScene");
    }

}
