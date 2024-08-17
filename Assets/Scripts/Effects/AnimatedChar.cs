using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedChar : MonoBehaviour
{
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

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 1f / FPS;
            frame++;
            if (frame >= noOfFrames)
            {
                frame = 0;
            }
            UpdateSprite();
        }
    }
}
