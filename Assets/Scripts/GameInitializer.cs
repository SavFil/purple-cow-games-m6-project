using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject gameManagerPrefab = null;
    void Start()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("gameMangerPrefab isn't set!");
            }
        }
    }
}
