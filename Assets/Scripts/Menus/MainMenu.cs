using UnityEngine;

public class MainMenu : Menu
{
    public static MainMenu Instance { get; private set; }



    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 MainMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("MainMenu Created!");
    }
}
