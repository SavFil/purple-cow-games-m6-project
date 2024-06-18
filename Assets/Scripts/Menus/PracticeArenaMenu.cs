using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeArenaMenu : Menu
{
    public static PracticeArenaMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 PracticeArenaMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("PracticeArenaMenu Created!");
    }
}
