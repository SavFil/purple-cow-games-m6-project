using UnityEngine;

public class PlayMenu : Menu
{
    public static PlayMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 PlayMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("PlayMenu Created!");
    }

    public void OnNormalButton()
    {
        TurnOff(false);
        CraftSelectMenu.Instance.TurnOn(this);
    }

    public void OnBulletHellButton()
    {
        TurnOff(false);
        CraftSelectMenu.Instance.TurnOn(this);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}
