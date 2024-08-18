using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedChar : MonoBehaviour
{

    public enum TypeOfAnimation
    {
        Number, Hit, Bullet
    }
    [Header("Type Of Animation")]
    public TypeOfAnimation typeOfAnimation;
    public bool notHavingLoop;
    bool loopEnded;

    [Header("Animation Settings")]
    public Sprite[] charSprites;
    public SpriteRenderer spriteRenderer;
    private Image image;

    public int digit = 0;
    private int frame = 0;

    public int offset = 0;

    public int noOfCharacters;
    public int noOfFrames;

    public float FPS = 10f;
    private float timer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            image = GetComponent<Image>();
            Debug.Assert(image != null);
        }
        timer = 1f / FPS;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        int loopedFrame = (frame + offset) % noOfFrames;
        int spriteIndex = digit + (loopedFrame * noOfCharacters);
        if (spriteIndex >= 0 && spriteIndex < charSprites.Length)
        {
            if (spriteRenderer)
                spriteRenderer.sprite = charSprites[spriteIndex];
            else if (image)
                image.sprite = charSprites[spriteIndex];
        }
    }

    public virtual void Update()
    {
        if (notHavingLoop && loopEnded) return;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 1f / FPS;
            frame++;
            if (frame >= noOfFrames)
            {
                frame = 0;
                if (notHavingLoop)
                {
                    loopEnded = true;
                    EndAnimation();
                }
            }
            UpdateSprite();
        }
    }

    public void EndAnimation()
    {
        switch (typeOfAnimation)
        {
            case TypeOfAnimation.Number:
                break;
            case TypeOfAnimation.Hit:
                Destroy(gameObject);
                break;
            case TypeOfAnimation.Bullet:
                //need the bullet hit for the craft when the health system is implemented
                //trial with no hit
                Bullet bullet = gameObject.GetComponent<Bullet>();
                if (bullet != null)
                {
                    GameManager.Instance.bulletManager.DeActivateBullet(bullet.index);
                    loopEnded = false;
                }
                break;
        }
    }



}
