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

        none,          // Waiting at a position
        direction,
        spline,
        atTarget,
        homing,
        follow,
        circle,

        NOOFMOVEMENTTYPES
    }

    public enum RotationType
    {
        INVALID,
        none,
        setAngle,
        lookAhead,
        spinning,
        facePlayer,
        NOOFROTATIONS,
    }

    [SerializeField]
    public RotationType rotate = RotationType.lookAhead;

    [SerializeField]
    public float endAngle = 0;

    [SerializeField]
    [Range(0.01f, 4f)]
    public float angleSpeed = 1;

    [SerializeField]
    public float noOfSpins = 1;

    [SerializeField]
    public MovementType movement;

    [SerializeField]
    public Vector2 direction;

    [SerializeField]
    public Spline spline;

    [SerializeField]
    [Range(0.01f, 20.0f)]
    public float movementSpeed = 4;

    [SerializeField]
    public float framesToWait = 30;

    public List<String> activateStates = new List<String>();
    public List<String> deActivateStates = new List<String>();

    public EnemyStep(MovementType inMovement)
    {
        movement = inMovement;
        direction = Vector2.zero;
        if (inMovement == MovementType.spline)
        {
            spline = new Spline();
        }
    }

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
        else if (movement == MovementType.spline)
        {
            return spline.Length() / movementSpeed;
        }
        else if (movement == MovementType.homing)
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
        else if (movement == MovementType.spline)
        {
            result += (spline.LastPoint() - spline.StartPoint());
            return result;
        }
        else if (movement == MovementType.homing)
        {
            if (GameManager.Instance && GameManager.Instance.playerOneCraft)
                return GameManager.Instance.playerOneCraft.transform.position;
            else
                return startPosition;
        }

        Debug.LogError("EndPosition unprocessed movement type, returning start");
        return result;
    }

    public Vector3 CalculatePosition(Vector2 startPos, float stepTime, Vector2 oldPosition, Quaternion oldAngle)
    {
        float normalisedTime = stepTime / TimeToComplete();
        if (normalisedTime < 0)
            normalisedTime = 0;

        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            float ratio = 0;
            if (timeToTravel != 0)
                ratio = stepTime / timeToTravel;

            Vector2 place = startPos + (direction * ratio);
            return place;
        }
        else if (movement == MovementType.none)
        {
            return startPos;
        }
        else if (movement == MovementType.spline)
        {
            return spline.GetPosition(normalisedTime) + startPos;
        }
        else if (movement == MovementType.homing) // homing in on player
        {
            Vector2 dir = (oldAngle * Vector2.down);
            Vector2 mov = (dir * movementSpeed);
            Vector2 pos = oldPosition + mov;
            return pos;
        }

        Debug.LogError("CalculatePosition unprocessed movement type, returning startPosition");
        return startPos;
    }

    public void FireActivateStates(Enemy enemy)
    {
        foreach (string state in activateStates)
        {
            enemy.EnableState(state);
        }
    }

    public void FireDeactivateStates(Enemy enemy)
    {
        foreach (string state in deActivateStates)
        {
            enemy.DisableState(state);
        }
    }

    internal float EndRotation() //TODO This is unfinished
    {
        return endAngle;
    }

    internal Quaternion CalculateRotation(float startRotation, Vector3 currentPosition, Vector3 oldPosition, float time)
    {
        float normalisedTime = time / TimeToComplete();
        if (normalisedTime <= 0)
            normalisedTime = 0;

        if (rotate == RotationType.setAngle)
        {
            Quaternion result = Quaternion.Euler(0, 0, endAngle);
            return result;
        }
        else if (rotate == RotationType.spinning)
        {
            float start = endAngle - (noOfSpins * 360);
            float angle = Mathf.Lerp(start, endAngle, normalisedTime);
            Quaternion result = Quaternion.Euler(0, 0, angle);
            return result;
        }
        else if (rotate == RotationType.facePlayer)
        {
            float angle = 0;
            Transform target = null;
            if (GameManager.Instance && GameManager.Instance.playerOneCraft)
                target = GameManager.Instance.playerOneCraft.transform;

            if (target!=null)
            {
                Vector2 currentDir = (currentPosition - oldPosition.normalized);
                Vector2 targetDir = (target.transform.position - currentPosition).normalized;
                Vector2 newDir = Vector2.Lerp(currentDir, targetDir, angleSpeed);
                angle = Vector2.SignedAngle(Vector2.down, newDir);
            }

            return Quaternion.Euler(0, 0, angle);
        }
        else if (rotate == RotationType.lookAhead)
        {
            Vector2 dir = (currentPosition - oldPosition).normalized;
            float angle = Vector2.SignedAngle(Vector2.down, dir);
            return Quaternion.Euler(0, 0, angle);
        }

        return Quaternion.Euler(0, 0, 0);
    }
}
