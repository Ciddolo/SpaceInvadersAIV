using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Animation
    {
        Sprite[] sprites;
        SpriteObj owner;
        int numFrames;
        float frameDuration;
        int currentFrameIndex;
        float counter;

        public bool Loop { get; set; }
        public bool IsPlaying { get; private set; }

        private int currentFrame
        {
            get
            {
                return currentFrameIndex;
            }
            set
            {
                currentFrameIndex = value;

                if (currentFrameIndex >= numFrames)
                    OnAnimationEnds();
                else
                    owner.SetSprite(sprites[currentFrameIndex]);
            }
        }


        public Animation(string[] files, SpriteObj animationOwner, float fps)
        {
            Loop = true;
            IsPlaying = true;
            numFrames = files.Length;
            owner = animationOwner;

            sprites = new Sprite[numFrames];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = new Sprite(files[i]);
            }

            owner.SetSprite(sprites[0]);

            if (fps > 0.0f)
                frameDuration = 1 / fps;
            else
                frameDuration = 0.0f;
        }

       

        private void OnAnimationEnds()
        {
            if (Loop)
                currentFrame = 0;
            else
                Pause();
        }

        public void Update()
        {

            if (owner != null && IsPlaying)
            {
                if (frameDuration != 0.0f)
                {
                    counter += Game.DeltaTime;

                    if (counter >= frameDuration)
                    {
                        counter = 0;
                        currentFrame = (currentFrame + 1);// % numFrames;
                    }
                }
                else
                {
                    owner.SetSprite(sprites[currentFrame]);
                }

            }
        }

        public void Start()
        {
            IsPlaying = true;
        }

        public void Restart()
        {
            currentFrame = 0;
            IsPlaying = true;
        }

        public void Stop()
        {
            currentFrame = 0;
            IsPlaying = false;
        }
        public void Pause()
        {
            IsPlaying = false;
        }
    }
}
