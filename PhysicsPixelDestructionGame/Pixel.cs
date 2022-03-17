using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Pixel : MoveableObject //pixels inherit methods and attributes from moveable objects, they are collideable squares that make up the bulk of the terrain
    {
        //initialising attributed for the class
        public Sprite pixelWithTint;
        public Color colour;
        public int pixelID;
        Random r = new Random();
        public Pixel(Texture2D texture, Vector2 Position, int id) //constructor to set default parameters for the pixel
        {
            width = 10;
            height = 10;
            pixelWithTint = new Sprite(texture); //creating a sprite to be rendered
            position = Position;
            pixelID = id;
            health = 100;
            colour = new Color(100 + r.Next(-10, 10), 40 + r.Next(-10, 10), r.Next(-10, 10));//randomising the brown of the pixels slightly
        }

        public Pixel(Sprite sprite, Vector2 Position, Color color, int id) //creating a pixel given we already know everything about me
        {
            pixelWithTint = sprite;
            colour = color;
            position = Position;
            pixelID = id; 
            health = 100;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) //method draws the pixel every draw call
        {
            pixelWithTint.Draw(spriteBatch, position, new Vector2(width, height), colour);
        }

        public void Update(GameTime gameTime) //pixels do not have any update logic
        {  
            
        }

        public void Damage(float Rg)
        {
            float pressure;
            if (Rg <= 0.3)
            {
                pressure = 1.379f / Rg + 0.542f / (float)Math.Pow(Rg, 2) - 0.035f / (float)Math.Pow(Rg, 3) + 0.006f / (float)Math.Pow(Rg, 4);
            }
            else if (0.3 <= Rg && Rg <= 1)
            {
                pressure = 0.607f / Rg - 0.032f / (float)Math.Pow(Rg, 2) + 0.209f / (float)Math.Pow(Rg, 3);
            }
            else
            {
                pressure = 0.065f / Rg + 0.397f / (float)Math.Pow(Rg, 2) + 0.322f / (float)Math.Pow(Rg, 3);
            }
            //Calculate the peak overpressure of the pressure wave at the position of the player.
            health -= pressure; //apply the pressure to the pixel
        }
    }
}

