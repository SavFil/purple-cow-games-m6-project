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

    private BulletManager bulletManager = null;

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
        Debug.Log("Spawning player" + playerIndex);
        playerOneCraft = Instantiate(craftPrefabs[craftType]).GetComponent<Craft>();
        playerOneCraft.playerIndex = playerIndex;
    }

    //Debug code for testing purposes.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerOneCraft) SpawnPlayer(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (playerOneCraft) playerOneCraft.Explode();
        }

        if (Input.GetKeyDown(KeyCode.S))
            if (bulletManager)
                bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3, 0, 150, Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }
}