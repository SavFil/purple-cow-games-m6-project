using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : Menu
{
    public static GameOverMenu Instance { get; private set; }
    public TextMeshProUGUI scoreReadout = null;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 GameOverMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("GameOverMenu Created!");
    }


    public void GameOver()
    {
        TurnOn(null);
        // AudioManager.instance.PlayMusic(AudioManager.Tracks.GameOver, true, 0.5f);
        scoreReadout.text = GameManager.Instance.playerDatas[0].score.ToString(); 
    }

    public void OnReturnButton()
    {
        SceneManager.LoadScene("MainMenusScene");
    }
    public void OnRetryButton()
    {
        GameManager.Instance.StartGame();
    }
}
