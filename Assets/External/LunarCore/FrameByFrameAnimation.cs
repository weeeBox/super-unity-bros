using UnityEngine;
using System.Collections;

namespace LunarCore
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FrameByFrameAnimation : BaseBehaviour2D
    {
        private SpriteRenderer spriteRenderer;

        public Sprite[] sprites;

        public int firstFrame;
        public int lastFrame;

        public int fps = 24;

        private int currentFrame;

        private float frameTime;
        private float elaspedTime;

        protected override void OnStart()
        {
            base.OnStart();

            frameTime = 1.0f / fps;

            currentFrame = firstFrame;
            lastFrame = lastFrame == 0 ? sprites.Length - 1 : lastFrame;

            spriteRenderer = GetRequiredComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[currentFrame];
        }

        protected override void OnUpdate(float deltaTime)
        {
            elaspedTime += deltaTime;
            if (elaspedTime >= frameTime)
            {
                ++currentFrame;
                if (currentFrame > lastFrame)
                {
                    currentFrame = firstFrame;
                }

                spriteRenderer.sprite = sprites[currentFrame];
                elaspedTime -= frameTime;
            }
        }
    }
}
