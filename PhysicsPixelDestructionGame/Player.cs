using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;


namespace PhysicsPixelDestructionGame
{
    //size of player spritesheet is 10x40
    //TODO: make the player check for collisions by using the new collision method in the MoveableObject class.
    class Player : MoveableObject
    {
        private Direction facing = Direction.Right;
        private KeyboardState keyState;
        private KeyboardState lastState;
        private long lastProjLaunch = 0L;
        public MouseState mouse;
        public List<Projectile> bombs = new List<Projectile>();
        public Sprite playerPicture;
        public Texture2D bombSheet;
        public Rectangle playerFuturePos;
        public Rectangle spriteRectangle = new Rectangle(0, 0, 20, 10);
        public long lastExplosion = 0L;
        public long lastFrameJumped = 0L;
        public int jumpStrength = 10;
        public Color color = Color.White;
        public Player(Texture2D texture, Texture2D bombs)
        {
            health = 100f;
            framesAlive = 0L;
            width = 50;
            height = 25;
            playerPicture = new Sprite(texture);
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            bombSheet = bombs;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, spriteRectangle, new Rectangle((int)position.X, (int)position.Y, width, height), color);

        }
        public void Damage(float Rg)
        {
            if (Rg > 0.3f)
            {
                Console.WriteLine("Damaging player with " + ((0.082f / Rg) + 0.26f / (float)Math.Pow(Rg, 2) + 0.69f / (float)Math.Pow(Rg, 3)) + " Mega Pascal of pressure.");
                health -= 1f * ((0.082f / Rg) + 0.26f / (float)Math.Pow(Rg, 2) + 0.69f / (float)Math.Pow(Rg, 3));
            }
            else
            {
                Console.WriteLine("Damaging player with: " + (1.379 / Rg + 0.543 / Math.Pow(Rg, 2) - 0.035 / Math.Pow(Rg, 3) + 0.006 / Math.Pow(Rg, 4)) + " mega pascal of pressure");
                health -= 1f * (float)(1.379 / Rg + 0.543 / Math.Pow(Rg, 2) - 0.035 / Math.Pow(Rg, 3) + 0.006 / Math.Pow(Rg, 4));
            }
        }
        public void Update(GameTime gameTime, SoundEffect boom, GameState state)
        {
            framesAlive++;
            keyState = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.D))
            {
                facing = Direction.Right;
                SetVel(new Vector2(5, velocity.Y));
                spriteRectangle = new Rectangle(20, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                facing = Direction.Left;
                SetVel(new Vector2(-5, velocity.Y));
                spriteRectangle = new Rectangle(0, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                if (framesAlive - lastFrameJumped > 25 && state == GameState.Playing)
                {
                    lastFrameJumped = framesAlive;
                    velocity.Y -= jumpStrength;
                }
                else
                {
                    velocity.Y -= jumpStrength;
                }
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                SetVel(new Vector2(velocity.X, 5));
            }
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S) && (lastState.IsKeyDown(Keys.W) || lastState.IsKeyDown(Keys.S)))
            {
                SetVel(new Vector2(velocity.X, 0));
            }
            if (keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D))
            {
                SetVel(new Vector2(0, velocity.Y));
            }
            if (keyState.IsKeyDown(Keys.B) && framesAlive - lastExplosion > 25)
            {
                lastExplosion = framesAlive;
                if (PhysicsObjects.projectiles.Count > 0)
                {
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
                    {
                        PhysicsObjects.projectiles[i].Explode(boom);
                    }
                }
            }
            if (keyState.IsKeyDown(Keys.R))
            {
                Teleport(new Vector2(0, 0));
                SetVel(new Vector2(0, 0));
            }
            Accelerate(new Vector2(0, 1));
            if (keyState.IsKeyDown(Keys.T) && framesAlive - lastProjLaunch > 25)
            {
                lastProjLaunch = framesAlive;
                PhysicsObjects.projectiles.Add(new Projectile(ProjectileType.TNT, 1, position, velocity, bombSheet, facing));
            }
            if (keyState.IsKeyDown(Keys.F) && framesAlive - lastProjLaunch > 25)
            {
                lastProjLaunch = framesAlive;
                PhysicsObjects.projectiles.Add(new Projectile(ProjectileType.Firework, 1, position, velocity, bombSheet, facing));
            }
            if (keyState.IsKeyDown(Keys.N) && framesAlive - lastProjLaunch > 25)
            {
                lastProjLaunch = framesAlive;
                PhysicsObjects.projectiles.Add(new Projectile(ProjectileType.Nuclear, 1, position, velocity, bombSheet, facing));
            }
            playerFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.position.X, (int)pixel.position.Y, pixel.width, pixel.height);
                Rectangle playerPos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (playerPos.Intersects(pixelPos))
                {

                    //position.X -= 10;
                    position.Y -= 10;
                }
                if (playerFuturePos.Intersects(pixelPos))
                {
                    //velocity.X = 0;

                    velocity.Y = 0;
                }

                //speculative contact ^
            }

            Move(velocity);
            lastState = keyState;
        }
    }
}
