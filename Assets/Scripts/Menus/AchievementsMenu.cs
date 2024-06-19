using UnityEngine;

public class AchievementsMenu : Menu
{
    public static AchievementsMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 AchievementsMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("AchievementsMenu Created!");
    }
}