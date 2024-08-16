using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public int health = 10;
    public int radiusOrWidth = 10;
    public float height = 10;
    public bool box = false;

    private int layerMask = 0;

    private Vector2 halfExtent;

    public bool damagedByBullets = true;
    public bool damagedByBeams = true;
    public bool damagedByBombs = true;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("GroundEnemy") & ~LayerMask.GetMask("EnemyBullets");

        halfExtent = new Vector3(radiusOrWidth / 2, height / 2, 1);
    }
    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider[] hits = new Collider[maxColliders];
        int noOfHits = 0;
        if (box)
        {
            noOfHits = Physics.OverlapBoxNonAlloc(transform.position, halfExtent, hits, transform.rotation, layerMask);
        }
        else
        {
            noOfHits = Physics.OverlapSphereNonAlloc(transform.position, radiusOrWidth, hits, layerMask);
        }

        if (noOfHits > 0)
        {
            for (int h = 0; h < noOfHits; h++)
            {
                if (damagedByBullets)
                {
                    Bullet bullet = hits[h].GetComponent<Bullet>();
                    if (bullet != null)
                    {
                        TakeDamage(1);
                        GameManager.Instance.bulletManager.DeActivateBullet(bullet.index);
                    }
                }
                if (damagedByBombs)
                {
                    Bomb bomb = hits[h].GetComponent<Bomb>();
                    if (bomb != null)
                    {
                        TakeDamage(bomb.power);
                    }
                }
            }
        }
    }

    public void TakeDamage(int ammount)
    {
        health -= ammount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
