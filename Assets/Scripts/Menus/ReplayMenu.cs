using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayMenu : Menu
{
    public static ReplayMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 ReplayMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("ReplayMenu Created!");
    }
}
