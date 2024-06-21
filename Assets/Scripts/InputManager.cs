using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 InputManager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("InputManager Created!");
    }
}
