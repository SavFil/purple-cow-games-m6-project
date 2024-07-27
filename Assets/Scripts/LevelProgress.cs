using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public ProgressData data;

    public int levelSize;

    public GameObject midGroundTileGrid;

    public float midGroundRate = 0.75f;

    public AnimationCurve speedCurve = new AnimationCurve();

    void Start()
    {
        data.positionX = transform.position.x;
        data.positionY = transform.position.y;
    }

    void Update()
    {
        if (data.progress < levelSize)
        {
            float ratio = (float)data.progress / (float)levelSize;
            float movement = speedCurve.Evaluate(ratio);
            data.progress++;
            UpdateProgressWindow(movement);
        }
    }
    void UpdateProgressWindow(float movement)
    {
        data.positionY += movement;
        transform.position = new Vector3(data.positionX, data.positionY, 0);
        //midGroundTileGrid.transform.position = new Vector3(0, data.positionY, 0);
    }
}

[Serializable]
public class ProgressData
{
    public int progress;
    public float positionX;
    public float positionY;
};
