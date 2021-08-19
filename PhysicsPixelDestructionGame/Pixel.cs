using System;
using System.Collections.Generic;
using System.Text;
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
        public int Strength;
        public List<Pixel> Pixels = new List<Pixel>();
        public float gravity = 16.35f;
        public int pixelID;
        Random r = new Random();

        public Pixel(Texture2D texture, Vector2 position, Color color, string material, List<Pixel> pixels, int id)
        {
            pixelWithTint = new Sprite(texture);
            colour = color;
            Position = position;
            Position.X += r.Next(-5, 5);
            Position.Y += r.Next(-5, 5);
            Pixels = pixels;
            pixelID = id;
            switch (material)
            {
                case "concrete":
                    Strength = 10;
                    break;

                case "steel":
                    Strength = 50;
                    break;

                case "wood":
                    Strength = 1;
                    break;

                default:
                    Strength = 0;
                    break;
            }
        }
        bool CheckOccupied(List<Pixel> pixels, Vector2 pos)
        {
            foreach (Pixel item in Pixels)
            {
                if (item.Position == pos)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            pixelWithTint.Draw(spriteBatch, Position, colour);
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;

            if (Position.Y < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1)
            {
                Velocity.Y += gravity;
            }
            if (Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1)
            {
                Velocity.Y = 0;
            }
            if (Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                Position.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1;
            }
            foreach (Pixel pixel in Pixels)
            {
                if (pixelID != pixel.pixelID)
                {
                    Vector2 nextPosition = new Vector2((Position.X + Velocity.X), (Position.Y + Velocity.Y));
                    Vector2 pixelNextPosition = new Vector2((pixel.Position.X + pixel.Velocity.X), (pixel.Position.Y + pixel.Velocity.Y));
                    if (nextPosition == pixelNextPosition)
                    {
                        
                    }
                }
            }





        }
    }
}
