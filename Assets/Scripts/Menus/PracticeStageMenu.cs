using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeStageMenu : Menu
{
    public static PracticeStageMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 PracticeStageMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("PracticeStageMenu Created!");
    }
}