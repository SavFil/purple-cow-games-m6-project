using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStep
{
   public enum MovementType
    {
        INVALID,

        none,
        direction,
        spline,
        atTarget,
        homing,
        follow,
        circle,

        NOOFMOVEMENTTYPES
    }

    [SerializeField]
    public MovementType movement;

    [SerializeField]
    public Vector2 direction;

    [SerializeField]
    [Range(1, 20)]
    public float movementSpeed = 4;

    public float TimeToComplete()
    {
        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            return timeToTravel;
        }

        Debug.LogError("TimeToComplete unprocessed movement type, returning 1");
        return 1;
    }

    public Vector2 EndPosition(Vector3 startPosition)
    {
        Vector2 result = startPosition;

        if (movement == MovementType.direction)
        {
            result += direction;
            return result;
        }

        Debug.LogError("EndPosition unprocessed movement type, returning start");
        return result;
    }
}
