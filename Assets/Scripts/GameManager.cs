using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private GameVersionSO gameVersionSO;
    [Tooltip("The version of the game.")]
    [SerializeField] private string currentVersion;
    public bool twoPlayer = false;

    public GameObject[] craftPrefabs;

    // public Craft playerOneCraft = null;
    public Craft[] playerCrafts = new Craft[2];

    public PlayerData[] playerDatas;

    public BulletManager bulletManager = null;

    public LevelProgress progressWindow = null;

    public Session gameSession = new Session();

    public PickUp[] cyclicDrops = new PickUp[15];
    public PickUp[] medals = new PickUp[10];
    private int currentDropIndex = 0;
    private int currentMedalIndex = 0;

    public HUD hud = null;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 GameManager!");
            Destroy(gameObject);
            return;
        }

        playerDatas = new PlayerData[2];
        playerDatas[0] = new PlayerData();
        playerDatas[1] = new PlayerData();

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //Debug.Log("GameManager Created!");

        currentVersion = gameVersionSO.Version;

        bulletManager = GetComponent<BulletManager>();

        Application.targetFrameRate = 60;

    }





    public void SpawnPlayer(int playerIndex, int craftType)
    {
        Debug.Assert(craftType < craftPrefabs.Length);
        Debug.Log("Spawning player " + playerIndex);
        playerCrafts[playerIndex] = Instantiate(craftPrefabs[craftType]).GetComponent<Craft>();
        playerCrafts[playerIndex].playerIndex = playerIndex;
    }

    public void ResetData()
    {
        playerDatas[0].health = 15;
        playerDatas[0].score = 0;
    }


    //Debug code for testing purposes.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!playerCrafts[0]) SpawnPlayer(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].AddOption();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerCrafts[0] && playerCrafts[0].craftData.shotPower < CraftConfiguration.MAX_SHOT_POWER - 1)
            {
                playerCrafts[0].craftData.shotPower++;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern = GameObject.FindObjectOfType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].IncreaseBeamStrength();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StageKiddiePool");
        StartCoroutine(TogleHud(0, true));
    }


    IEnumerator TogleHud(int playerIndex, bool active)
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log("hud1");
        while (!SceneManager.GetSceneByName("StageKiddiePool").isLoaded && !hud)
        {
            Debug.Log("hud2");
            yield return null;
        }
        hud.playerHUDs[playerIndex].healthBG.SetActive(active);
        hud.playerScore[playerIndex].gameObject.SetActive(active);
    }



    public void PickUpFallOffScreen(PickUp pickup)
    {
        if (pickup.config.type == PickUp.PickUpType.Medal)
        {
            currentMedalIndex = 0;
        }
    }

    public PickUp GetNextDrop()
    {
        PickUp result = cyclicDrops[currentDropIndex];

        if (result.config.type == PickUp.PickUpType.Medal)
        {
            result = medals[currentMedalIndex];
            currentMedalIndex++;
            if (currentMedalIndex > 9)
                currentMedalIndex = 0;
        }

        currentDropIndex++;
        if (currentDropIndex > 14)
            currentDropIndex = 0;

        return result;
    }
}