using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public LineRenderer lineRenderer = null;
    public float beamWidth = 20;
    public Craft craft = null;
    private int layerMask = 0;

    public GameObject beamFlash = null;
    public GameObject[] beamHits = new GameObject[5];

    const int MINIMUMCHARGE = 10;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("PlayerBullets");
    }
    public void Fire()
    {
        if (!craft.craftData.beamFiring)
        {
            if (craft.craftData.beamCharge > MINIMUMCHARGE)
            {
                craft.craftData.beamFiring = true;
                craft.craftData.beamTimer = craft.craftData.beamCharge;
                craft.craftData.beamCharge = 0;
                UpdateBeam();
                float scale = beamWidth / 30;
                beamFlash.transform.localScale = new Vector3(scale, scale, 1);
                gameObject.SetActive(true);
                beamFlash.SetActive(true);
            }
            else
            {
                UpdateBeam();
            }
        }
    }

    private void FixedUpdate()
    {
        if (craft.craftData.beamFiring)
        {
            UpdateBeam();
        }
    }

    void UpdateBeam()
    {
        if (craft.craftData.beamTimer > 0) craft.craftData.beamTimer--;
        if (craft.craftData.beamTimer == 0)
        {
            craft.craftData.beamFiring = false;
            HideHits();
            beamFlash.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        float topY = 180;
        if (GameManager.Instance.progressWindow)
        {
            topY += GameManager.Instance.progressWindow.transform.position.y;
        }

        int maxColliders = 20;
        Collider2D[] hits = new Collider2D[maxColliders];
        float middleY = (craft.transform.position.y + topY) * .5f;
        Vector2 halfSize = new Vector2(beamWidth * .5f, (topY - craft.transform.position.y) * .5f);
        Vector3 center = new Vector3(craft.transform.position.x, middleY, 0);
        int noOfHits = Physics2D.OverlapBoxNonAlloc(center, halfSize, 0, hits, layerMask);
        float lowest = topY;
        Shootable lowestShootable = null;
        Collider2D lowestCollider = null;
        const int MAXRAYHITS = 10;
        if (noOfHits > 0)
        {
            // Find lowest hit
            for (int hit = 0; hit < noOfHits; hit++)
            {
                Shootable shootable = hits[hit].GetComponent<Shootable>();

                if (shootable && shootable.damagedByBeams)
                {
                    RaycastHit2D[] hitInfo = new RaycastHit2D[MAXRAYHITS];
                    Vector2 ray = Vector3.up;
                    float height = topY - craft.transform.position.y;
                    if (hits[hit].Raycast(ray, hitInfo, height) > 0)
                    {
                        if (hitInfo[0].point.y < lowest)
                        {
                            lowest = hitInfo[0].point.y;
                            lowestShootable = hits[hit].GetComponent<Shootable>();
                            lowestCollider = hits[hit];
                        }
                    }
                }
            }

            // Find hits on Collider
            if (lowestShootable != null)
            {
                // fire 5 rays to find each hit
                Vector3 start = craft.transform.position;
                start.x -= (beamWidth / 5);

                for (int h = 0; h < 5; h++)
                {
                    RaycastHit2D[] hitInfo = new RaycastHit2D[MAXRAYHITS];
                    Vector2 ray = Vector3.up;
                    if (lowestCollider.Raycast(ray, hitInfo, 360) > 0)
                    {
                        Vector3 pos = hitInfo[0].point;
                        pos.x += Random.Range(-3f, 3f);
                        pos.y += Random.Range(-3f, 3f);
                        beamHits[h].transform.position = pos;
                        beamHits[h].SetActive(true);
                        lowestShootable.TakeDamage(craft.craftData.beamPower + 1);
                    }
                    else
                    {
                        beamHits[h].SetActive(false);
                    }
                    start.x += (beamWidth / 5);
                }
            }
            else
            {

                HideHits();
            }
        }
        else
        {
            HideHits();
        }

        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;

        lineRenderer.SetPosition(0, transform.position);
        Vector3 top = transform.position;
        top.y = lowest;
        lineRenderer.SetPosition(1, top);
    }

    private void HideHits()
    {
        for (int h = 0; h < 5; h++)
        {
            beamHits[h].SetActive(false);
        }
    }
}
