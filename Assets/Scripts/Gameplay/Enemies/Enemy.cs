using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    private EnemyPattern pattern;

    private EnemySection[] sections;

    private void Start()
    {
        sections = gameObject.GetComponentsInChildren<EnemySection>();
    }

    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }

    private void FixedUpdate()
    {
        data.progressTimer++;
        if (pattern)
            pattern.Calculate(transform, data.progressTimer);

        // off screen check - maybe  numbers need tweaking for our game
        float y = transform.position.y;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
            y -= GameManager.Instance.progressWindow.data.positionY;
        if (y < -350)
            OutOfBounds();
    }

    private void OutOfBounds()
    {
        Destroy(gameObject);
    }

    public void EnableState(string name)
    {
        foreach (EnemySection section in sections)
        {
            section.EnableState(name);
        }
    }

    public void DisableState(string name)
    {
        foreach (EnemySection section in sections)
        {
            section.DisableState(name);
        }
    }
}

[Serializable]

public struct EnemyData
{
    public float progressTimer;

    public float positionX;
    public float positionY;

    public int patternUID;

}