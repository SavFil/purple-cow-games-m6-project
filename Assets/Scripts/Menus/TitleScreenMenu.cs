using UnityEngine;

public class TitleScreenMenu : Menu
{
    public static TitleScreenMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 TitleScreenMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("TitleScreenMenu Created!");
    }

    public void OnFireButton()
    {
        //TurnOff();
        //MainMenu.Instance.TurnOn();
    }
}
