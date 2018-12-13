using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Pixel
    {
        Vector2 position;
        Vector2 velocity;
        int width;
        Color color;

        public bool IsGravityAffected;
        public bool IsVisibile;
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector2 Position { get { return position; } private set { position = value; } }

        public Pixel(Vector2 pos, int w, Color col)
        {
            position = pos;
            width = w;
            color = col;
            IsGravityAffected = false;
            IsVisibile = true;
        }

        public void Draw()
        {
            GfxTools.DrawRect((int)position.X, (int)position.Y, width, width, color.R, color.G, color.B);
        }

        public void Translate(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public void Update()
        {
            if (IsGravityAffected)
                velocity.Y += Game.Gravity * Game.DeltaTime;

            position.X += velocity.X * Game.DeltaTime;
            position.Y += velocity.Y * Game.DeltaTime;

            if(position.Y>=GfxTools.Win.height || position.X+width<0 || position.X >= GfxTools.Win.width)
            {
                IsVisibile = false;
            }
        }

    }
}
