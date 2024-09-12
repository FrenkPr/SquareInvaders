using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    static class BulletMngr
    {
        private static Bullet[] playerBullets;
        private static AlienBullet[] alienBullets;
        private static int playerIndexToShoot;

        public static void InitBullets(int numBullets, int width = 7, int height = 20)
        {
            playerBullets = new Bullet[numBullets];
            alienBullets = new AlienBullet[numBullets];

            for (int i = 0; i < numBullets; i++)
            {
                playerBullets[i] = new Bullet(width, height);
                alienBullets[i] = new AlienBullet();
            }

            playerIndexToShoot = 0;
        }

        public static void SetBulletsToAvailable()
        {
            Vector2 pos;

            for (int i = 0; i < playerBullets.Length; i++)
            {
                pos = playerBullets[i].pos;

                if (pos.Y < 0)
                {
                    playerBullets[i].isShot = false;
                }
            }

            for (int i = 0; i < alienBullets.Length; i++)
            {
                pos = alienBullets[i].pos;

                if (pos.Y >= Gfx.Window.Width)
                {
                    alienBullets[i].isShot = false;
                }
            }
        }

        public static void ResetIndexToShoot()
        {
            if (AllPlayerBulletsShot())
            {
                playerIndexToShoot = 0;
            }
        }

        public static bool AllPlayerBulletsShot()
        {
            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (playerBullets[i].isShot)
                {
                    return false;
                }
            }

            return true;
        }

        public static void DrawShotBullets()
        {
            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (playerBullets[i].isShot)
                {
                    playerBullets[i].Draw();
                }
            }

            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (alienBullets[i].isShot)
                {
                    alienBullets[i].Draw();
                }
            }
        }

        public static void MoveShotBullets()
        {
            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (playerBullets[i].isShot)
                {
                    playerBullets[i].Move();
                }
            }

            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (alienBullets[i].isShot)
                {
                    alienBullets[i].Move();
                }
            }
        }

        public static int GetFreePlayerBulletIndex()
        {
            int index = -1;

            if (playerIndexToShoot != playerBullets.Length)
            {
                index = playerIndexToShoot;
            }

            return index;
        }

        public static void IncPlayerIndexToShoot()
        {
            playerIndexToShoot++;
        }

        public static int GetFreeAlienBulletIndex()
        {
            for (int i = 0; i < alienBullets.Length; i++)
            {
                if (!alienBullets[i].isShot)
                {
                    return i;
                }
            }

            return -1;
        }

        public static Bullet[] GetPlayerBullets()
        {
            return playerBullets;
        }

        public static AlienBullet[] GetAlienBullets()
        {
            return alienBullets;
        }

        public static void EnemyCollidedWithBullet()
        {
            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (AlienManager.EnemyKilled(playerBullets[i]))
                {
                    playerBullets[i].isShot = false;
                }
            }
        }
    }
}
