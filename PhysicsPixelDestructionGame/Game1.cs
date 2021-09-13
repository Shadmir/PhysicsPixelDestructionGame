using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PhysicsPixelDestructionGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle window;
        private const float gravity = 16.35f; //real life g at 60fps assuming 100px = 1m
        private string gameState = "test";
        private int pixelsMade = 0;
        private MouseState mouseState = new MouseState();
        private Vector2 mousePosVect;
        private Texture2D whitePixel;
        private SpriteFont font;
        private List<Pixel> pixels = new List<Pixel>();
        Color bg;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; // makes fullscreen & whatever current monitor res is
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            window = GraphicsDevice.Viewport.Bounds;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = Content.Load<Texture2D>("whitePixel");
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            mouseState = Mouse.GetState();
            mousePosVect = new Vector2(mouseState.X, mouseState.Y);
            bool colliding = false;
            switch (gameState)
            {
                case "test":
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Pixel pixel = new Pixel(whitePixel, mousePosVect, "concrete", pixelsMade);
                        Rectangle pixelEdge = new Rectangle((int)(pixel.Position.X + pixel.Velocity.X), (int)(pixel.Position.Y + pixel.Velocity.Y), (int)pixel.Width, (int)pixel.Height);
                        foreach (Pixel pixel1 in pixels)
                        {

                            if (pixel1.pixelID != pixel.pixelID)
                            {
                                Rectangle otherPixelEdge = new Rectangle((int)(pixel1.Position.X + pixel1.Velocity.X), (int)(pixel1.Position.Y + pixel1.Velocity.Y), (int)pixel1.Width, (int)pixel1.Height);
                                if (pixelEdge.Intersects(otherPixelEdge))
                                {
                                    colliding = true;
                                }               
                            }
                        }
                        if (!colliding)
                        {
                            pixels.Add(pixel);
                            pixelsMade++;
                        }
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        if (pixels.Count > 0)
                        {
                            pixels.Remove(pixels[0]);
                            pixelsMade--;
                        }
                    }

                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Update(gameTime, pixels);
                    }
                    break;

                default:
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            switch (gameState)
            {
                case "test":
                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    _spriteBatch.DrawString(font, pixelsMade.ToString(), new Vector2(200, 200), Color.Black);
                    break;

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
