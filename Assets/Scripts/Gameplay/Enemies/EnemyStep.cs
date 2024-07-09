using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

[Serializable]
public class EnemyStep
{
   public enum MovementType
    {
        INVALID,

        none,          // Waiting at a position
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

    [SerializeField]
    public float framesToWait = 30;

    public float TimeToComplete()
    {
        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            return timeToTravel;
        }
        else if (movement == MovementType.none)
        {
            return framesToWait;
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
        else if (movement == MovementType.none) 
        {
            return startPosition;
        }

        Debug.LogError("EndPosition unprocessed movement type, returning start");
        return result;
    }

    public Vector3 CalculatePosition(Vector2 startPos, float stepTime)
    {
        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            float ratio = stepTime / timeToTravel;

            Vector2 place = startPos + (direction * ratio);
            return place;
        }
        else if (movement == MovementType.none)
        {
            return startPos;
        }

        Debug.LogError("CalculatePosition unprocessed movement type, returning startPosition");
        return startPos;
    }
}
