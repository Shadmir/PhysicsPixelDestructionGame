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
        public bool colliding = false;
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
            switch (projectileType) {
                case ProjectileType.C4:

                    break;
                case ProjectileType.Firework:

                    break;
                case ProjectileType.Gunpowder:

                    break;
                case ProjectileType.HolyHandGrenade:

                    break;
                case ProjectileType.Nuclear:

                    break;
                case ProjectileType.TNT:

                    break;
            }
        }
        public void Update(GameTime gameTime)
        {
            colliding = false;
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
                    Vector2 normalisedVel = velocity;
                    normalisedVel.Normalize();
                    collisionAngle = (float)Math.Acos(Vector2.Dot(normalisedVel, new Vector2(1, 0)) / Math.Sqrt((Math.Pow(normalisedVel.X,2) + Math.Pow(normalisedVel.Y, 2))));
                    colliding = true;
                }
                //speculative contact ^
            }
            if (colliding)
            {
                if ((collisionAngle > 315 || collisionAngle < 45) || (135 < collisionAngle && collisionAngle < 225))
                {
                    velocity.X = -0.8f * velocity.X;
                    velocity.Y = 0.8f * velocity.Y;
                }
                if ((45 < collisionAngle && collisionAngle < 135) || (225 < collisionAngle && collisionAngle < 315))
                {
                    velocity.Y = -0.8f * velocity.Y;
                    velocity.X = 0.8f * velocity.X;
                }
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
