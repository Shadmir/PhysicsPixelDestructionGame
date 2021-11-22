using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace PhysicsPixelDestructionGame
{
    //size of player spritesheet is 10x40
    class Player
    {
        private Direction facing = Direction.Right;
        private KeyboardState keyState;
        private KeyboardState lastState;
        private long lastProjLaunch = 0L;
        public MouseState mouse;
        public Vector2 position;
        public List<Projectile> bombs = new List<Projectile>();
        public Vector2 velocity;
        public Sprite playerPicture;
        public Texture2D bombSheet;
        public Rectangle playerFuturePos;
        public Rectangle spriteRectangle = new Rectangle(0, 0, 20, 10);
        public int width = 50;
        public int height = 25;
        public long framesAlive = 0L;
        public long lastFrameJumped = 0L;
        public int jumpStrength = 10;
        public int health = 100;
        public Color color = Color.White;
        public Player(Texture2D texture, Texture2D bombs)
        {
            playerPicture = new Sprite(texture);
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            bombSheet = bombs;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, spriteRectangle, new Rectangle((int)position.X, (int)position.Y, width, height), color);
            if (bombs.Count > 0)
            {
                foreach (var bomb in bombs)
                {
                    bomb.Draw(spriteBatch, gameTime);
                }
            }
        }
        public void Update(GameTime gameTime, List<Pixel> pixels, Pixel[,] map)
        {
            framesAlive++;
            keyState = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.D))
            {
                facing = Direction.Right;
                velocity.X = 5;
                spriteRectangle = new Rectangle(20, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                facing = Direction.Left;
                velocity.X = -5;
                spriteRectangle = new Rectangle(0, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                //if (framesAlive - lastFrameJumped > 25)
                //{
                    lastFrameJumped = framesAlive;
                    velocity.Y -= jumpStrength;
                //}
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity.Y = 5;
            }
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S) && (lastState.IsKeyDown(Keys.W) || lastState.IsKeyDown(Keys.S)))
            {
                velocity.Y = 0;
            }
            if (keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D))
            {
                velocity.X = 0;
            }
            if (keyState.IsKeyDown(Keys.R))
            {
                position = new Vector2(0, 0);
                velocity = new Vector2(0, 0);
            }
            velocity.Y += 1;
            if(keyState.IsKeyDown(Keys.F) && framesAlive - lastProjLaunch > 25)
            {
                lastProjLaunch = framesAlive;
                bombs.Add(new Projectile(ProjectileType.TNT, 1, position, velocity, pixels, bombSheet, facing)) ;
            }

            if (velocity.Y >= 10)
            {
                //velocity.Y = 9;
            }
            if (velocity.Y <= -10)
            {
                //velocity.Y = -9;
            }

            playerFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height);
            foreach (Pixel pixel in pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.Position.X, (int)pixel.Position.Y, pixel.Width, pixel.Height);
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
            if (bombs.Count > 0)
            {
                foreach (Projectile bomb in bombs)
                {
                    bomb.Update(gameTime);
                }
            }
            position += velocity;
            lastState = keyState;
        }
    }
}
