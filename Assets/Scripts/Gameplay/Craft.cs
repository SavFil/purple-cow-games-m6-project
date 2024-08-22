using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public CraftData craftData = new CraftData();
    Vector3 newPosition = new Vector3();

    public int playerIndex;

    public CraftConfiguration config;

    Animator animator;
    int leftBoolID;
    int rightBoolID;

    bool alive = true;
    bool invulnerable = true;
    int invulnerableTimer = 120;
    const int INVULNERABLELENGTH = 120;

    SpriteRenderer spriteRenderer = null;

    int layerMask = 0;
    int pickUpLayer = 0;

    public BulletSpawner[] bulletSpawner = new BulletSpawner[5];

    public Option[] options = new Option[4];

    public GameObject[] optionMarkersL1 = new GameObject[4];
    public GameObject[] optionMarkersL2 = new GameObject[4];
    public GameObject[] optionMarkersL3 = new GameObject[4];
    public GameObject[] optionMarkersL4 = new GameObject[4];

    public Beam beam = null;

    public GameObject bombPrefab = null;

    private enum SFXType
    {
        Shoot,
        Explode
    }

    public AudioSource audioSourceShoot;
    public AudioSource audioSourceExplode;


    int enemyBLayer = 0;
    int enemyLayer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Assert(animator);

        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);

        layerMask = ~LayerMask.GetMask("PlayerBullets") &
            ~LayerMask.GetMask("PlayerBombs") &
            ~LayerMask.GetMask("Player") &
            ~LayerMask.GetMask("GroundEnemy");

        pickUpLayer = LayerMask.NameToLayer("PickUp");

        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyBLayer = LayerMask.NameToLayer("EnemyBullets");

    }

    public void SetInvulnerable()
    {
        invulnerable = true;
        invulnerableTimer = INVULNERABLELENGTH;
    }

    private void FixedUpdate()
    {
        if (invulnerable)
        {
            if (invulnerableTimer % 12 < 6)
                spriteRenderer.material.SetColor("_Overbright", Color.black);
            else
                spriteRenderer.material.SetColor("_Overbright", Color.white);
            invulnerableTimer--;
            if (invulnerableTimer <= 0)
            {
                invulnerable = false;
                spriteRenderer.material.SetColor("_Overbright", Color.black);
            }
        }

        // Hit Detection
        int maxColliders = 10;
        Collider2D[] hits = new Collider2D[maxColliders];

        // Bullet hits
        Vector2 halfSize = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
        int noOfhits = Physics2D.OverlapBoxNonAlloc(transform.position, halfSize, 0, hits, layerMask);
        if (noOfhits > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit)
                {
                    if (hit.gameObject.layer == enemyLayer || hit.gameObject.layer == enemyBLayer)
                        Hit(hit.gameObject);
                }
            }
        }

        // Pickups and bullet grazing

        halfSize = new Vector2(15f, 21f);
        noOfhits = Physics2D.OverlapBoxNonAlloc(transform.position, halfSize, 0, hits, layerMask);
        if (noOfhits > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit)
                {
                    if (hit.gameObject.layer == pickUpLayer)
                        PickUp(hit.GetComponent<PickUp>());
                    else // Bullet graze
                        craftData.beamCharge++;
                }
            }
        }

        // Movement
        if (InputManager.Instance && alive)
        {
            craftData.positionX += InputManager.Instance.playerState[playerIndex].movement.x * config.speed;
            craftData.positionY += InputManager.Instance.playerState[playerIndex].movement.y * config.speed;

            if (craftData.positionX < -158) craftData.positionX = -158;
            if (craftData.positionX > 158) craftData.positionX = 158;

            if (craftData.positionY < -68) craftData.positionY = -68; // bottom part
            if (craftData.positionY > 68) craftData.positionY = 68;   // top part

            newPosition.x = (int)craftData.positionX;
            if (!GameManager.Instance.progressWindow)
                GameManager.Instance.progressWindow = GameObject.FindObjectOfType<LevelProgress>();
            if (GameManager.Instance.progressWindow)
                newPosition.y = (int)craftData.positionY + GameManager.Instance.progressWindow.transform.position.y;
            else
                newPosition.y = (int)craftData.positionY;
            gameObject.transform.position = newPosition;





            if (InputManager.Instance.playerState[playerIndex].left)
            {
                animator.SetBool(leftBoolID, true);
            }
            else
            {
                animator.SetBool(leftBoolID, false);
            }

            if (InputManager.Instance.playerState[playerIndex].right)
            {
                animator.SetBool(rightBoolID, true);
            }
            else
            {
                animator.SetBool(rightBoolID, false);
            }

            // Shooting bullets
            if (InputManager.Instance.playerState[playerIndex].shoot)
            {
                ShotConfiguration shotConfig = config.shotLevel[craftData.shotPower];
                PlaySFX(SFXType.Shoot);

                for (int s = 0; s < 5; s++)
                {
                    bulletSpawner[s].Shoot(shotConfig.spawnerSizes[s]);
                }
                for (int o = 0; o < craftData.noOfEnabledOptions; o++)
                {
                    if (options[o])
                    {
                        options[o].Shoot();
                    }
                }
            }

            if (InputManager.Instance.playerState[playerIndex].beam)
            {
                beam.Fire();
            }

            // Bomb
            if (!InputManager.Instance.playerPrevState[playerIndex].bomb &&
                InputManager.Instance.playerState[playerIndex].bomb)
            {
                FireBomb();
            }

            // Bullets Layouts
            if (!InputManager.Instance.playerPrevState[playerIndex].options &&
                InputManager.Instance.playerState[playerIndex].options)
            {
                craftData.optionsLayout++;
                if (craftData.optionsLayout > 3)
                {
                    craftData.optionsLayout = 0;
                }
                SetOptionsLayout(craftData.optionsLayout);
            }

        }
    }

    private void PlaySFX(SFXType sfxType)
    {
        switch (sfxType)
        {
            case SFXType.Shoot:
                if (audioSourceShoot != null && audioSourceShoot.clip != null)
                {
                    audioSourceShoot.Play();
                }
                else
                {
                    Debug.Log("Shoot sound missing.");
                }
                break;

            case SFXType.Explode:
                if (audioSourceExplode != null && audioSourceShoot.clip != null)
                {
                    audioSourceExplode.Play();
                }
                else
                {
                    Debug.Log("Explode sound missing.");
                }
                break;

            default:
                Debug.LogWarning("Unhandled SFX type.");
                break;
        }
    }

    private void FireBomb()
    {
        if (craftData.smallBombs > 0)
        {
            craftData.smallBombs--;
            Vector3 pos = transform.position;
            pos.y += 100;
            Instantiate(bombPrefab, pos, Quaternion.identity);
        }
    }

    public void PowerUp(byte powerLevel)
    {
        craftData.shotPower += powerLevel;
        if (craftData.shotPower > 8)
            craftData.shotPower = 8;
    }

    public void IncreaseScore(int value)
    {
        GameManager.Instance.playerDatas[playerIndex].score += value;
    }

    public void OneUp()
    {
        GameManager.Instance.playerDatas[playerIndex].health += 5;
        if (GameManager.Instance.playerDatas[playerIndex].health > PlayerData.MAXHEALTH)
            GameManager.Instance.playerDatas[playerIndex].health = PlayerData.MAXHEALTH;
    }

    public void AddBomb(int power)
    {
        if (power == 1)
            craftData.smallBombs++;
        else if (power == 2)
            craftData.largeBombs++;
        else
            Debug.LogError("Invalid bomb power pickup");
    }

    public void AddMedal(int level, int value)
    {
        IncreaseScore(value);
    }

    public void PickUp(PickUp pickUp)
    {
        if (pickUp)
        {
            pickUp.ProcessPickUp(playerIndex, craftData);
        }
    }

    public void Hit(GameObject hitGameObject)
    {
        if (!invulnerable)
        {
            if (alive && GameManager.Instance.playerDatas[playerIndex].health > 0)
            {
                DecreaseHealth(hitGameObject);
            }
        }
        //Explode();
    }

    private void DecreaseHealth(GameObject hitGameObject)
    {
        int damage = 0;
        if (hitGameObject.layer == enemyLayer)
        {
            if (hitGameObject.transform.root.GetComponent<Enemy>().isBoss)
            {
                GameManager.Instance.playerDatas[playerIndex].health = 0;
                Explode();
                return;
            }
            //Debug.Log("Hit//////////////////////");
            damage = hitGameObject.transform.root.GetComponent<Enemy>().hitDamage;
            Destroy(hitGameObject);
        }
        else if (hitGameObject.layer == enemyBLayer)
        {
            damage = hitGameObject.GetComponent<Bullet>().bulletDamage;
            GameManager.Instance.bulletManager.DeActivateBullet(hitGameObject.GetComponent<Bullet>().index);
        }

        GameManager.Instance.playerDatas[playerIndex].health -= damage;
        if (GameManager.Instance.playerDatas[playerIndex].health <= 0)
        {
            Explode();
        }
    }



    public void Explode()
    {
        alive = false;
        Invoke("GameIsOver", 1);
        StartCoroutine(Exploding());
    }

    IEnumerator Exploding()
    {
        Color col = Color.white;
        for (float redness = 0; redness <= 1; redness += 0.3f)
        {
            col.g = 1 - redness;
            col.b = 1 - redness;
            spriteRenderer.color = col;
            yield return new WaitForSeconds(0.1f);
        }

        EffectSystem.instance.CraftExplosion(transform.position);
        PlaySFX(SFXType.Explode);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);

        GameManager.Instance.playerCrafts[0] = null;
        Destroy(gameObject);
        yield return null;
    }


    public void GameIsOver()
    {
        //GameOverMenu.Instance.GameOver();
        WellDoneMenu.Instance.WellDone();
    }

    internal void AddOption()
    {
        if (craftData.noOfEnabledOptions < 4)
        {
            options[craftData.noOfEnabledOptions].gameObject.SetActive(true);
            craftData.noOfEnabledOptions++;
        }
    }

    public void SetOptionsLayout(int layoutIndex)
    {
        Debug.Assert(layoutIndex < 4);

        for (int o = 0; o < 4; o++)
        {
            switch (layoutIndex)
            {
                case 0:
                    options[o].gameObject.transform.position = optionMarkersL1[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL1[o].transform.rotation;
                    break;
                case 1:
                    options[o].gameObject.transform.position = optionMarkersL2[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL2[o].transform.rotation;
                    break;
                case 2:
                    options[o].gameObject.transform.position = optionMarkersL3[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL3[o].transform.rotation;
                    break;
                case 3:
                    options[o].gameObject.transform.position = optionMarkersL4[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkersL4[o].transform.rotation;
                    break;
            }
        }
    }

    public void IncreaseBeamStrength()
    {
        if (craftData.beamPower < 5)
        {
            craftData.beamPower++;
            UpdateBeam();
        }
    }

    void UpdateBeam()
    {
        beam.beamWidth = (craftData.beamPower + 2) * 8f;
    }
}

[Serializable]

public class CraftData
{
    public float positionX;
    public float positionY;

    public byte shotPower;

    public byte noOfEnabledOptions;
    public byte optionsLayout;

    public bool beamFiring;
    public byte beamPower; // power settings and width
    public byte beamCharge; // picked by charge
    public byte beamTimer; // current charge level (how much beam is left)

    public byte smallBombs;
    public byte largeBombs;
}
