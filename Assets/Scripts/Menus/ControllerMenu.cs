using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerMenu : Menu
{
    public static ControllerMenu Instance { get; private set; }
    [Header ("CONTROLLER SETTINGS")]
    public int whichPlayer = 0;

    public TextMeshProUGUI textUI = null;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 ControllerMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("ControllerMenu Created!");
    }

    private void Update()
    {
        if (ROOT.gameObject.activeInHierarchy)
        {
            if (InputManager.Instance.CheckForPlayerInput(whichPlayer))
            {
                TurnOff(false);
                //GameManager.instance.ResumeGameplay();
            }
        }
    }
}
