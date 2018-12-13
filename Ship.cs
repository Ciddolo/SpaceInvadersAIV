using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Ship
    {
        Vector2 velocity;
        SpriteObj sprite;
        Animation animation;
        float ray;

        public Vector2 Position;
        public bool IsAlive;

        public int Width { get { return sprite.Width; } }
        public int Height { get { return sprite.Height; } }
        public float Ray { get { return ray; } }

        public Ship()
        {
            Position = new Vector2(0, 0);
            velocity = Position;
            sprite = new SpriteObj();
            
            string[] animationFiles = new string[8];
            for (int i = 0; i < 5; i++)
            {
                animationFiles[i] = "Assets/ship_" + i + ".png";
            }

            int index = 3;
            for (int i = 0; i <3; i++)
            {
                animationFiles[i+5] = "Assets/ship_" + index + ".png";
                index--;
            }
            // = { "Assets/alienBullet_0.png", "Assets/alienBullet_1.png" };
            animation = new Animation(animationFiles, sprite, 12);
            ray = sprite.Width / 2;
        }

        public void Update()
        {
            animation.Update();

            float deltaX = velocity.X * GfxTools.Win.deltaTime;
            Position.X += deltaX;

            float deltaY = velocity.Y * GfxTools.Win.deltaTime;
            Position.Y += deltaY;

            if (sprite != null)
                sprite.Translate(deltaX, deltaY);

            if (Position.X - ray  >= GfxTools.Win.width)
            {
                IsAlive = false;
            }
        }

        public void Draw()
        {
            sprite.Draw();
        }

        public void Respawn()
        {
            Position = new Vector2(-ray, Height / 2 + 50);
            velocity.X = 200;
            IsAlive = true;
            sprite.Position = Position;
            sprite.Translate(-Width / 2, -Height / 2);
        }

        public void OnHit()
        {
            IsAlive = false;
        }

        //public bool Collides(Vector2 center, float ray)
        //{
        //    Vector2 dist = Position.Sub(center);
        //    return (dist.GetLength() <= Width / 2 + ray);
        //}

    }
}
