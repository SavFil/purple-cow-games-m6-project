using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WellDoneMenu : Menu
{
    public static WellDoneMenu Instance { get; private set; }
    public TextMeshProUGUI scoreReadout = null;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 WellDoneMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("WellDoneMenu Created!");
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("MainMenusScene");
    }


    public void WellDone()
    {
        Debug.Log("winmenu");
        TurnOn(null);
        scoreReadout.text = GameManager.Instance.playerDatas[0].score.ToString();
        
    }
}
