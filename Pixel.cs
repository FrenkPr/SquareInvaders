using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvaders
{
    class Pixel
    {
        public Vector2 Velocity;
        private Vector2 position;
        private int width;
        private Color color;
        public bool isOutOfScreen;

        public Pixel(Vector2 pos, int w, Color col)
        {
            position = pos;
            width = w;
            color = col;
            isOutOfScreen = false;
        }

        public void Translate(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public void Update()
        {
            Velocity.Y += Game.Gravity * Gfx.Window.DeltaTime;

            Vector2 delta = Velocity.Scale(Gfx.Window.DeltaTime); //velocity * time

            //position = position + delta move
            position.Add(delta);

            //check for screen
            if (position.X + width < 0 || position.X >= Gfx.Window.Width || position.Y >= Gfx.Window.Height)
            {
                isOutOfScreen = true;
            }
        }

        public void Draw()
        {
            Gfx.DrawFilledRect((int)position.X, (int)position.Y, width, width, color);
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
