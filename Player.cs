using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Player
    {
        Vector2 position;
        int width;
        int height;
        //Rect baseRect;
        //Rect cannonRect;
		SpriteObj sprite;
        float speed;
        const float maxSpeed = 180.0f;
        int distToSide;
        Bullet[] bullets;
        bool IsFirePressed;
        float counter;
        float shootDelay;
        Color color;
        float ray;
        int nrg;

        int score;

        public bool IsAlive;
        public int Lifes { get { return nrg; } }

        public Player(Vector2 pos, Color col)
        {
            position = pos;
            sprite = new SpriteObj("Assets/player.png", position);
            width = sprite.Width;
            height = sprite.Height;
            sprite.Translate(-width / 2, -height);
            distToSide = 20;
            shootDelay = 0.5f;
            color = col;
            ray = width / 2;
            nrg = 3;
            score = 0;

            IsAlive = true;


            //baseRect = new Rect(position.X - width / 2, position.Y - height / 2, width, height / 2, color);
            //int cannWidth = width / 3;
            //cannonRect = new Rect(position.X - cannWidth / 2, position.Y - height, cannWidth, height / 2, color);

            bullets = new Bullet[30];

            Color bulletCol = new Color(200, 0, 0);
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Bullet(10, 20,bulletCol);
            }
        }

        public void Input()
        {
            counter += GfxTools.Win.deltaTime;

            if (GfxTools.Win.GetKey(KeyCode.Right))
            {
                speed = maxSpeed;
            }
            else if (GfxTools.Win.GetKey(KeyCode.Left))
            {
                speed = -maxSpeed;
            }
            else
                speed = 0;

            if (GfxTools.Win.GetKey(KeyCode.Space))
            {
                if (/*IsFirePressed == false && */counter >= shootDelay)
                {
                    //IsFirePressed = true;
                    Shoot();
                    counter = 0;
                }
            }
            //else if (IsFirePressed)
            //{
            //    IsFirePressed = false;
            //}
        }

        private Bullet GetFreeBullet()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive == false)
                {
                    return bullets[i];
                }
            }
            return null;
        }

        public void Shoot()
        {
            Bullet b = GetFreeBullet();
            if (b != null)
            {
                b.Shoot(new Vector2(position.X,position.Y-height-15),new Vector2(0,-250));
            }
        }

        public bool OnHit()
        {
            nrg--;
            if (nrg <= 0)
            {
                IsAlive = false;
            }

            return !IsAlive;//return true if player is dead
        }

        public void Update()
        {
            float deltaX= speed * GfxTools.Win.deltaTime;
            position.X += deltaX;
            float maxX = position.X + width / 2;
            float minX = position.X - width / 2;

            if (maxX > GfxTools.Win.width-distToSide)
            {
                float overflowX = maxX - (GfxTools.Win.width-distToSide);
                position.X -= overflowX;
                deltaX -= overflowX;
            }
            else if (minX < distToSide)
            {
                float overflowX = minX - distToSide;
                position.X -= overflowX;
                deltaX -= overflowX;
            }

            //rectangles update
            //baseRect.Translate(deltaX, 0);
            //cannonRect.Translate(deltaX, 0);
            sprite.Translate(deltaX, 0);

            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive)
                {
                    bullets[i].Update();

                    if (Game.BarriersCollides(bullets[i].Position, bullets[i].GetWidth() / 2))
                    {
                        bullets[i].IsAlive = false;
                    }
                    else if (EnemyMgr.CollideWithBullet(bullets[i]))
                    {
                        bullets[i].IsAlive = false;
                    }
                }
            }
        }

        public void Draw()
        {
            //baseRect.Draw();
            //cannonRect.Draw();
            sprite.Draw();

            for (int i = 0; i < bullets.Length; i++)
            {
                if(bullets[i].IsAlive)
                    bullets[i].Draw();
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetRay()
        {
            return ray;
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int amount)
        {
            score += amount;
            Game.SetScore(score);
        }
    }
}
