using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftConfig", menuName = "SHMUP/CraftConfiguration")]
public class CraftConfiguration : ScriptableObject
{
    public const int MAX_SHOT_POWER = 10;
    public float speed;
    public float bulletStrenth;
    public float beamSrength;
    public Sprite craftSprite;

    public ShotConfiguration[] shotLevel = new ShotConfiguration[MAX_SHOT_POWER];
}

[Serializable]
public class ShotConfiguration
{
    public int[] spawnerSizes = new int[5];

}