using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Projectile : Game1
    {
        public Sprite projectile;
        public Vector2 position;
        public Vector2 velocity;
        public List<Pixel> pixels;
        public Sprite sprite;
        public float collisionAngle;
        public ProjectileType projectileType;
        public int width = 15;
        public int height = 15;
        public Dictionary<ProjectileType, float> TntEquiv = new Dictionary<ProjectileType, float> {{ ProjectileType.TNT, 1 }, { ProjectileType.C4, 30.6f }, { ProjectileType.Gunpowder, 0.7f }, { ProjectileType.Nuclear, 490245.5f }, { ProjectileType.Firework, 1.5f }, { ProjectileType.HolyHandGrenade, 50f }};
        public int mass;

        public Projectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, List<Pixel> Pix) {
            projectileType = type;
            mass = Mass;
            position = Pos;
            velocity = Vel;
            pixels = Pix;
            height *= (mass / 10);
            width *= (mass / 10);
        }
        public void Update(GameTime gameTime)
        {
            position += velocity;
            velocity.Y -= 1;
            Rectangle projectileFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            foreach (Pixel pixel in pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.Position.X, (int)pixel.Position.Y, pixel.Width, pixel.Height);
                Rectangle projectilePos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (projectilePos.Intersects(pixelPos))
                {
                    
                }
                if (projectileFuturePos.Intersects(pixelPos))
                {
                    //need to find the angle at which i strike the pixel. then lose something like 85% of E_k and bounce depending on
                    collisionAngle = (float)Math.Acos(Vector2.Dot(velocity, new Vector2(1, 0)) / Math.Sqrt((Math.Pow(velocity.X,2) + Math.Pow(velocity.Y, 2))));
                }

                //speculative contact ^
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
        public void Explode()
        {
            this.Dispose();
        }
    }
}
