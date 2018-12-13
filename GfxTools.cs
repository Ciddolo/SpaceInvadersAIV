using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    static class GfxTools
    {
        public static Window Win;
        public static void Init(Window window)
        {
            Win = window;
        }

        public static void Clean()
        {
            for (int i = 0; i < Win.bitmap.Length; i++)
            {
                Win.bitmap[i] = 0;
            }
        }

        public static void PutPixel(int x, int y, byte r, byte g, byte b)
        {
            if (x < 0 || x >= Win.width || y < 0 || y >= Win.height)
                return;
            int index = (y * Win.width + x) * 3;
            Win.bitmap[index] = r;
            Win.bitmap[index + 1] = g;
            Win.bitmap[index + 2] = b;
        }

        public static void DrawSprite(Sprite sprite, int spriteX, int spriteY)
        {
            int x;
            int y;

            for (int r = 0; r < sprite.height; r++)
            {
                for (int c = 0; c < sprite.width; c++)
                {
                    x = spriteX + c;
                    y = spriteY + r;

                    if (x < 0 || x >= Win.width || y < 0 || y >= Win.height)
                        continue;

                    int canvasIndex = (y * Win.width + x) * 3;
                    int spriteIndex = (r * sprite.width + c) * 4;

                    byte red = sprite.bitmap[spriteIndex];
                    byte gre = sprite.bitmap[spriteIndex + 1];
                    byte blu = sprite.bitmap[spriteIndex + 2];
                    byte a = sprite.bitmap[spriteIndex + 3];
                    float alpha = a / 255f;

                    byte backR = Win.bitmap[canvasIndex];
                    byte backG = Win.bitmap[canvasIndex + 1];
                    byte backB = Win.bitmap[canvasIndex + 2];

                    byte blendedR = (byte)(red * alpha + backR * (1 - alpha));
                    byte blendedG = (byte)(gre * alpha + backG * (1 - alpha));
                    byte blendedB = (byte)(blu * alpha + backB * (1 - alpha));

                    Win.bitmap[canvasIndex] = blendedR;
                    Win.bitmap[canvasIndex + 1] = blendedG;
                    Win.bitmap[canvasIndex + 2] = blendedB;
                }
            }

        }

        public static void DrawHorizontalLine(int x, int y, int width, byte r, byte g, byte b)
        {
            for (int i = 0; i < width; i++)
            {
                PutPixel(x + i, y, r, g, b);
            }
        }

        public static void DrawVerticalLine( int x, int y, int height, byte r, byte g, byte b)
        {
            for (int i = 0; i < height; i++)
            {
                PutPixel(x, y + i, r, g, b);
            }
        }

        public static void DrawRect(int x, int y, int width, int height, byte r, byte g, byte b)
        {
            for (int i = 0; i < height; i++)
            {
                DrawHorizontalLine(x, y + i, width, r, g, b);
            }
        }
    }
}
