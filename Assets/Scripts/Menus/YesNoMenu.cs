using UnityEngine;

public class YesNoMenu : Menu
{
    public static YesNoMenu Instance { get; private set; }

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 YesNoMenu!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("YesNoMenu Created!");
    }
}
