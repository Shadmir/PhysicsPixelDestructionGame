using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;


namespace PhysicsPixelDestructionGame
{
    //size of player spritesheet is 10x40
    //change angle of launch with left/right, change power with up/down, make a new object displaying current selected ProjectileType, power, hp, angle follow the player around.
    //shouldn't be too hard
    class Player : MoveableObject
    {
        public Direction facing = Direction.Right;
        private KeyboardState keyState;
        private KeyboardState lastState;
        public MouseState mouse;
        public List<Projectile> bombs = new List<Projectile>();
        public Sprite playerPicture;
        public Texture2D bombSheet;
        public ProjectileType launchType = ProjectileType.C4;
        public int launchMass = 0;
        public int launchAngle = 0;
        public int launchPower = 0;
        public bool makeCluster = false;
        public Rectangle playerFuturePos;
        public Rectangle spriteRectangle = new Rectangle(0, 0, 20, 10);
        public long lastExplosion = 0L;
        public long lastFrameJumped = 0L;
        public int jumpStrength = 10;
        public Color color = Color.White;
        public PlayerStatistics statsBoard;
        public Player(Texture2D texture, Texture2D bombs, SpriteFont f, Texture2D wp)
        {
            health = 100f;
            framesAlive = 0L;
            width = 50;
            height = 25;
            playerPicture = new Sprite(texture);
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            bombSheet = bombs;
            statsBoard = new PlayerStatistics(f, bombSheet, wp);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, spriteRectangle, new Rectangle((int)position.X, (int)position.Y, width, height), color);
            statsBoard.Draw(spriteBatch);
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
            health -= pressure;

        }
        public void Update(GameTime gameTime, SoundEffect boom, GameState state)
        {
            statsBoard.Update(position, health, launchType, launchMass, launchPower, launchAngle, makeCluster);
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
                    Accelerate(new Vector2(0, -jumpStrength));
                }
                else if (state == GameState.Test)
                {
                    Accelerate(new Vector2(0, -jumpStrength));
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
            if (keyState.IsKeyDown(Keys.Q) && lastState != keyState)
            {
                launchType++;
                if (launchType == ProjectileType.HolyHandGrenade)
                {
                    launchType = ProjectileType.C4;
                }
            } 
            if (keyState.IsKeyDown(Keys.PageUp))
            {
                launchMass++;
            }
            if (keyState.IsKeyDown(Keys.PageDown) && launchMass > 0)
            {
                launchMass--;   
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                launchAngle++;
                if (launchAngle > 359)
                {
                    launchAngle = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                launchAngle--;
                if (launchAngle < 0)
                {
                    launchAngle = 359;
                }
            }
            if(keyState.IsKeyDown(Keys.Left) && launchPower > 0)
            {
                launchPower--;
            }
            if (keyState.IsKeyDown(Keys.Right) && launchPower < 100)
            {
                launchPower++;
            }
            if (keyState.IsKeyDown(Keys.C) && lastState != keyState)
            {
                makeCluster = !makeCluster;
            }

            if(keyState.IsKeyDown(Keys.F) && lastState != keyState)
            {
                if (!makeCluster)
                {
                    //normal shot
                }
                else
                {
                    //cluster shot
                }
            }
            if (keyState.IsKeyDown(Keys.R))
            {
                Teleport(new Vector2(0, 0));
                SetVel(new Vector2(0, 0));
            }
            Accelerate(new Vector2(0, 1));
            //new projectile launching logic
            playerFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.position.X, (int)pixel.position.Y, pixel.width, pixel.height);
                Rectangle playerPos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (playerPos.Intersects(pixelPos))
                {
                    position.Y -= 10;
                }
                if (playerFuturePos.Intersects(pixelPos))
                {
                    velocity.Y = 0;
                }
                //speculative contact ^
            }

            Move(velocity);
            lastState = keyState;
        }
        private void LaunchProjectile()
        {
            Projectile launchProj = new Projectile(launchType, launchMass, position, velocity, bombSheet, facing, launchAngle, launchPower);
            PhysicsObjects.projectiles.Add(launchProj);
        }
    }
}
