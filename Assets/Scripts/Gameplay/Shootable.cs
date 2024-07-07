using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public int health = 10;
    private int radius = 10;

    private int layerMask = 0;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("EnemyBullets");
    }
    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider[] hits = new Collider[maxColliders];
        int noOfHits = Physics.OverlapSphereNonAlloc(transform.position, radius, hits, layerMask);
        if (noOfHits > 0)
        {
            for (int h = 0; h < noOfHits; h++)
            {
                Bullet bullet = hits[h].GetComponent<Bullet>();
                if (bullet != null)
                {
                    TakeDamage(1);
                    GameManager.Instance.bulletManager.DeActivateBullet(bullet.index);
                }
                else
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
