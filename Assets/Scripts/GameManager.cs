using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private GameVersionSO gameVersionSO;
    [Tooltip("The version of the game.")]
    [SerializeField] private string currentVersion;
    public bool twoPlayer = false;

    public GameObject[] craftPrefabs;

    public Craft playerOneCraft = null;

    public BulletManager bulletManager = null;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 GameManager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager Created!");

        currentVersion = gameVersionSO.Version;

        bulletManager = GetComponent<BulletManager>();
    }

    public void SpawnPlayer(int playerIndex, int craftType)
    {
        Debug.Assert(craftType < craftPrefabs.Length);
        Debug.Log("Spawning player " + playerIndex);
        playerOneCraft = Instantiate(craftPrefabs[craftType]).GetComponent<Craft>();
        playerOneCraft.playerIndex = playerIndex;
    }

    //Debug code for testing purposes.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerOneCraft) SpawnPlayer(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerOneCraft)
            {
                playerOneCraft.AddOption();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerOneCraft && playerOneCraft.craftData.shotPower < CraftConfiguration.MAX_SHOT_POWER - 1)
            {
                playerOneCraft.craftData.shotPower++;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerOneCraft)
            {
                playerOneCraft.IncreaseBeamStrength();
            }
        }



    }
}