using UnityEngine;

public class AudioOptionsMenu : Menu
{
    public static AudioOptionsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 AudioOptionsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("AudioOptionsMenu Created!");
    }
}