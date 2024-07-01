using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(1);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}