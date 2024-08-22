using UnityEngine;

public class GraphicsOptionsMenu : Menu
{
    public static GraphicsOptionsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 GraphicsOptionsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("GraphicsOptionsMenu Created!");
    }

    public void OnApplyButton()
    {
        // to do and all other buttons
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}