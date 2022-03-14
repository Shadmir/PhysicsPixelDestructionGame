using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace PhysicsPixelDestructionGame
{
    class Projectile : MoveableObject
    {
        public Sprite projectile;
        public bool exploded = false;
        public Rectangle spritebounds;
        public ProjectileType projectileType;
        public readonly Dictionary<ProjectileType, float> TntEquiv = new Dictionary<ProjectileType, float> { { ProjectileType.TNT, 1 }, { ProjectileType.C4, 30.6f }, { ProjectileType.Gunpowder, 0.7f }, { ProjectileType.Nuclear, 490245.5f }, { ProjectileType.Firework, 1.5f }, { ProjectileType.HolyHandGrenade, 50f } };
        public int mass;
        private bool colliding = false;

        public Projectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, Texture2D sheet, Direction initDir, int angle, int power)
        {
            width = 15;
            height = 15;
            projectileType = type;
            projectile = new Sprite(sheet);
            mass = Mass;
            position = Pos;
            velocity = Vel;
            // need to add logic to launch projectile at correct angle with overall magnitude of 10root2
            //to do this, take each component of the vector, divide by magnitude of overall vector, and times by 10root2
            switch (initDir)
            {
                case Direction.Right:
                    velocity += new Vector2(10, -10);
                    break;
                case Direction.Left:
                    velocity += new Vector2(-10, -10);
                    break;
            }
            height *= (int)Math.Pow(mass, 1 / 2);
            width *= (int)Math.Pow(mass, 1 / 2);
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
        }
        public void Update(GameTime gameTime)
        {
            colliding = false;
            Move(velocity);
            Accelerate(new Vector2(0, 1));
            Rectangle projectileFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.position.X, (int)pixel.position.Y, pixel.width, pixel.height);
                Rectangle projectilePos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (projectilePos.Intersects(pixelPos))
                {
                    Move(new Vector2(0, -10));
                }
                if (projectileFuturePos.Intersects(pixelPos))
                {
                    colliding = true;
                    SetVel(new Vector2(0 ,0));
                }
                //speculative contact ^
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            projectile.Draw(spriteBatch, spritebounds, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
        }
        public void Explode(SoundEffect boom)
        {
            float equiv = TntEquiv[projectileType];
            float Rg;
            for (int i = 0; i < PhysicsObjects.players.Count; i++)
            {
                float distance = (PhysicsObjects.players[i].position - position).Length();
                Debug.WriteLine("Distance from player in pixels: " + distance);
                distance /= 100; //previously established that 10px = 1m. could scale to 100px = 1m if needed
                Debug.WriteLine("Distance from player in meters: " + distance);

                Debug.WriteLine("TNT Equivalent: " + equiv);
                Rg = distance / (float)Math.Pow((double)equiv, (double)1.0 / 3.0);

                Debug.WriteLine("RG: " + Rg);
                PhysicsObjects.players[i].Damage(Rg);

            }
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                float distance = (pixel.position - position).Length();
                distance /= 100;
                Rg = distance / (float)Math.Pow((double)equiv, (double)1.0 / 3.0);
                pixel.Damage(Rg);
            }
            exploded = true;
            boom.Play();
        }
    }
}
