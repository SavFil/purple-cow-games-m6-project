using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public BulletManager.BulletType bulletType = BulletManager.BulletType.Bullet1_Size1;

    public BulletSequence sequence;

    public int rate = 1;
    public int speed = 10;
    private int timer = 0;

    public float startAngle = 0;
    public float endAngle = 0;
    public int radialNumber = 1;

    public float dAngle = 0;


    public GameObject muzzleFlash = null;

    public bool autoFireActive = false;
    private bool firing = false;
    private int frame = 0;

    public bool fireAtPlayer = false;
    public bool fireAtTarget = false;
    public GameObject target = null;

    public bool homing = false;

    public bool isPlayer = false;



    public float height;
    public float width;



    public void Shoot(int size)
    {
        if (size < 0) return;


        //?////////////////////////////////////Screen Adjustment/////////////////////////////////////////////
        //!/bullets for enemies
        //!/New way 
        float x = transform.position.x;

        if (GameManager.Instance && GameManager.Instance.progressWindow)
        {

            x -= GameManager.Instance.progressWindow.data.positionX;
            if (x < -width || x > width)
            {
                //Debug.Log("x = " + x);
                return;
            }
        }

        float y = transform.position.y;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
        {
            y -= GameManager.Instance.progressWindow.data.positionY;
            if (y < -height || y > height)
            {
                return;
            }
        }


        Vector2 primaryDirection = transform.up;

        if (fireAtPlayer || fireAtTarget)
        {
            Vector2 targetPosition = Vector2.zero;
            if (fireAtPlayer)
            {
                targetPosition = GameManager.Instance.playerCrafts[0].transform.position;
            }
            else if (fireAtTarget && target != null)
            {
                targetPosition = target.transform.position;
            }

            primaryDirection = targetPosition - (Vector2)transform.position;
            primaryDirection.Normalize();
        }

        if (firing || timer == 0)
        {
            float angle = startAngle;
            for (int a = 0; a < radialNumber; a++)
            {
                Quaternion myRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 velocity = myRotation * primaryDirection * speed;

                BulletManager.BulletType bulletToShoot = bulletType; // size seems to incorrectly increment the bullet type to +1 of what it originally is + size;
                GameManager.Instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, angle, dAngle, homing);

                angle = angle + ((endAngle - startAngle) / (radialNumber - 1));
            }
            if (muzzleFlash)
            {
                muzzleFlash.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {

        height = GameManager.Instance.progressWindow.progressCamera.orthographicSize;
        width = (height * (GameManager.Instance.progressWindow.progressCamera.aspect / 2)) + 30;


        timer++;
        if (timer >= rate)
        {
            timer = 0;
            if (muzzleFlash)
            {
                muzzleFlash.SetActive(false);
            }
            if (autoFireActive)
            {
                firing = true;
                frame = 0;
            }
        }

        if (firing)
        {
            if (sequence.ShouldFire(frame))
            {
                Shoot(1);
            }

            frame++;
            if (frame > sequence.totalFrames)
            {
                firing = false;
            }
        }
    }

    public void Activate()
    {
        autoFireActive = true;
        timer = 0;
        frame = 0;
        firing = true;
    }

    public void DeActivate()
    {
        autoFireActive = false;
    }
}

[Serializable]
public class BulletSequence
{
    public List<int> emmitFrames = new List<int>();
    public int totalFrames;

    public bool ShouldFire(int currentFrame)
    {
        foreach (int frame in emmitFrames)
        {
            if (currentFrame == frame)
            {
                return true;
            }
        }
        return false;
    }
}
