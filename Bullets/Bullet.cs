namespace SquareInvaders
{
    class Bullet
    {
        public Vector2 pos;
        private int width;
        private int height;
        private Vector2 moveSpeed;
        public bool isShot;
        private Color color;

        public Bullet(int width, int height)
        {
            this.width = width;
            this.height = height;
            moveSpeed = new Vector2(0, 1000);
            isShot = false;
            color = new Color(255, 0, 0);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public void Move()
        {
            pos.Sub(moveSpeed.Scale(Gfx.Window.DeltaTime));
        }

        public void Draw()
        {
            Gfx.DrawFilledRect((int)pos.X, (int)pos.Y, width, height, color);
        }
    }
}
