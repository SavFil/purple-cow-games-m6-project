using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Bullet[] bulletPrefabs;

    public enum BulletType
    {
        Bullet1_Size1,
        Bullet1_Size2,
        Bullet1_Size3,
        Bullet1_Size4,
        Bullet1_Size5,
        Bullet2_Size1,
        Bullet2_Size2,
        Bullet2_Size3,
        Bullet2_Size4,
        Bullet3_Size1,
        Bullet3_Size2,
        Bullet3_Size3,
        Bullet3_Size4,
        Bullet4_Size1,
        Bullet4_Size2,
        Bullet4_Size3,
        Bullet4_Size4,
        Bullet5_Size1,
        Bullet5_Size2,
        Bullet5_Size3,
        Bullet5_Size4,
        Bullet6_Size1,
        Bullet6_Size2,
        Bullet6_Size3,
        Bullet6_Size4,
        Bullet6_Size5,
        Bullet6_Size6,
        Bullet6_Size7,
        Bullet6_Size8,
        Bullet7_Size1,
        Bullet7_Size2,
        Bullet7_Size3,
        Bullet7_Size4,
        Bullet8_Size1,
        Bullet8_Size2,
        Bullet8_Size3,
        Bullet8_Size4,
        Bullet9_Size1,
        Bullet9_Size2,
        Bullet9_Size3,
        Bullet9_Size4,
        Bullet10_Size1,
        Bullet10_Size2,
        Bullet10_Size3,
        Bullet10_Size4,
        Bullet11_Size1,
        Bullet11_Size2,
        Bullet11_Size3,
        Bullet11_Size4,
        Bullet11_Size5,
        Bullet11_Size6,
        Bullet11_Size7,
        Bullet11_Size8,
        Bullet11_Size9,
        Bullet11_Size10,
        Bullet11_Size11,
        Bullet11_Size12,
        Bullet11_Size13,
        Bullet11_Size14,
        Bullet11_Size15,
        Bullet11_Size16,
        Bullet12_Size1,
        Bullet12_Size2,
        Bullet12_Size3,
        Bullet12_Size4,
        Bullet12_Size5,
        Bullet12_Size6,
        Bullet12_Size7,
        Bullet12_Size8,
        Bullet13_Size1,
        Bullet13_Size2,
        Bullet13_Size3,
        Bullet13_Size4,
        Bullet14_Size1,
        Bullet14_Size2,
        Bullet14_Size3,
        Bullet14_Size4,
        Bullet15_Size1,
        Bullet15_Size2,
        Bullet15_Size3,
        Bullet16_Size1,
        Bullet16_Size2,
        Bullet16_Size3,
        MAX_TYPES,
    }

    const int MAX_BULLET_PER_TYPE = 500;
    const int MAX_BULLET_COUNT    = MAX_BULLET_PER_TYPE * (int)BulletType.MAX_TYPES;
    private Bullet[] bullets      = new Bullet[MAX_BULLET_COUNT];

    void Start()
    {
        int index = 0;
        for (int bulletType = (int)BulletType.Bullet1_Size1; bulletType<(int)BulletType.MAX_TYPES; bulletType++)
        {
            for (int b=0;b<MAX_BULLET_PER_TYPE;b++)
            {
                Bullet newBullet = Instantiate(bulletPrefabs[bulletType]).GetComponent<Bullet>();
                newBullet.gameObject.SetActive(false);
                newBullet.transform.SetParent(transform);
                bullets[index] = newBullet;
                index++;

            }
        }
        
    }
}

