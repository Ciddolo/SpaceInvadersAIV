using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
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

        public Vector2 Sub(Vector2 vec)
        {
            return new Vector2(X - vec.X, Y - vec.Y);
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

    }
}
