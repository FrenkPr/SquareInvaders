using Aiv.Draw;
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
        private static float endCount;

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
            while (Gfx.Window.IsOpened && player.GetNumLives() > 0 && endCount <= 3)
            {
                Gfx.ClearScreen();

                if (player.IsPauseMenuActive)
                {
                    player.TogglePauseMenu();
                    player.QuitGame();

                    player.Draw();
                    AlienManager.DrawAliens();
                    BulletMngr.DrawShotBullets();

                    Gfx.Window.Blit();
                    continue;
                }

                player.TogglePauseMenu();
                player.QuitGame();
                player.Move();
                player.CheckBoundCollisions();

                player.Draw();

                player.Shoot();

                if (!AlienManager.AllAliensDead())
                {
                    AlienManager.MoveAliensX();

                    AlienManager.CheckBoundCollisions();

                    BulletMngr.EnemyCollidedWithBullet();

                    AlienManager.Shoot();
                }

                else
                {
                    endCount += Gfx.Window.DeltaTime;
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
