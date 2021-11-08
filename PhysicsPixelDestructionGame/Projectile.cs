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
        public readonly Dictionary<ProjectileType, float> TntEquiv = new Dictionary<ProjectileType, float> {{ ProjectileType.TNT, 1 }, { ProjectileType.C4, 30.6f }, { ProjectileType.Gunpowder, 0.7f }, { ProjectileType.Nuclear, 490245.5f }, { ProjectileType.Firework, 1.5f }, { ProjectileType.HolyHandGrenade, 50f }};
        public int mass;
        private bool collidingL = false;
        private bool collidingR = false;
        private bool collidingT = false;
        private bool collidingB = false;

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
            collidingL = false;
            collidingR = false;
            collidingT = false;
            collidingB = false;
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
                    Rectangle pixLeft = new Rectangle(pixelPos.Left, pixelPos.Top, 1, pixel.Height);
                    Rectangle pixTop = new Rectangle(pixelPos.Left, pixelPos.Top, pixel.Width, 1);
                    Rectangle pixRight = new Rectangle(pixelPos.Right, pixelPos.Top, 1, pixel.Height);
                    Rectangle pixBottom = new Rectangle(pixelPos.Left, pixelPos.Bottom, pixel.Width, 1);
                    for (int x = 0; x < (int)velocity.X; x++)
                    {
                        for (int y = 0; y < (int)velocity.Y; y++)
                        {
                            Point posToTest = new Point((int)position.X + x, (int)position.Y + y);
                            if(pixLeft.Contains(posToTest))
                            {
                                collidingL = true;
                            }
                            if (pixRight.Contains(posToTest))
                            {
                                collidingR = true;
                            }
                            if (pixTop.Contains(posToTest))
                            {
                                collidingT = true;
                            }
                            if (pixBottom.Contains(posToTest))
                            {
                                collidingB = true;
                            }
                            if (collidingL && collidingR)
                            {
                                float distL = (new Vector2(pixLeft.X, pixLeft.Y) - position).Length();
                                float distR = (new Vector2(pixRight.X, pixRight.Y) - position).Length();
                                if (distL > distR)
                                {
                                    collidingL = false;
                                }
                                else
                                {
                                    collidingR = false;
                                }
                            }
                            if (collidingT && collidingB)
                            {
                                float distT = (new Vector2(pixTop.X, pixTop.Y) - position).Length();
                                float distB = (new Vector2(pixBottom.X, pixBottom.Y) - position).Length();
                                if (distT > distB)
                                {
                                    collidingT = false;
                                }
                                else
                                {
                                    collidingR = false;
                                }
                            }
                        }
                    }
                }
                //speculative contact ^
            }
            if (collidingT || collidingB)
            {
                velocity.Y = -0.8f * velocity.Y;
                velocity.X = 0.8f * velocity.X;
            }
            if (collidingL || collidingR)
            {
                velocity.X = -0.8f * velocity.X;
                velocity.Y = 0.8f * velocity.Y;
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
