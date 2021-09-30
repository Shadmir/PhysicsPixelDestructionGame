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
        private Vector2 position;
        public Sprite playerPicture;
        public Player(Texture2D texture)
        {
            playerPicture = new Sprite(texture);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, position, new Vector2(50, 25), Color.White);
        }
        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X += 5;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X -= 5;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                position.Y -= 5;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y += 5;
            }
            lastState = keyState;
        }
    }
}
