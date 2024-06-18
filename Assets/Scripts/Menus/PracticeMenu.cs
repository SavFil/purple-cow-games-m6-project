using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMenu : Menu
{
    public static PracticeMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 PracticeMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("PracticeMenu Created!");
    }
}
