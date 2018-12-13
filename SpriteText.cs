using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class SpriteText
    {
        SpriteObj[] sprites;
        string text;
        public Vector2 Position;
        int width;
        int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }


        public SpriteText(Vector2 spritePos, string textString = "")
        {
            //sprite = new Sprite("assets/" + fileName);
            Position = spritePos;
            sprites = new SpriteObj[32];
            if (textString != "")
                SetText(textString);

            
        }

        private void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                int charX = (int)Position.X;
                int charY = (int)Position.Y;

                for (int i = 0; i < numChars && i < sprites.Length; i++)
                {
                    char number = text[i];
                    sprites[i] = new SpriteObj("Assets/numbers_" + number + ".png", charX, charY);
                    charX += sprites[i].Width;
                }
            }

        }

        public void Draw()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null)
                    return;
                sprites[i].Draw();
            }
        }
    }
}
