using System;

namespace SquareInvaders
{
    static class AlienManager
    {
        private static Alien[,] aliens;
        private static Vector2[,] recentPositions;

        public static void InitAliens(int aliensPerRow, int aliensPerCol, int width, int height)
        {
            float x = 30;
            float y = 10;
            float distX = 10;
            float distY = 10;

            aliens = new Alien[aliensPerRow, aliensPerCol];
            recentPositions = new Vector2[aliensPerRow, aliensPerCol];

            for (int i = 0; i < aliensPerRow; i++)
            {
                for (int j = 0; j < aliensPerCol; j++)
                {
                    aliens[i, j] = new Alien(new Vector2(x, y), width, height);
                    x += width + distX;
                }

                y += height + distY;
                x = 30;
            }
        }

        public static void CheckBoundsCollision()
        {
            string indexesAllToLeft = GetAlienIndexesAllToLeft();
            string indexesAllToRight = GetAlienIndexesAllToRight();

            int iLeft = int.Parse(indexesAllToLeft.Substring(0, indexesAllToLeft.IndexOf(' ')));
            int jLeft = int.Parse(indexesAllToLeft.Substring(indexesAllToLeft.IndexOf(' ') + 1));

            int iRight = int.Parse(indexesAllToRight.Substring(0, indexesAllToLeft.IndexOf(' ')));
            int jRight = int.Parse(indexesAllToRight.Substring(indexesAllToLeft.IndexOf(' ') + 1));

            Vector2 alienPosLeft = aliens[iLeft, jLeft].pos;
            Vector2 alienPosRight = aliens[iRight, jRight].pos;

            //horizontal collisions
            if (alienPosLeft.X < 30)
            {
                SetAliensToRecentPosition();
                MoveAliensY();

                Alien.ChangeXDir();
            }

            else if (alienPosRight.X + aliens[iRight, jRight].GetWidth() > Gfx.Window.Width - 31)
            {
                SetAliensToRecentPosition();
                MoveAliensY();

                Alien.ChangeXDir();
            }
        }

        public static void SetAliensToRecentPosition()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    aliens[i, j].pos = recentPositions[i, j];
                }
            }
        }

        public static void MoveAliensX()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    recentPositions[i, j] = aliens[i, j].pos;
                    aliens[i, j].MoveX();
                }
            }
        }

        public static void MoveAliensY()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    aliens[i, j].MoveY();
                }
            }
        }

        public static void DrawAliens()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    aliens[i, j].Draw();
                }
            }
        }

        public static bool AllAliensDead()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    if (aliens[i, j].IsAlive())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string GetAlienIndexesAllToLeft()
        {
            for (int j = 0; j < aliens.GetLength(1); j++)
            {
                for (int i = 0; i < aliens.GetLength(0); i++)
                {
                    if (aliens[i, j].IsAlive())
                    {
                        return i + " " + j;
                    }
                }
            }

            return "Not found";
        }

        public static string GetAlienIndexesAllToRight()
        {
            for (int j = aliens.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = aliens.GetLength(0) - 1; i >= 0; i--)
                {
                    if (aliens[i, j].IsAlive())
                    {
                        return i + " " + j;
                    }
                }
            }

            return "Not found";
        }

        public static Alien[] GetAliensAtBottom()
        {
            Alien[] aliensAtBottom = new Alien[aliens.GetLength(1)];

            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    if (aliens[i, j].IsAlive())
                    {
                        aliensAtBottom[j] = aliens[i, j];
                    }
                }
            }

            return aliensAtBottom;
        }

        public static void Shoot()
        {
            int freeBulletIndex = BulletMngr.GetFreeAlienBulletIndex();
            AlienBullet[] bullets = BulletMngr.GetAlienBullets();
            Alien[] aliensAtBottom = GetAliensAtBottom();
            int randAlienIndex;
            int shootProbability = RandomGenerator.GetRandomInt(101); //every time the number 5 or 10 gets gened, the enemy will shoot if bullets slot is not full
            Vector2 bulletPos;

            if (freeBulletIndex != -1 && (shootProbability == 5 || shootProbability == 10))
            {
                randAlienIndex = GetRandomAlienIndexAtBottom(aliensAtBottom);

                bulletPos.X = aliensAtBottom[randAlienIndex].pos.X + aliensAtBottom[randAlienIndex].GetWidth() * 0.5f - bullets[freeBulletIndex].GetWidth() * 0.5f;
                bulletPos.Y = aliensAtBottom[randAlienIndex].pos.Y + aliensAtBottom[randAlienIndex].GetHeight();
                bullets[freeBulletIndex].pos = bulletPos;

                if (!AlienBulletTooNearToAnother(freeBulletIndex, bullets, aliensAtBottom[randAlienIndex]))
                {
                    bullets[freeBulletIndex].isShot = true;
                }
            }
        }

        public static bool AlienBulletTooNearToAnother(int currentIndex, AlienBullet[] bullets, Alien alien)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (i == currentIndex)
                {
                    continue;
                }

                if (bullets[i].isShot &&
                    ((bullets[currentIndex].pos.X >= bullets[i].pos.X && bullets[currentIndex].pos.X <= bullets[i].pos.X + bullets[i].GetWidth()) || (bullets[currentIndex].pos.X + bullets[currentIndex].GetWidth() >= bullets[i].pos.X && bullets[currentIndex].pos.X + bullets[currentIndex].GetWidth() <= bullets[i].pos.X + bullets[i].GetWidth())) &&
                    bullets[i].pos.Y <= alien.pos.Y + alien.GetHeight() + 100)
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetRandomAlienIndexAtBottom(Alien[] aliens)
        {
            int index = 0;

            do
            {
                index = RandomGenerator.GetRandomInt(aliens.Length);
            }
            while (aliens[index] == null);

            return index;
        }

        public static bool EnemyKilled(Bullet b)
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    if (aliens[i, j].Collided(b.pos) && aliens[i, j].IsAlive() && b.isShot)
                    {
                        aliens[i, j].OnHit();
                        return true;
                    }
                }
            }

            return false;
        }

        public static void UpdateAliensExplosion()
        {
            for (int i = 0; i < aliens.GetLength(0); i++)
            {
                for (int j = 0; j < aliens.GetLength(1); j++)
                {
                    aliens[i, j].UpdateExplosion();
                }
            }
        }
    }
}
