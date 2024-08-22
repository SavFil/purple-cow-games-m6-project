using UnityEngine;

public class PauseMenu : Menu
{
    public static PauseMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 PauseMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("PauseMenu Created!");
    }
}