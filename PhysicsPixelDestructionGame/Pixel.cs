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

        public Pixel(Texture2D texture, Vector2 position, string material, int id, bool glue)
        {
            pixelWithTint = new Sprite(texture);
            Position = position;
            Velocity.X += r.Next(-3, 3);
            Position.X += r.Next(-10, 10);
            Position.Y += r.Next(-5, 5);
            pixelID = id;
            bonded = glue;
            switch (material)
            {
                case "concrete":
                    Strength = 10;
                    colour = new Color((192 + r.Next(-10, 10)), (192 + r.Next(-10, 10)), (192 + r.Next(-10, 10)));
                    break;

                case "steel":
                    Strength = 50;
                    colour = new Color((153 + r.Next(-10, 10)), (0 + r.Next(0, 10)), (0 + r.Next(0, 10)));
                    break;

                case "wood":
                    Strength = 1;
                    colour = new Color((112 + r.Next(-10, 10)), (45 + r.Next(-10, 10)), (0 + r.Next(0, 10)));
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
        bool CheckOccupied(List<Pixel> Pixels, Vector2 pos)
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

        public void Update(GameTime gameTime, List<Pixel> Pixels)
        {
            bool colliding_R = false;
            bool colliding_L = false;
            bool colliding_B = false;
            Pixel colliding_with = null;
            int highestY = 0;

            //unnecessary list to stop my head imploding from the coordinate system
            Vector2 Next_Top_Left = new Vector2(Position.X + Velocity.X, Position.Y + Velocity.Y);
            Vector2 Next_Top_Right = new Vector2(Position.X + Width + Velocity.X, Position.Y + Velocity.Y);
            Vector2 Next_Bottom_Left = new Vector2(Position.X + Velocity.X, Position.Y + Height + Velocity.Y);
            Velocity.Y += gravity;
            if (Position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 11)
            {
                Velocity.Y = 0;
                if (Velocity.X < 0)
                {
                    Velocity.X += 1;
                } else if (Velocity.X > 0)
                {
                    Velocity.X -= 1;
                }
            }
            if (Position.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height+1)
            {
                foreach (Pixel pixel in Pixels)
                {
                    if (pixel.Position.X == Position.X)
                    {
                        if (pixel.Position.Y > highestY)
                        {
                            highestY = (int)pixel.Position.Y;
                        }
                    }
                }
                Position.Y = highestY + Height;
            }
            foreach (Pixel pixel in Pixels)
            {
                if (pixelID!= pixel.pixelID)
                {
                    //unnecessary list 2
                    Vector2 Other_Top_Left = new Vector2(pixel.Position.X, pixel.Position.Y);
                    Vector2 Other_Top_Right = new Vector2(pixel.Position.X + pixel.Width, pixel.Position.Y);
                    Vector2 Other_Bottom_Left = new Vector2(pixel.Position.X, pixel.Position.Y + pixel.Height);
                    Vector2 Other_Bottom_Right = new Vector2(pixel.Position.X + pixel.Width, pixel.Position.Y + pixel.Height);
                    Vector2 Other_Next_Top_Left = new Vector2(pixel.Position.X + pixel.Velocity.X, pixel.Position.Y + pixel.Velocity.Y);
                    Vector2 Other_Next_Top_Right = new Vector2(pixel.Position.X + pixel.Width + pixel.Velocity.X, pixel.Position.Y + pixel.Velocity.Y);
                    Vector2 Other_Next_Bottom_Left = new Vector2(pixel.Position.X + pixel.Velocity.X, pixel.Position.Y + pixel.Height + pixel.Velocity.Y);
                    Vector2 Other_Next_Bottom_Right = new Vector2(pixel.Position.X + pixel.Width + pixel.Velocity.X, pixel.Position.Y + pixel.Height + pixel.Velocity.Y);


                    //left
                    if (Next_Top_Left.X < Other_Next_Top_Right.X && Next_Top_Right.X > Other_Next_Top_Right.X &&
                        Next_Top_Left.Y < Other_Next_Bottom_Left.Y && Next_Bottom_Left.Y > Other_Next_Top_Left.Y)
                    {
                        colliding_L = true;
                        colliding_with = pixel;
                    }

                    ////right
                    if (Next_Top_Right.X > Other_Next_Top_Left.X && Next_Top_Left.X < Other_Next_Top_Left.X &&
                        Next_Top_Left.Y < Other_Next_Bottom_Left.Y && Next_Bottom_Left.Y > Other_Next_Top_Left.Y)
                    {
                        colliding_R = true;
                        colliding_with = pixel;
                    }

                    //bottom
                    if (Position.Y + Height >= pixel.Position.Y && Position.Y + Height <= pixel.Position.Y + pixel.Height)
                    {
                        if (Position.X + Width >= pixel.Position.X && Position.X <= pixel.Position.X + pixel.Width)
                        {
                            colliding_B = true;
                            colliding_with = pixel;
                        }
                    }
                }
            }

            if (colliding_R || colliding_B || colliding_L)
            {
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
                
                if (colliding_R)
                {
                    colliding_with.Velocity.X = 0;
                    Velocity.X = 0;
                    Position.X = colliding_with.Position.X + Width;
                    colliding_L = false;
                }
                if (colliding_L)
                {
                    colliding_with.Velocity.X = 0;
                    Velocity.X = 0;
                    Position.X = colliding_with.Position.X - Width;
                }
            }

            Position += Velocity;
            Position.X -= Position.X % 11;
            Position.Y -= Position.Y % 11;
        }
        
    }
}
