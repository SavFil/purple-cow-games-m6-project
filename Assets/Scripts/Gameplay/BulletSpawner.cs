using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public BulletManager.BulletType bulletType = BulletManager.BulletType.Bullet1_Size1;

    public float rate = 1;
    public float speed = 10;

    private float timer = 0;

    public GameObject muzzleFlash = null;

    public bool autoFireActive = false;

    public void Shoot(int size)
    {
        if (size < 0)
        {
            return;
        }

        if (timer == 0)
        {
            Vector3 velocity = transform.up * speed;
            BulletManager.BulletType bulletToShoot = bulletType + size;
            GameManager.Instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, 0);
            muzzleFlash.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        timer++;
        if (timer >= rate)
        {
            timer = 0;
            if (muzzleFlash)
            {
                muzzleFlash.SetActive(false);
                if (autoFireActive)
                    Shoot(1);
            }
        }
    }

        public void Activate()
        {
            autoFireActive = true;
            timer = 0;
            Shoot(1);
        }

        public void DeActivate()
        {
            autoFireActive = false;
        }
}
