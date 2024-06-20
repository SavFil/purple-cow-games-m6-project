using UnityEngine;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 ControlsOptionsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("ControlsOptionsMenu Created!");
    }

    public void OnShootButton()
    {
        // to do and all other buttons
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}