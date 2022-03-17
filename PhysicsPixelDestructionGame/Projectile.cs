using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace PhysicsPixelDestructionGame
{
    class Projectile : MoveableObject
    {
        //Defining attributes for the class.
        public Sprite projectile;
        public bool exploded = false;
        public Rectangle spritebounds;
        public ProjectileType projectileType;
        public readonly Dictionary<ProjectileType, float> TntEquiv = new Dictionary<ProjectileType, float> { { ProjectileType.TNT, 1 }, { ProjectileType.C4, 30.6f }, { ProjectileType.Gunpowder, 0.7f }, { ProjectileType.Nuclear, 490245.5f }, { ProjectileType.Firework, 1.5f }, { ProjectileType.HolyHandGrenade, 50f } };
        //^ Storing the projectile types in a dictionary for quick lookup of the equivalent mass of TNT/
        public int mass;

        public Projectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, Texture2D sheet, Direction initDir, Vector2 launchVec)
        {
            width = (int)(15 * Math.Sqrt(Mass));
            height = (int)(15 * Math.Sqrt(Mass));
            //Defining the size of the projectile, as this is 2D then mass must be proportional to area and not volume.
            projectileType = type;
            //Assigning the correct type to the projectile
            projectile = new Sprite(sheet);
            //Creating a sprite to render the projectile.
            mass = Mass;
            position = Pos;
            velocity = Vel;
            velocity += launchVec;
            //Assigning the final parameters to the projectile.
            switch (projectileType)
            {
                case ProjectileType.C4:
                    spritebounds = new Rectangle(0, 0, 15, 15);
                    break;
                case ProjectileType.Firework:
                    spritebounds = new Rectangle(60, 0, 15, 15);
                    break;
                case ProjectileType.Gunpowder:
                    spritebounds = new Rectangle(30, 0, 15, 15);
                    break;
                case ProjectileType.HolyHandGrenade:
                    spritebounds = new Rectangle(75, 0, 15, 15);
                    break;
                case ProjectileType.Nuclear:
                    spritebounds = new Rectangle(45, 0, 15, 15);
                    break;
                case ProjectileType.TNT:
                    spritebounds = new Rectangle(15, 0, 15, 15);
                    break;
            }
            //Switch statements allows the projectile to easily be assigned the correct coordinates
            //on the spritesheet to take the picture from. 
        }
        public void Update(GameTime gameTime)
        //This method is called 60 times per second, and is where the logic of the game takes place.
        {
            Move(velocity); //Moves the player by their current velocity.
            Accelerate(new Vector2(0, 1)); //Accelerates the player downwards, due to gravity
            Rectangle projectileFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            //^ Simulates the position of the projectile during the next tick, to allow for speculative contacts. 
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.position.X, (int)pixel.position.Y, pixel.width, pixel.height);
                Rectangle projectilePos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (projectilePos.Intersects(pixelPos))
                {
                    //Checking if the projectile has somehow found its way inside the floor, and if so,
                    //moving it out of the floor.
                    Move(new Vector2(0, -10));
                }
                if (projectileFuturePos.Intersects(pixelPos))
                {
                    SetVel(new Vector2(0, 0));
                    //This is speculative contact, whereby the projectile class checks if it will contact the ground in the future and 
                    //if it does, make it so that it does not go through the ground.
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Call the draw method from the sprite so that the projectile shows up on the screen
            projectile.Draw(spriteBatch, spritebounds, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
        }
        public virtual void Explode(SoundEffect boom)
        {
            //This is the logic for the projectile exploding and sending a pressure wave out to all other objects.
            float equiv = TntEquiv[projectileType]; //This pulls the equivalent mass in TNT of the currently selected type. In other words, finds the relative explosive power between the currently selected explosive and TNT.
            float Rg;
            for (int i = 0; i < PhysicsObjects.players.Count; i++)
            {
                float distance = (PhysicsObjects.players[i].position - position).Length(); //Find the distance to both players
                distance /= 100; //Scale the distance so 100px = 1m
                Rg = distance / (float)Math.Pow((double)equiv, (double)1.0 / 3.0); //Find the distance scaled definition of the explosion.
                PhysicsObjects.players[i].Damage(Rg); //Send the pressure wave to each player.
            }
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                float distance = (pixel.position - position).Length(); //Find the distance to ever pixel of floor.
                distance /= 100; //Scale the distance to 100px = 1m
                Rg = distance / (float)Math.Pow((double)equiv, (double)1.0 / 3.0); //Find the distance scaled definition of the explosion
                pixel.Damage(Rg); //Send the pressure wave to each pixel
            }
            exploded = true; //Flag the projectile to be removed at the end of the turn
            boom.Play(); //Play the explosion sound effect.
        }
    }
}
