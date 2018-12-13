using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class AlienBullet
    {
        Vector2 velocity;
        SpriteObj sprite;
        Animation animation;

        public Vector2 Position;
        public bool IsAlive;

        public int Width { get { return sprite.Width; } }
        public int Height { get { return sprite.Height; } }

        public AlienBullet()
        {
            Position = new Vector2(0, 0);
            velocity = Position;
            sprite = new SpriteObj();
            string[] animationFiles = { "Assets/alienBullet_0.png", "Assets/alienBullet_1.png" };
            animation = new Animation(animationFiles, sprite, 12);           
        }

        //public void SetSprite(SpriteObj newSprite)
        //{
        //    sprite = newSprite;
        //}

        public void Update()
        {
            animation.Update();

            float deltaX = velocity.X * GfxTools.Win.deltaTime;
            Position.X += deltaX;

            float deltaY = velocity.Y * GfxTools.Win.deltaTime;
            Position.Y += deltaY;

            if (sprite != null)
                sprite.Translate(deltaX, deltaY);

            if (Position.Y - Height / 2 >= GfxTools.Win.height)
            {
                IsAlive = false;
            }
        }

        public void Draw()
        {
            sprite.Draw();
        }

        public bool Collides(Vector2 center, float ray)
        {
            Vector2 dist = Position.Sub(center);
            return (dist.GetLength() <= Width / 2 + ray);
        }

        public void Shoot(Vector2 startPos, Vector2 startVelocity)
        {
            Position = startPos;
            velocity = startVelocity;
            IsAlive = true;
            sprite.Position = new Vector2(Position.X - sprite.Width / 2, Position.Y-sprite.Height / 2);
        }
    }
}
