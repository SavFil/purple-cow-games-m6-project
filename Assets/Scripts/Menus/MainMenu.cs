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
        //Debug.Log("MainMenu Created!");
    }

    public void OnPlayButton()
    {
        TurnOff(false);
        PlayMenu.Instance.TurnOn(this);
    }

    public void OnOptionsButton()
    {
        TurnOff(false);
        OptionsMenu.Instance.TurnOn(this);
    }

    public void OnScoresButton()
    {
        TurnOff(false);
        ScoresMenu.Instance.TurnOn(this);
    }

    public void OnAchievementsButton()
    {
        TurnOff(false);
        AchievementsMenu.Instance.TurnOn(this);
    }

    public void OnCreditsButton()
    {
        TurnOff(false);
        CreditsMenu.Instance.TurnOn(this);
    }

    public void OnQuitButton()
    {
        TurnOff(false);
        YesNoMenu.Instance.TurnOn(this);
    }
}
