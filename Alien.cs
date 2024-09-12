

namespace SquareInvaders
{
    class Alien
    {
        public Vector2 pos;
        private int width;
        private int height;
        private static float speedX;
        private static float speedY;
        private bool isAlive;
        private bool drawExplosion;
        private Color color;
        private int pixelsPerRow;
        private int pixelsPerCol;
        private Pixel[] pixels;
        private byte[,] pixelsToDraw;

        public Alien(Vector2 pos, int width, int height)
        {
            //bytes set to 1 are pixels to draw, while 0 the opposite
            pixelsToDraw = new byte[,] {{0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0},
                                        {0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                                        {0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0},
                                        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1},
                                        {1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1},
                                        {0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0}};
            this.pos = pos;
            this.width = width;
            this.height = height;
            speedX = 100;
            speedY = 1000;
            isAlive = true;
            drawExplosion = false;
            color = new Color(0, 255, 0);
            pixelsPerRow = 8;
            pixelsPerCol = 11;

            int numPixels = CountPixelsToDraw(pixelsToDraw);
            pixels = new Pixel[numPixels];
            InitAlienSprite();
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void SetAlive(bool val)
        {
            isAlive = val;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public static void ChangeXDir()
        {
            speedX *= -1;
        }

        public void MoveX()
        {
            if (isAlive)
            {
                pos.Add(new Vector2(speedX * Gfx.Window.DeltaTime, 0.0f));
                TranslateAllPixels(speedX * Gfx.Window.DeltaTime, 0);
            }
        }

        public void MoveY()
        {
            if (isAlive)
            {
                pos.Add(new Vector2(0.0f, speedY * Gfx.Window.DeltaTime));
                TranslateAllPixels(0, speedY * Gfx.Window.DeltaTime);
            }
        }

        public void TranslateAllPixels(float x, float y)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].Translate(x, y);
            }
        }

        private void InitAlienSprite()
        {
            int pixelWidth = width / pixelsPerCol;
            int pixelHeight = height / pixelsPerRow;
            float startPosX = pos.X;
            float posPixelX = pos.X;
            float posPixelY = pos.Y;
            int pixelIndex = 0;

            for (int i = 0; i < pixelsPerRow; i++)
            {
                for (int j = 0; j < pixelsPerCol; j++)
                {
                    if (pixelsToDraw[i, j] == 1)
                    {
                        pixels[pixelIndex] = new Pixel(new Vector2(posPixelX, posPixelY), pixelWidth, color);
                        pixelIndex++;
                    }

                    posPixelX += pixelWidth;
                }

                posPixelX = startPosX;
                posPixelY += pixelHeight;
            }
        }

        public void Draw()
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                if (isAlive || drawExplosion)
                {
                    pixels[i].Draw();
                }
            }
        }

        public void UpdateExplosion()
        {
            if (drawExplosion)
            {
                for (int i = 0; i < pixels.Length; i++)
                {
                    if (!pixels[i].isOutOfScreen)
                    {
                        pixels[i].Update();
                    }
                }
            }

            if (AllPixelsOutOfScreen())
            {
                drawExplosion = false;
            }
        }

        private int CountPixelsToDraw(byte[,] pixels)
        {
            int count = 0;

            for (int i = 0; i < pixelsPerRow; i++)
            {
                for (int j = 0; j < pixelsPerCol; j++)
                {
                    if (pixels[i, j] == 1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public bool Collided(Vector2 bulletPos)
        {
            Vector2 dist = bulletPos;
            dist.Sub(new Vector2(pos.X, pos.Y + height));  //subtraction between bullet and alien pos

            return dist.GetVectorLength() <= height;
        }

        public void OnHit()
        {
            isAlive = false;
            drawExplosion = true;

            Explode();
        }

        private bool AllPixelsOutOfScreen()
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                if (!pixels[i].isOutOfScreen)
                {
                    return false;
                }
            }

            return true;
        }

        private void Explode()
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                //v = pixel position - Alien position
                Vector2 pixelVel = pixels[i].GetPosition();
                pixelVel.Sub(new Vector2(pos.X + width * 0.5f, pos.Y + height * 0.5f));

                pixelVel.X *= RandomGenerator.GetRandomInt(4, 15);
                pixelVel.Y *= RandomGenerator.GetRandomInt(4, 23);
                pixelVel.Mul(4);
                pixels[i].Velocity = pixelVel;
            }
        }
    }
}
