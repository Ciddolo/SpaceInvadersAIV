using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    static class Game
    {
        static Window window;
        static Player player;
        static float totalTime;
        static float gravity;
        static SpriteText scoreSprite;
        static int playerOneScore = 0;
        static Barrier[] barriers;
        static Sprite heart;

        public static float DeltaTime { get { return window.deltaTime; } }
        public static float Gravity { get { return gravity; } }

        static Game()
        {
            window = new Window(800, 600, "Space Invaders", PixelFormat.RGB);
            gravity = 555.0f;
            GfxTools.Init(window);

            Vector2 playerPos;
            playerPos.X = window.width / 2;
            playerPos.Y = window.height - 20;

            EnemyMgr.Init(24, 3);

            player = new Player(playerPos, new Color(22, 22, 200));
            heart = new Sprite("Assets/heart.png");

            barriers = new Barrier[3];

            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i] = new Barrier(window.width * (i + 1) / 4, (int)playerPos.Y - 80); 
            }



            scoreSprite = new SpriteText(new Vector2(window.width / 4, 20), "000000");
        }

        public static Player GetPlayer()
        {
            return player;
        }

        private static void DrawGUI()
        {
            scoreSprite.Draw();
            for(int i = 0; i < player.Lifes; i++)
            {
                GfxTools.DrawSprite(heart, (int)(window.width * (0.75f/* 3/4 */)+(i*heart.width)), 10);
            }
        }

        public static bool BarriersCollides(Vector2 center, float ray)
        {
            for (int i = 0; i < barriers.Length; i++)
            {
                if (barriers[i].Collides(center, ray))
                    return true;
            }
            return false;
        }

        public static void Play()
        {
            while (window.opened)
            {
                if (!player.IsAlive || EnemyMgr.AllGone || EnemyMgr.Landed)
                {
                     return;
                }

                totalTime += GfxTools.Win.deltaTime;
                GfxTools.Clean();
                //Input
                if (window.GetKey(KeyCode.Esc))
                    return;

                player.Input();

                //Update
                EnemyMgr.Update();
                for (int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].RepairBarrier();
                }
                player.Update();

                //Draw
                for (int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].Draw();
                }
                EnemyMgr.Draw();
                player.Draw();
                DrawGUI();

                window.Blit();
            }
        }

        public static int GetScore()
        {
            return player.GetScore() - (int)(totalTime * 1);
        }

        public static void SetScore(int newScore)
        {
            playerOneScore = newScore;
            scoreSprite.Text = playerOneScore.ToString("D6");
        }
    }
}
