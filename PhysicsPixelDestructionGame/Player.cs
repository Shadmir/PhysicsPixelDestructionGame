using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Player
    {
        public Sprite playerPicture;
        public Player(Texture2D texture)
        {
            playerPicture = new Sprite(texture);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, new Vector2(0, 0), new Vector2(100, 50), Color.White);
        }
        public void Update(GameTime gameTime)
        {

        }
    }
}
