using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public ProgressData data;

    public int levelSize;



    public AnimationCurve speedCurve = new AnimationCurve();

    private Craft player1craft = null;

    void Start()
    {
        data.positionX = transform.position.x;
        data.positionY = transform.position.y;

        if (GameManager.Instance)
            GameManager.Instance.progressWindow = this;
    }

    void Update()
    {
        if (data.progress < levelSize)
        {
            float ratio = (float)data.progress / (float)levelSize;
            float movement = speedCurve.Evaluate(ratio);
            data.progress++;

            if (player1craft == null)
            {
                player1craft = GameManager.Instance.playerOneCraft;
            }
            if (player1craft)
            {
                UpdateProgressWindow(player1craft.craftData.positionX, movement);
            }
        }
    }
    void UpdateProgressWindow(float shipX, float movement)
    {
        data.positionX = shipX / 10f;
        data.positionY += movement;
        transform.position = new Vector3(data.positionX, data.positionY, 0);
    }
}

[Serializable]
public class ProgressData
{
    public int progress;
    public float positionX;
    public float positionY;
};
