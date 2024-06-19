using UnityEngine;

public class CreditsMenu : Menu
{
    public static CreditsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 CreditsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("CreditsMenu Created!");
    }
}