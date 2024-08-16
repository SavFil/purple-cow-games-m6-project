using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

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

    const int MAX_BULLET_PER_TYPE = 50;
    const int MAX_BULLET_COUNT = MAX_BULLET_PER_TYPE * (int)BulletType.MAX_TYPES;
    private Bullet[] bullets = new Bullet[MAX_BULLET_COUNT];
    private NativeArray<BulletData> bulletData;
    private TransformAccessArray bulletTransforms;

    ProcessBulletJob jobProcessor;

    void Start()
    {
        bulletData = new NativeArray<BulletData>(MAX_BULLET_COUNT, Allocator.Persistent);
        bulletTransforms = new TransformAccessArray(MAX_BULLET_COUNT);

        int index = 0;
        for (int bulletType = (int)BulletType.Bullet1_Size1; bulletType < (int)BulletType.MAX_TYPES; bulletType++)
        {
            for (int b = 0; b < MAX_BULLET_PER_TYPE; b++)
            {
                Bullet newBullet = Instantiate(bulletPrefabs[bulletType]).GetComponent<Bullet>();
                newBullet.index = index;
                newBullet.gameObject.SetActive(false);
                newBullet.transform.SetParent(transform);
                bullets[index] = newBullet;
                bulletTransforms.Add(bullets[index].transform);
                index++;
            }
        }
        jobProcessor = new ProcessBulletJob { bullets = bulletData };
    }
    private void OnDestroy()
    {
        bulletData.Dispose();
        bulletTransforms.Dispose();
    }

    private int NextFreeBulletIndex(BulletType type)
    {
        int startIndex = (int)type * MAX_BULLET_PER_TYPE;
        for (int b = 0; b < MAX_BULLET_PER_TYPE; b++)
        {
            if (!bulletData[startIndex + b].active)
                return startIndex + b;
        }
        return -1;
    }

    public Bullet SpawnBullet(BulletType type, float x, float y, float dX, float dY, float angle, float dAngle, bool homing)
    {
        int bulletIndex = NextFreeBulletIndex(type);
        if (bulletIndex > -1)
        {
            Bullet result = bullets[bulletIndex];
            result.gameObject.SetActive(true);
            bulletData[bulletIndex] = new BulletData(x, y, dX, dY, angle, dAngle, (int)type, true, homing);
            bullets[bulletIndex].gameObject.transform.position = new Vector3(x, y, 0);
            return result;
        }
        return null;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance && GameManager.Instance.playerOneCraft)
        {
            jobProcessor.player1Position = GameManager.Instance.playerOneCraft.transform.position;
        }
        else
        {
            jobProcessor.player1Position = new Vector2(-9999, -9999);
        }

        if (GameManager.Instance && GameManager.Instance.progressWindow)
        {
            jobProcessor.progessY = GameManager.Instance.progressWindow.transform.position.y;
        }
        else
        {
            jobProcessor.progessY = 0;
        }

        ProcessBullets();

        for (int b = 0; b < MAX_BULLET_COUNT; b++)
        {
            if (!bulletData[b].active)
                bullets[b].gameObject.SetActive(false);
        }
    }

    void ProcessBullets()
    {
        JobHandle handler = jobProcessor.Schedule(bulletTransforms);
        handler.Complete();
    }

    public void DeActivateBullet(int index)
    {
        bullets[index].gameObject.SetActive(false);

        float x = bulletData[index].positionX;
        float y = bulletData[index].positionY;
        float dX = bulletData[index].dX;
        float dY = bulletData[index].dY;
        float angle = bulletData[index].angle;
        float dAngle = bulletData[index].dAngle;
        int type = bulletData[index].type;
        bool homing = bulletData[index].homing;
        bulletData[index] = new BulletData(x, y, dX, dY, angle, dAngle, type, false, homing);
    }

    public struct ProcessBulletJob : IJobParallelForTransform
    {
        public NativeArray<BulletData> bullets;
        public Vector2 player1Position;
        public float progessY;
        public void Execute(int index, TransformAccess transform)
        {
            bool active = bullets[index].active;
            if (!active) return;

            float dX = bullets[index].dX;
            float dY = bullets[index].dY;
            float x = bullets[index].positionX;
            float y = bullets[index].positionY;
            float angle = bullets[index].angle;
            float dAngle = bullets[index].dAngle;
            int type = bullets[index].type;
            bool homing = bullets[index].homing;


            // Homing
            if (player1Position.y < -1000)
            {
                active = false;
            }
            else if (homing)
            {
                Vector2 velocity = new Vector2(dX, dY);
                float speed = velocity.magnitude;
                Vector2 toPlayer= new Vector2(player1Position.x - x, player1Position.y - y);
                Vector2 newVelocity = Vector2.Lerp(velocity.normalized, toPlayer.normalized, 0.05f).normalized;
                newVelocity *= speed;
                dX = newVelocity.x;
                dY = newVelocity.y;
            }

            // Movement
            x = x + dX;
            y = y + dY;

            // Check for out of bounds - numbers might need adgustment
            if (x < -320) active = false;
            if (x > 320) active = false;
            if (y - progessY < -300) active = false;
            if (y-progessY > 180) active = false;

            bullets[index] = new BulletData(x, y, dX, dY, angle, dAngle, type, active, homing);

            if (active)
            {
                Vector3 newPosition = new Vector3(x, y, 0);
                transform.position = newPosition;

                // Facing rotation
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(dX, dY, 0));
            }
        }
    }
}

