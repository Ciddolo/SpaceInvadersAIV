using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    static class EnemyMgr
    {
        static Alien[] aliens;
        static int numAliens;
        static int numRows;
        static int aliensPerRow;
        static int alienWidth;
        static int alienHeight;
        static AlienBullet[] bullets;
        static int numAlives;
        static int numVisibles;
        static float nextShipSpawn;
        static Ship ship;

        public static bool Landed;

        public static void Init(int numOfAliens, int numOfRows)
        {
            numAliens = numOfAliens;
            numRows = numOfRows;
            aliensPerRow = numAliens / numRows;
            numVisibles = numAlives = numAliens;
            ship = new Ship();
            nextShipSpawn = RandomGenerator.GetRandom(5, 20);

            aliens = new Alien[numAliens];

            int startX = 40;
            int posY = 40;
            int dist = 5;
            alienWidth = 55;
            alienHeight = 40;

            Color green = new Color(0, 255, 0);

            for (int i = 0; i < aliens.Length; i++)
            {
                if(i!=0 && i%aliensPerRow==0)
                {
                    posY += alienHeight+dist;
                }
                int alienX = startX + (i%aliensPerRow) * (alienWidth + dist);
                aliens[i] = new Alien(new Vector2(alienX, posY), new Vector2(100, 0), alienWidth, alienHeight, green);

                if (i>=numOfAliens-aliensPerRow)
                {
                    aliens[i].CanShoot = true;
                }
            }

            bullets = new AlienBullet[aliensPerRow];

            Color bulletCol = new Color(200, 200, 200);


            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new AlienBullet();
            }


           // aliens[0]= new Alien(tools, new Vector2(tools.Win.width / 2, 40), new Vector2(250, 0), 40, 40, new Color(0, 255, 0));
        }

        public static void Update()
        {
            bool endReached = false;
            float tmpOverflowX = 0;
            float overflowX = 0;

            if (!ship.IsAlive)
            {
                nextShipSpawn -= Game.DeltaTime;
                if (nextShipSpawn <= 0)
                {
                    ship.Respawn();
                    nextShipSpawn = RandomGenerator.GetRandom(5, 20);
                }
            }
            else
            {
                ship.Update();
            }

            for (int i = 0; i < aliens.Length; i++)
            {
                if (aliens[i].IsVisible)
                {
                    if (aliens[i].Update(ref tmpOverflowX))
                    {
                        endReached = true;
                        overflowX = tmpOverflowX;
                    }
                    Game.BarriersCollides(aliens[i].Position, alienWidth / 2);
                }
            }

            if (endReached)//at least one alien has reached the end of the screen (or the start!)
            {
                for (int i = 0; i < aliens.Length; i++)
                {
                    //Position.X -= overflowX;
                    //deltaX -= overflowX;
                    if (aliens[i].IsAlive)
                    {
                        aliens[i].Translate(new Vector2(-overflowX, 20));
                        aliens[i].Velocity.X = -aliens[i].Velocity.X;
                    }
                }
            }

            Player player = Game.GetPlayer();
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive)
                {
                    bullets[i].Update();
                    //check collision with player
                    if (bullets[i].Collides(player.GetPosition(), player.GetRay()))
                    {
                        bullets[i].IsAlive = false;
                        player.OnHit();
                    }
                    else if (Game.BarriersCollides(bullets[i].Position, bullets[i].Width / 2))
                    {
                        bullets[i].IsAlive = false;
                    }

                }
            }
        }

        private static AlienBullet GetFreeBullet()
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

        public static void Shoot(Alien shooter)
        {
            AlienBullet b = GetFreeBullet();
            if (b != null)
            {
                b.Shoot(new Vector2(shooter.Position.X, shooter.Position.Y + shooter.GetHeight()/2 + 15), new Vector2(0, 250));
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                if (aliens[i].IsVisible)
                    aliens[i].Draw();
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].IsAlive)
                    bullets[i].Draw();
            }

            if (ship.IsAlive)
            {
                ship.Draw();
            }
        }

        public static int GetAlives()
        {
            return numAlives;
        }

        public static bool AllGone
        {
            get
            {
                return numVisibles <= 0;
            }
        }

        public static void OnAlienDisappears()
        {
            numVisibles--;
        }

        private static void IncAliensSpeed(float percentage)
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                aliens[i].Velocity.X *= percentage;
            }
        }

        public static bool CollideWithBullet(Bullet bullet)
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                if (aliens[i].IsAlive)
                {
                    //Vector2 dist = aliens[i].Position.Sub(bullet.Position);
                    //if (dist.GetLength() <= aliens[i].GetWidth()/2 + bullet.GetWidth()/2)
                    if(bullet.Collides(aliens[i].Position, aliens[i].GetWidth()/2))
                    {
                        //alien dies
                        if (aliens[i].OnHit())
                        {
                            Game.GetPlayer().AddScore(5);
                            IncAliensSpeed(1.05f);
                            //he's dead
                            if (aliens[i].CanShoot)
                            {
                                int prevAlienIndex = i - aliensPerRow;
                                while (prevAlienIndex >= 0)
                                {
                                    if (aliens[prevAlienIndex].IsAlive)
                                    {
                                        aliens[prevAlienIndex].CanShoot = true;
                                        break;
                                    }

                                    prevAlienIndex -= aliensPerRow;
                                }
                            }
                            numAlives--;
                        }
                        return true;
                    }
                }
                    
            }

            //spaceship collision
            if(ship.IsAlive && bullet.Collides(ship.Position, ship.Ray))
            {
                ship.OnHit();
                Game.GetPlayer().AddScore(100);
                return true;
            }
            return false;
        }
    }
}
