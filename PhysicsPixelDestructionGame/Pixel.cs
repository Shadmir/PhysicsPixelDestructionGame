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
        public float gravity = 1f;
        public int pixelID;
        Random r = new Random();
        public bool bonded;
        public int Width = 9;
        public int Height = 9;
        private Pixel collidingWith = null;
        public int diffX = 0;
        public int diffY = 0;
        public Rectangle futurePosition = new Rectangle();


        public Pixel(Texture2D texture, Vector2 position, string material, int id)
        {
            pixelWithTint = new Sprite(texture);
            Position = position;
            pixelID = id;
            //Velocity.X = r.Next(-10, 20);
            switch (material)
            {
                case "concrete":
                    Strength = 10;
                    colour = new Color((92 + r.Next(-10, 10)), (92 + r.Next(-10, 10)), (92 + r.Next(-10, 10)));
                    bonded = false;
                    break;

                case "steel":
                    Strength = 50;
                    colour = new Color((153 + r.Next(-10, 10)), (0 + r.Next(0, 10)), (0 + r.Next(0, 10)));
                    bonded = true;
                    break;

                case "wood":
                    Strength = 1;
                    colour = new Color((162 + r.Next(-10, 10)), (95 + r.Next(-10, 10)), (50 + r.Next(0, 10)));
                    bonded = true;
                    break;

                default:
                    Strength = 0;
                    colour = new Color(r.Next(255), r.Next(255), r.Next(255));
                    break;
            }
        }
        public Pixel(Sprite sprite, Vector2 position, Vector2 velocity, Color color, int strength, int id, bool glue)
        {
            pixelWithTint = sprite;
            colour = color;
            Position = position;
            Velocity = velocity;
            Strength = strength;
            pixelID = id;
            bonded = glue;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            pixelWithTint.Draw(spriteBatch, Position, colour);
        }

        public void Update(GameTime gameTime, List<Pixel> Pixels)
        {
            //need to look into the future for collision
            Velocity.Y += gravity;
            Position += Velocity;
            Position.X -= Position.X % 10;
            Position.Y -= Position.Y % 10;
            futurePosition = new Rectangle((int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y), Width, Height);
            if (Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                Velocity.Y = 0;
                Position.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 10;
                Velocity.X = 0;
            }

            if (Position.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 11)
            {
                Velocity.Y = 0;
                Position.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 9;
                Velocity.X = 0;
            }
            if (Position.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width || Position.X < 0)
            {
                if (Position.X < 0)
                {
                    Position.X = 0;
                    Velocity.X = 0;
                }
                else
                {
                    Position.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10;
                    Velocity.X = 0;
                }

            }
            collidingWith = null;
            colour = Color.Gray;
            foreach (Pixel pixel in Pixels)
            {
                if (pixelID != pixel.pixelID)
                {
                    Rectangle futureOtherPos = new Rectangle((int)(pixel.Position.X + pixel.Velocity.X), (int)(pixel.Position.Y + pixel.Velocity.Y), pixel.Width, pixel.Height);
                    if (futurePosition.Intersects(futureOtherPos))
                    {
                        collidingWith = pixel;
                    }
                }

                // check is right or bottom are contained.

                //CAN DO WITH MATHS FROM POSITION OF TOP LEFT PIX  
            }
            if (collidingWith != null)
            {
                diffX = (int)futurePosition.X - (int)collidingWith.futurePosition.X;
                diffY = (int)futurePosition.Y - (int)collidingWith.futurePosition.Y;
            }

        }

    }
}

