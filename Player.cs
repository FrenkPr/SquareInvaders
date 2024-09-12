using Aiv.Draw;

namespace SquareInvaders
{
    class Player
    {
        private Vector2 pos;
        private Vector2 moveSpeed;
        Sprite player;
        private int numLives;

        public Player()
        {
            player = new Sprite("Assets/player.png");
            pos = new Vector2(Gfx.Window.Width * 0.5f - player.Width * 0.5f, 500);
            moveSpeed = new Vector2(500, 0);
            numLives = 3;
        }

        public void Move()
        {
            if (Gfx.Window.GetKey(KeyCode.Left))
            {
                pos.Sub(moveSpeed.Scale(Gfx.Window.DeltaTime));
            }

            if (Gfx.Window.GetKey(KeyCode.Right))
            {
                pos.Add(moveSpeed.Scale(Gfx.Window.DeltaTime));
            }
        }

        public void Shoot()
        {
            int freeIndex = BulletMngr.GetFreePlayerBulletIndex();
            Bullet[] bullets = BulletMngr.GetPlayerBullets();
            Vector2 bulletPos;
            Vector2 prevBulletPos;

            if (freeIndex != -1)
            {
                if (freeIndex == 0)
                {
                    if (Gfx.Window.GetKey(KeyCode.Up))
                    {
                        bulletPos.X = pos.X + player.Width * 0.5f - bullets[freeIndex].GetWidth() * 0.5f;
                        bulletPos.Y = pos.Y - bullets[freeIndex].GetHeight();

                        bullets[freeIndex].isShot = true;
                        bullets[freeIndex].pos = bulletPos;

                        BulletMngr.IncPlayerIndexToShoot();
                    }
                }

                else
                {
                    prevBulletPos = bullets[freeIndex - 1].pos;

                    if (Gfx.Window.GetKey(KeyCode.Up) && prevBulletPos.Y <= 400)
                    {
                        bulletPos.X = pos.X + player.Width * 0.5f - bullets[freeIndex].GetWidth() * 0.5f;
                        bulletPos.Y = pos.Y - bullets[freeIndex].GetHeight();

                        bullets[freeIndex].isShot = true;
                        bullets[freeIndex].pos = bulletPos;

                        BulletMngr.IncPlayerIndexToShoot();
                    }
                }
            }
        }

        public void CheckBoundsCollision()
        {
            //horizontal collisions
            if (pos.X < 30)
            {
                pos.X = 30;
            }

            else if (pos.X + player.Width > Gfx.Window.Width - 31)
            {
                pos.X = (Gfx.Window.Width - 31) - player.Width;
            }
        }

        public void Draw()
        {
            Gfx.DrawSprite(player, (int)pos.X, (int)pos.Y);
        }

        public void PlayerCollidedWithBullet()
        {
            AlienBullet[] alienBullets = BulletMngr.GetAlienBullets();

            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (CollidedWithBullet(alienBullets[i].pos, alienBullets[i].GetHeight(), alienBullets[i].GetWidth()))
                {
                    numLives--;
                    alienBullets[i].isShot = false;
                    alienBullets[i].pos = new Vector2(alienBullets[i].pos.X, 700);  //changes y position to don't let come game over once you've been hit for the first time

                    break;
                }
            }
        }

        public bool CollidedWithBullet(Vector2 bulletPos, int bulletHeight, int bulletWidth)
        {
            return bulletPos.Y + bulletHeight >= pos.Y && bulletPos.Y + bulletHeight <= pos.Y + player.Height && bulletPos.X + bulletWidth >= pos.X && bulletPos.X <= pos.X + player.Width;
        }

        public void PlayerCollidedWithAlien()
        {
            Alien[] aliensAtBottom = AlienManager.GetAliensAtBottom();

            for (int i = 0; i < aliensAtBottom.Length; i++)
            {
                if (aliensAtBottom[i] != null)
                {
                    if (CollidedWithAlien(aliensAtBottom[i].pos, aliensAtBottom[i].GetHeight()))
                    {
                        numLives = 0;

                        break;
                    }
                }
            }
        }

        public bool CollidedWithAlien(Vector2 alienPos, int alienHeight)
        {
            return alienPos.Y + alienHeight >= pos.Y;
        }

        public int GetNumLives()
        {
            return numLives;
        }
    }
}
