using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class SpriteObj
    {
        Sprite sprite;
        Vector2 position;

        public int Width { get { return sprite.width; } }
        public int Height { get { return sprite.height; } }
        public Vector2 Position { get { return position; } set { position = value; } }

        public SpriteObj()
        {

        }

        public SpriteObj(string fileName, int x=0, int y=0)
        {
            sprite = new Sprite(fileName);
            position.X = x;
            position.Y = y;
        }

        public SpriteObj(string fileName,Vector2 spritePosition): this(fileName, (int) spritePosition.X, (int) spritePosition.Y)
        {
            
        }

        public SpriteObj(Sprite spriteRef)
        {
            sprite = spriteRef;
        }

        public void Translate(float deltaX, float deltaY)
        {
            position.X += deltaX;
            position.Y += deltaY;
        }

        public void SetSprite(Sprite newSprite)
        {
            sprite = newSprite;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public void Draw()
        {
            GfxTools.DrawSprite(sprite, (int)position.X, (int)position.Y);
        }
    }
}
