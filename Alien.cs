using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Alien
    {
        int width;
        int height;

        Color color;
        //Rect sprite;
        int distToSide;
        Pixel[] sprite;
        float nextShoot;
        int visiblePixels;

        public Vector2 Velocity;
        public Vector2 Position;
        public bool IsAlive;
        public bool CanShoot;
        public bool IsVisible;

        public Alien(Vector2 pos, Vector2 vel, int w, int h, Color col)
        {
            Position = pos;
            Velocity = vel;
            width = w;
            height = h;
            color = col;
            IsAlive = true;
            IsVisible = true;
            distToSide = 20;

            nextShoot = RandomGenerator.GetRandom(2, 12);


            //sprite = new Rect(Position.X - width / 2, Position.Y - height / 2, width, height, tools, color);

            byte[] pixelArr = {  0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                                 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                                 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                                 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0,
                                 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1,
                                 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1,
                                 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0
            };

            int numPixels = 0;

            for (int i = 0; i < pixelArr.Length; i++)
            {
                if (pixelArr[i] == 1)
                    numPixels++;
            }

            sprite = new Pixel[numPixels];
            visiblePixels = numPixels;

            int verticalPixel = 8;
            int horizontalPixel = 11;
            int pixelSize = height / verticalPixel;
            width = horizontalPixel * pixelSize;

            float startPosX = Position.X - (float)width / 2;
            float posY = Position.Y - height / 2;


            int sp = 0;
            for (int i = 0; i < pixelArr.Length; i++)
            {
                if (i != 0 && i % horizontalPixel == 0)
                    posY += pixelSize;

                if (pixelArr[i] != 0)
                {
                    float pixelX = startPosX + (i % horizontalPixel) * (pixelSize);
                    sprite[sp] = new Pixel(new Vector2(pixelX, posY), pixelSize, color);
                    sp++;
                }
            }
        }

        public bool OnHit()
        {
            IsAlive = false;

            for (int i = 0; i < sprite.Length; i++)
            {
                Vector2 pixelVel = sprite[i].Position.Sub(Position);
                pixelVel.X *= RandomGenerator.GetRandom(4, 15);
                pixelVel.Y *= RandomGenerator.GetRandom(4, 23);
                sprite[i].Velocity = pixelVel;
                sprite[i].IsGravityAffected = true;

            }
            return true;
        }

        public bool Update(ref float overflowX)
        {
            bool endReached = false;

            if (IsAlive)
            {
                float deltaX = Velocity.X * GfxTools.Win.deltaTime;
                float deltaY = Velocity.Y * GfxTools.Win.deltaTime;
                Position.X += deltaX;
                Position.Y += deltaY;

                float maxX = Position.X + width / 2;
                float minX = Position.X - width / 2;


                if (maxX > GfxTools.Win.width - distToSide)
                {
                    overflowX = maxX - (GfxTools.Win.width - distToSide);
                    //Position.X -= overflowX;
                    //deltaX -= overflowX;
                    //velocity.X = -velocity.X;
                    endReached = true;
                }
                else if (minX < distToSide)
                {
                    overflowX = minX - distToSide;
                    //Position.X -= overflowX;
                    //deltaX -= overflowX;
                    //velocity.X = -velocity.X;
                    endReached = true;
                }

                //sprite.Translate(deltaX, deltaY);
                TranslateSprite(new Vector2(deltaX, deltaY));

                if (Position.Y + height / 2 >= Game.GetPlayer().GetPosition().Y)
                {
                    EnemyMgr.Landed = true;
                }
                else if (CanShoot)
                {
                    nextShoot -= GfxTools.Win.deltaTime;
                    if (nextShoot <= 0)
                    {
                        EnemyMgr.Shoot(this);
                        nextShoot = RandomGenerator.GetRandom(1, 15);
                    }
                }


            }
            else if (IsVisible)//he's dead but visible
            {
                for (int i = 0; i < sprite.Length; i++)
                {
                    if (sprite[i].IsVisibile)
                    {
                        sprite[i].Update();
                        if (sprite[i].IsVisibile == false)
                        {
                            visiblePixels--;
                            if (visiblePixels <= 0)
                            {
                                IsVisible = false;
                                EnemyMgr.OnAlienDisappears();
                            }
                        }
                    }
                }
            }
            return endReached;
        }

        public void Draw()
        {
            for (int i = 0; i < sprite.Length; i++)
            {
                if(sprite[i].IsVisibile)
                    sprite[i].Draw();
            }
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        private void TranslateSprite(Vector2 transVect)
        {
            for (int i = 0; i < sprite.Length; i++)
            {
                sprite[i].Translate(transVect.X, transVect.Y);
            }
        }

        public void Translate(Vector2 transVect)
        {
            Position.X += transVect.X;
            Position.Y += transVect.Y;

            //sprite.Translate(transVect.X, transVect.Y);
            TranslateSprite(transVect);
        }

    }
}
