using UnityEngine;

public class ScoresMenu : Menu
{
    public static ScoresMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 ScoresMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("ScoresMenu Created!");
    }
}