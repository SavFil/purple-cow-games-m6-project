using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    private EnemyPattern pattern;

    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern; 
        pattern = inPattern; 
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