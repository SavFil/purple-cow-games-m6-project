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
        return Vector2.zero;
    }

    public Quaternion CalculateRotation(float progressTimer)
    {
        return Quaternion.identity;
    }
}