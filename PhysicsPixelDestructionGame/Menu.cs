using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhysicsPixelDestructionGame
{
    class Menu
    {
        //menu button coords - (113, 131) -> (322, 336)
        public Sprite menuArt;
        public MouseState mouseState;
        public Menu(Texture2D tex)
        {
            menuArt = new Sprite(tex);
        }
        public GameState Update()
        {
            mouseState = Mouse.GetState();
            if (113 <= mouseState.X && 322 >= mouseState.X && 131 <= mouseState.Y && 336 >= mouseState.Y && mouseState.LeftButton == ButtonState.Pressed)
            {
                return GameState.Playing;
            }
            return GameState.Menu;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            menuArt.Draw(spriteBatch, new Vector2(0, 0), new Vector2(1920, 1080), Color.White);
        }
    }
}
