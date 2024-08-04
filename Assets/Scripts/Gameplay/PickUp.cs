using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        INVALID,

        Bomb,
        Coin,
        PowerUp,
        BeamUp,
        Options,
        Medal,
        Secret,
        Lives,

        NOOFPICKUPTYPES
    };

    public PickUpConfig config;
    public Vector2 position;
    public Vector2 velocity;

    private void OnEnable()
    {
        position = transform.position;
    }

    private void FixedUpdate()
    {
        //Move
        position.y -= config.fallSpeed;
        transform.position = position;
    }

    public void ProcessPickUp(int playerIndex, CraftData craftData)
    {
        Destroy(gameObject);
    }
}
