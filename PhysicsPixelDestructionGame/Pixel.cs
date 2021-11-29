using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Pixel
    {
        public Sprite pixelWithTint;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color colour;
        public int pixelID;
        Random r = new Random();
        public int Width = 10;
        public int Height = 10;
        public Pixel(Texture2D texture, Vector2 position, int id)
        {
            pixelWithTint = new Sprite(texture);
            Position = position;
            pixelID = id;
            colour = new Color(100 + r.Next(-10, 10), 40 + r.Next(-10, 10), r.Next(-10, 10));
        }

        public Pixel(Sprite sprite, Vector2 position, Color color, int id)
        {
            pixelWithTint = sprite;
            colour = color;
            Position = position;
            pixelID = id;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            pixelWithTint.Draw(spriteBatch, Position, new Vector2(Width, Height), colour);
        }

        public void Update(GameTime gameTime)
        {  
            
        }
    }
}

