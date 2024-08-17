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

    public LevelProgress progressWindow = null;


    //TESTING RESOLUTION\
    public Resolution resolution4K;
    public Resolution resolutionFHD;

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

        Application.targetFrameRate = 60;


        //TESTING RESOLUTION\
        
        resolution4K.width = 3840;
        resolution4K.height = 2160;
        resolution4K.refreshRate = 60;

        resolutionFHD.width = 1920;
        resolutionFHD.height = 1080;
        resolutionFHD.refreshRate = 60;

        SetResolution(resolution4K);
    }


    //TESTING RESOLUTION\
    public void SetResolution(Resolution res)
    {
        Screen.SetResolution(res.width, res.height, FullScreenMode.ExclusiveFullScreen, res.refreshRate);
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern = GameObject.FindObjectOfType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerOneCraft)
            {
                playerOneCraft.IncreaseBeamStrength();
            }
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageGeneral");
    }
}