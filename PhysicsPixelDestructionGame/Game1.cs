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
        private List<Pixel> pixels = new List<Pixel>();
        private Vector2 mousePosVect;
        private Texture2D whitePixel;

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
            switch(gameState)
            {
                case "test":
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Pixel pixel = new Pixel(whitePixel, mousePosVect, new Color(0, 255, 0), "concrete", pixels, pixelsMade);
                        pixels.Add(pixel);
                        pixelsMade++;
                    }
                    foreach (Pixel pixel1 in pixels)
                    {
                        pixel1.Update(gameTime);
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
                    break;
            }
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
