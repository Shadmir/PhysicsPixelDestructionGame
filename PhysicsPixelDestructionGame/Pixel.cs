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
        public int Width = 10;
        public int Height = 10;

        public Pixel(Texture2D texture, Vector2 position, string material, int id)
        {
            pixelWithTint = new Sprite(texture);
            Position = position;
            pixelID = id;
            Velocity.X = r.Next(-10, 20);
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
            bool colliding_L = false;
            bool colliding_B = false;
            Pixel colliding_with = null;

            Rectangle pixelFutureEdge = new Rectangle((int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y), (int)Width, (int)Height);
            Velocity.Y += gravity;
            if (Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 11)
            {
                Velocity.Y = 0;
                Velocity.X = 0;
            }
            if (Position.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                Velocity.Y = 0;
                Position.Y = 1070;
            }

            foreach (Pixel pixel in Pixels)
            {
                if (pixelID != pixel.pixelID)
                {
                    Rectangle otherFutureEdge = new Rectangle((int)(pixel.Position.X + pixel.Velocity.X), (int)(pixel.Position.Y + pixel.Velocity.Y), (int)pixel.Width, (int)pixel.Height);
                    if (Position.Y + Height > pixel.Position.Y && Position.Y + Height < pixel.Position.Y + pixel.Height)
                    {
                        if (Position.X + Width >= pixel.Position.X && Position.X <= pixel.Position.X + pixel.Width)
                        {
                            colliding_B = true;
                            colliding_with = pixel;
                        }
                    }
                    if (pixelFutureEdge.Intersects(otherFutureEdge))
                    {
                        colliding_L = true;
                        colliding_with = pixel;
                    }
                    if (Position == pixel.Position)
                    {
                        Position.X += 10;
                    }
                }
            }



            if (colliding_B)
            {
                Velocity.Y = 0;
                if (Velocity.X > 0)
                {
                    Velocity.X -= 1;
                }
                if (Velocity.X < 0)
                {
                    Velocity.X += 1;
                }
                colliding_with.Velocity.Y = 0;
                Position.Y = colliding_with.Position.Y - Height;
            }
            if (colliding_L)
            {
                colliding_with.Velocity = Velocity / 2;
                Velocity.X /= 2;
            }

            Position += Velocity;
            Position.X -= Position.X % 10;
            Position.Y -= Position.Y % 10;
        }

    }
}

