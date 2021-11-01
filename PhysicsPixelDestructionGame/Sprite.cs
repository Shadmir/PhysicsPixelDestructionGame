using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsPixelDestructionGame
{
    class Sprite
    {
        public Texture2D Texture { get; private set; }
        

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle inputRectangle, Rectangle outputRectangle, Color color)
        {
            spriteBatch.Draw(Texture, outputRectangle, inputRectangle, color);
        }
    }
}
