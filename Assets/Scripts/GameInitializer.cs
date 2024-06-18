using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private bool menuLoaded = false;
    public enum GameMode
    {
        INVALID,
        Menus,
        Gameplay
    }

    public GameMode gameMode;
    public GameObject gameManagerPrefab = null;
    void Start()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("gameMangerPrefab isn't set!");
            }
        }
    }

    public void Update()
    {
        if (!menuLoaded)
        {
            switch (gameMode)
            {
                case GameMode.Menus:
                    MenuManager.Instance.SwitchToMainMenuMenus();
                    break;
                case GameMode.Gameplay:
                    MenuManager.Instance.SwitchToGameplayMenus();
                    break;
            };
            menuLoaded = true;
        }
    }
}
