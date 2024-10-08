using UnityEngine;

public class OptionsMenu : Menu
{
    public static OptionsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 OptionsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("OptionsMenu Created!");
    }

    public void OnGraphicsButton()
    {
        TurnOff(false);
        GraphicsOptionsMenu.Instance.TurnOn(this);
    }

    public void OnAudioButton()
    {
        TurnOff(false);
        AudioOptionsMenu.Instance.TurnOn(this);
    }

    public void OnControlsButton()
    {
        TurnOff(false);
        ControlsOptionsMenu.Instance.TurnOn(this);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}