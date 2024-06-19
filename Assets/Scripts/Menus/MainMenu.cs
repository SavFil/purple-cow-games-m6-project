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

    public void OnPlayButton()
    {
        TurnOff(true);
        PlayMenu.Instance.TurnOn(this);
    }

    public void OnOptionsButton()
    {
        TurnOff(true);
        OptionsMenu.Instance.TurnOn(this);
    }

    public void OnScoresButton()
    {
        TurnOff(true);
        ScoresMenu.Instance.TurnOn(this);
    }

    public void OnAchievementsButton()
    {
        TurnOff(true);
        AchievementsMenu.Instance.TurnOn(this);
    }

    public void OnCreditsButton()
    {
        TurnOff(true);
        CreditsMenu.Instance.TurnOn(this);
    }

    public void OnQuitButton()
    {
        TurnOff(true);
        YesNoMenu.Instance.TurnOn(this);
    }
}
