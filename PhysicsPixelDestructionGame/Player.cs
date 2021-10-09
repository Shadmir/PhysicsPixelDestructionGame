using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Player
    {
        private KeyboardState keyState;
        private KeyboardState lastState;
        public Vector2 position;
        public Vector2 velocity = new Vector2 (0, 0);
        public Sprite playerPicture;
        public Player(Texture2D texture)
        {
            playerPicture = new Sprite(texture);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, position, new Vector2(50, 25), Color.White);
        }
        public void Update(GameTime gameTime, List<Pixel> pixels)
        {
            
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
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S))
            {
                velocity.Y = 0;
            }
            if (keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D))
            {
                velocity.X = 0;
            }
            //TODO need logic for floor collisions
            int placeholder = 0;
            if ((position.Y - (position.Y % 10)) / 10 == placeholder)
            {

            }
            //Doing it there ^
            position += velocity;
            lastState = keyState;
        }
    }
}
