using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsPixelDestructionGame
{
    class Sprite //a class designed to make it easier to render a texture
    {
        public Texture2D Texture { get; private set; } //sprites have a texture that can be read by anything but can only be set by the constructor
        

        public Sprite(Texture2D texture) //the constructor sets the texture of the sprite
        {
            Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color) //draw the whole texture at a defined position and size with an optional colour tint
        {
            spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle inputRectangle, Rectangle outputRectangle, Color color) //draw a portion of the texture at a defined position and size with optional colour tint
        {
            spriteBatch.Draw(Texture, outputRectangle, inputRectangle, color);
        }
    }
}
