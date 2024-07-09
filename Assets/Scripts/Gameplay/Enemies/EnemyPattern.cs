using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyPattern : MonoBehaviour
{
    public List<EnemyStep> steps = new List<EnemyStep>();

    public Enemy enemyPrefab;

    private Enemy spawnedEnemy;

    private int UID;

    [MenuItem("GameObject/SHMUP/EnemyPattern", false, 10)]
    static void CreateEnemyPatternObject(MenuCommand menuCommand)
    {
        Helpers helper = (Helpers)Resources.Load("Helper");
        if (helper != null)
        {
            GameObject go = new GameObject("EnemyPattern" + helper.nextFreePatternID);
            EnemyPattern pattern = go.AddComponent<EnemyPattern>();
            pattern.UID = helper.nextFreePatternID;
            helper.nextFreePatternID++;

            // Register creation with undo system
            Undo.RegisterCompleteObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        else Debug.LogError("Could not find Helper");
    }

    public void Spawn()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation).GetComponent<Enemy>();
        spawnedEnemy.SetPattern(this);
    }

    public Vector2 CalculatePosition(float progressTimer)
    {
        int s = WhichStep(progressTimer);
        EnemyStep step = steps[s];

        float stepTime = progressTimer - StartTime(s);

        Vector3 startPos = EndPosition(s-1);

        return step.CalculatePosition(startPos, stepTime);
    }

    public Quaternion CalculateRotation(float progressTimer)
    {
        return Quaternion.identity;
    }

    int WhichStep(float timer)
    {
        float timeToCheck = timer;
        for(int s=0; s<steps.Count;s++)
        {
            if (timeToCheck < steps[s].TimeToComplete())
                return s;
            timer -= steps[s].TimeToComplete();
        }
        return steps.Count- 1;
    }

    public float StartTime(int step)
    {
        if (step <= 0) return 0;

        float result = 0;
        for (int s=0; s<step;s++) 
        {
            result += steps[s].TimeToComplete();
        }

        return result;
    }

    public Vector3 EndPosition(int stepIndex)
    {
        Vector3 result = transform.position;
        if (stepIndex>=0) 
        {
            for (int s=0; s<=stepIndex; s++)
            {
                result = steps[s].EndPosition(result);
            }
        }
        return result;
    }
}