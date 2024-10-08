using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Scene displayScene;
    void Start()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
                displayScene = SceneManager.GetSceneByName("DisplayScene");
            }
            else
            {
                Debug.LogError("gameMangerPrefab isn't set!");
            }
        }
        if (!displayScene.isLoaded)
        {
            SceneManager.LoadScene("DisplayScene", LoadSceneMode.Additive);
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
                    GameManager.Instance.ResetData();
                    GameManager.Instance.gameState = GameManager.Gamestate.InMenus;
                    break;
                case GameMode.Gameplay:
                    MenuManager.Instance.SwitchToGameplayMenus();
                    GameManager.Instance.gameState = GameManager.Gamestate.Playing;
                    
                    break;
            };



            if (gameMode == GameMode.Gameplay)
            {
                if (!GameManager.Instance.playerCrafts[0])
                {
                    GameManager.Instance.SpawnPlayer(0, 0);
                }
            }

                menuLoaded = true;
        }
    }
}