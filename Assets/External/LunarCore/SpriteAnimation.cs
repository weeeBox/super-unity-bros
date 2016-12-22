using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using LunarPlugin;

using Assert = LunarPlugin.CAssert;

public delegate void SpriteAnimationDelegate(SpriteAnimation anim);

public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public int frameRate;

    public bool looped;
    public bool pingPong;
    public bool destroyWhenFinished = true;

    public SpriteAnimationDelegate finishDelegate;

    private SpriteRenderer spriteRenderer;

    private int frameIndex;
    private float frameTime;
    private float frameElasped;

    private bool increasingFrames;
    private bool running;

    void Start()
    {
        Assert.IsNotNull(sprites);

        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(spriteRenderer);

        frameTime = 1.0f / frameRate;

        increasingFrames = true;
        running = sprites.Length > 1;

        spriteRenderer.sprite = sprites[0];
    }

    void Update()
    {
        if (running)
        {
            frameElasped += Time.deltaTime;
            if (frameElasped >= frameTime)
            {
                frameElasped = 0;

                if (increasingFrames)
                {
                    if (frameIndex < sprites.Length - 1)
                    {
                        ++frameIndex;
                    }
                    else
                    {
                        if (looped)
                        {
                            if (pingPong)
                            {
                                frameIndex = sprites.Length - 2;
                                increasingFrames = false;
                            }
                            else
                            {
                                frameIndex = 0;
                            }
                        }
                        else
                        {
                            running = false;
                        }
                    }
                }
                else
                {
                    if (frameIndex > 0)
                    {
                        --frameIndex;
                    }
                    else
                    {
                        if (looped)
                        {
                            if (pingPong)
                            {
                                frameIndex = 1;
                                increasingFrames = true;
                            }
                            else
                            {
                                frameIndex = sprites.Length - 1;
                            }
                        }
                        else
                        {
                            running = false;
                        }
                    }
                }

                spriteRenderer.sprite = sprites[frameIndex];
            }

            if (!running)
            {
                if (finishDelegate != null)
                {
                    finishDelegate(this);
                }
                if (destroyWhenFinished)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}