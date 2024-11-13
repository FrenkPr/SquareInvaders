using Aiv.Draw;

namespace SquareInvaders
{
    class AlienBullet
    {
        public Vector2 pos;
        private Sprite bullet;
        private Animation animation;
        private Vector2 moveSpeed;
        public bool isShot;

        public AlienBullet()
        {
            // Animation Sprites settings
            string[] frames = new string[2];

            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = $"Assets/alienBullet_{i}.png";
            }

            animation = new Animation(frames, 20);
            bullet = animation.CurrentSprite;

            moveSpeed = new Vector2(0, 500);
            isShot = false;
        }

        public void Move()
        {
            pos.Add(moveSpeed.Scale(Gfx.Window.DeltaTime));

            animation.Update();
            bullet = animation.CurrentSprite;
        }

        public void Draw()
        {
            Gfx.DrawSprite(bullet, (int)pos.X, (int)pos.Y);
        }

        public int GetWidth()
        {
            return bullet.Width;
        }

        public int GetHeight()
        {
            return bullet.Height;
        }
    }
}
