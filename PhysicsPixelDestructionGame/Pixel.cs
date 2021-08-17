using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LEarning
{
    class Pixel
    {
        public Sprite pixelWithTint;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color colour;

        public Pixel(Texture2D texture, Vector2 position, Color color)
        {
            pixelWithTint = new Sprite(texture);
            colour = color;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            pixelWithTint.Draw(spriteBatch, Position, color);
        }

    }
}
