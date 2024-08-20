using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public AnimatedNumber[] playerScore = new AnimatedNumber[2];
    public AnimatedNumber topScore;
    public GameObject player2Start;

    public PlayerHUD[] playerHUDs = new PlayerHUD[2];

    private void FixedUpdate()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (!GameManager.Instance) return;

        //Score
        if (playerScore[0])
        {
            int p1Score = GameManager.Instance.playerDatas[0].score;
            Debug.Log("score " +  p1Score);
            playerScore[0].UpdateNumber(p1Score);
        }

        if (GameManager.Instance.playerCrafts[0])
        {
            UpdateLives(0);
        }
        //if (GameManager.Instance.twoPlayer)
        //{
        //    if (player2Start)
        //        player2Start.SetActive(false);
        //
        //    if (playerScore[1])
        //    {
        //        int p2Score = GameManager.Instance.playerDatas[1].score;
        //        playerScore[0].UpdateNumber(p2Score);
        //    }
        //}
        //else
        //{
        //    if (player2Start)
        //        player2Start.SetActive(true);
        //}
    }



    private void UpdateLives(int playerIndex)
    {

        PlayerData data = GameManager.Instance.playerDatas[playerIndex];
        PlayerHUD hud = playerHUDs[playerIndex];


        int healthHud = data.health;
        //float fillammoutRatio = 1 / (float)healthHud;

        hud.healthImage.fillAmount = (float)healthHud / (float)PlayerData.MAXHEALTH;
        Debug.Log(hud.healthImage.fillAmount);
    }

    [Serializable]
    public class PlayerHUD
    {
        public Image healthImage;
    }
}
