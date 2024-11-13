using System;

namespace SquareInvaders
{
    struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Add(Vector2 v)
        {
            X += v.X;
            Y += v.Y;
        }

        public void Sub(Vector2 v)
        {
            X -= v.X;
            Y -= v.Y;
        }

        public void Mul(float s)
        {
            X *= s;
            Y *= s;
        }

        public Vector2 Scale(float s)
        {
            Vector2 v;

            v.X = X;
            v.Y = Y;

            v.X *= s;
            v.Y *= s;

            return v;
        }

        public Vector2 GetNorm()
        {
            Vector2 v;
            float vLen = 0.0f;

            v.X = X;
            v.Y = Y;

            vLen = v.GetVectorLength();

            v.X /= vLen;
            v.Y /= vLen;

            return v;
        }

        public float GetVectorLength()
        {
            return (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
    }
}
