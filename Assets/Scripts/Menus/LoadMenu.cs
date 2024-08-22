using UnityEngine;

public class LoadMenu : Menu
{
    public static LoadMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 LoadMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("LoadMenu Created!");
    }
}