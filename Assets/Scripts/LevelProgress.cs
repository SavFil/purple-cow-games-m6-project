using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public ProgressData data;

    public int levelSize;

    public AnimationCurve speedCurve = new AnimationCurve();

    // Start is called before the first frame update
    void Start()
    {
        data.positionX = transform.position.x;
        data.positionY = transform.position.y;
    }

    // Update is called once per frame
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
    }
}

[Serializable]
public class ProgressData
{
    public int progress;
    public float positionX;
    public float positionY;
};
