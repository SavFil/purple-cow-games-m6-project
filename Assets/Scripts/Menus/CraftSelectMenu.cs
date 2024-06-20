using UnityEngine;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 CraftSelectMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("CraftSelectMenu Created!");
    }

    public void OnPlayButton()
    {
        // to do as well the a-z buttons
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}