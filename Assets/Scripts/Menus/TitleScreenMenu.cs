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
        //Debug.Log("TitleScreenMenu Created!");
    }

    private void Update()
    {
        if (ROOT.gameObject.activeInHierarchy)
        {
            if (InputManager.Instance.CheckForPlayerInput(0))
            {
                TurnOff(false);
                MainMenu.Instance.TurnOn(null);
            }
        }
    }
}
