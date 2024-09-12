using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;

namespace SquareInvaders
{
    class Animation
    {
        private Sprite[] frames;
        private int currentFrameIndex;
        private float frameDuration;
        private float counter;
        private int numFrames;
        private bool isPlaying;
        private bool loop;

        public Sprite CurrentSprite;

        public Animation(string[] sprites, float fps)
        {
            numFrames = sprites.Length;
            frames = new Sprite[numFrames];

            for (int i = 0; i < numFrames; i++)
            {
                frames[i] = new Sprite(sprites[i]);
            }

            currentFrameIndex = 0;
            CurrentSprite = frames[currentFrameIndex];

            if(fps <= 0)
            {
                fps = 1;
            }

            frameDuration = 1 / fps;

            isPlaying = true;
            loop = true;
        }

        public void Update()
        {
            if(isPlaying)
            {
                counter += Gfx.Window.DeltaTime;

                if(counter >= frameDuration)
                {
                    currentFrameIndex++;

                    if(currentFrameIndex >= numFrames)
                    {
                        if(loop)
                        {
                            currentFrameIndex = 0;
                        }
                        else
                        {
                            currentFrameIndex = numFrames - 1;
                            isPlaying = false;
                        }
                    }

                    counter = 0;
                    CurrentSprite = frames[currentFrameIndex];
                }
            }
        }

        public bool IsPlaying()
        {
            return isPlaying;
        }

        public void SetLoop(bool value)
        {
            loop = value;
        }

        public bool GetLoop()
        {
            return loop;
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
            currentFrameIndex = 0;
            CurrentSprite = frames[currentFrameIndex];
        }

        public void Pause()
        {
            isPlaying = false;
        }
    }
}
