using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    static class Game
    {
        private static Player player;
        public static float Gravity { get; private set; }
        private static int endCount;

        public static void Init()
        {
            Gfx.InitWindow("Square Invaders");
            player = new Player();
            AlienManager.InitAliens(5, 11, 55, 40);
            BulletMngr.InitBullets(4);
            Gravity = 1500;
            endCount = 0;
        }

        public static void Run()
        {
            while (Gfx.Window.IsOpened && player.GetNumLives() > 0 && endCount <= 500)
            {
                Gfx.ClearScreen();

                player.Move();
                player.CheckBoundsCollision();

                player.Draw();

                player.Shoot();

                if (!AlienManager.AllAliensDead())
                {
                    AlienManager.MoveAliensX();

                    AlienManager.CheckBoundsCollision();

                    BulletMngr.EnemyCollidedWithBullet();

                    AlienManager.Shoot();
                }

                else
                {
                    endCount++;
                }

                AlienManager.UpdateAliensExplosion();
                AlienManager.DrawAliens();

                BulletMngr.MoveShotBullets();
                BulletMngr.DrawShotBullets();

                BulletMngr.SetBulletsToAvailable();
                BulletMngr.ResetIndexToShoot();

                player.PlayerCollidedWithBullet();
                player.PlayerCollidedWithAlien();

                Gfx.Window.Blit();

                if (player.GetNumLives() == 0)
                {
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }
    }
}
