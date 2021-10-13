using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    //TODO: create algorithm to search map given position of player and find the nearest suitable open space. Perhaps make player 1x1 in terms of
    //size and just cramp up their sprite if surrounded by pixels?
    class Player
    {
        private KeyboardState keyState;
        private KeyboardState lastState;
        private Vector2 freePos;
        private bool colliding = false;
        public Vector2 position;
        public Vector2 velocity;
        public Sprite playerPicture;
        public Player(Texture2D texture)
        {
            playerPicture = new Sprite(texture);
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, position, new Vector2(50, 25), Color.White);
        }
        public void Update(GameTime gameTime, List<Pixel> pixels, Pixel[,] map)
        {
            freePos = new Vector2(0, 0);
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity.X = 5;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity.X = -5;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                velocity.Y = -5;
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
            //TODO need logic for floor collisions. is still very janky
            velocity.Y += 1;
            colliding = false;
            foreach (Pixel pixel in pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.Position.X, (int)pixel.Position.Y, pixel.Width, pixel.Height);
                Rectangle playerPos = new Rectangle((int)position.X, (int)position.Y, 19, 9);
                if (playerPos.Intersects(pixelPos))
                {
                    colliding = true;
                    if (pixel.Position.Y < freePos.Y) // move up and out of intersecting pixel.
                    {
                        freePos.Y = pixel.Position.Y - 1;
                        velocity.Y = 0;
                    }
                    // need logic to find the nearest open pixel space to teleport to.

                }   
            }
            //Doing it there ^
            position += velocity;
            lastState = keyState;
        }
    }
}
