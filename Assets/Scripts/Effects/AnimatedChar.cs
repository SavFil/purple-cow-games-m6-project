using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;

public class AnimatedChar : MonoBehaviour
{
    public Sprite[] charSprites;
    public SpriteRenderer spriteRenderer;

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
        Debug.Assert(spriteRenderer != null);
        timer = 1f / FPS;
        UpdateSprite(0);
    }

    public void UpdateSprite(int newFrame)
    {
        int loopedFrame = (newFrame + offset) % noOfFrames;
        int spriteIndex = digit + (loopedFrame * noOfCharacters);
        spriteRenderer.sprite = charSprites[spriteIndex];
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 1f / FPS;
            frame++;
            if (frame>=noOfFrames)
            {
                frame = 0;
            }
            UpdateSprite(frame);
        }
    }
}
