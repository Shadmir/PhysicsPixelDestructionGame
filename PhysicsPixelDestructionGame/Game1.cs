using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

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
        private Texture2D playerTexture;
        private SpriteFont font;
        private Player player;
        private Pixel[,] pixArray;
        private List<Pixel> pixels = new List<Pixel>();
        private string debugString = "";

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
            //pixArray = new Pixel[(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10) + 1, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 10) + 1];
            pixArray = new Pixel[(1080 / 10) + 1, (1920 / 10) + 1];
            window = GraphicsDevice.Viewport.Bounds;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = Content.Load<Texture2D>("whitePixel");
            playerTexture = Content.Load<Texture2D>("playerSheet");
            player = new Player(playerTexture);
            font = Content.Load<SpriteFont>("font");
            GenerateTerrain(0);
            // TODO: use this.Content to load your game content here
            //need to generate terrain here??? or perhaps change method call for generating terrain to link to start menu button

        }
        protected void GenerateTerrain(int level)
        {
            string inputLine = "";
            try
            {
                using (StreamReader sr = new StreamReader("terrain.txt"))
                {
                    for (int i = 0; i < level + 1; i++)
                    {
                        inputLine = sr.ReadLine();
                    }
                    string[] inputSplit = inputLine.Split(":");
                    for (int i = 0; i < inputSplit.Length - 1; i++)
                    {
                        string[] pixDefSplit = inputSplit[i].Split(",");
                        debugString = pixDefSplit[pixDefSplit.Length - 1];
                        pixels.Add(new Pixel(whitePixel, new Vector2(float.Parse(pixDefSplit[0]), float.Parse(pixDefSplit[1])), int.Parse(pixDefSplit[2])));
                        pixelsMade++;

                    }
                }
            } catch (Exception e)
            {
                debugString = "No terrain file found.";

            }

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
            debugString = player.position.X.ToString() + "," + player.position.Y.ToString() + "," + player.velocity.Y.ToString();
            switch (gameState)
            {
                case "terraincreator":

                    if (pixels.Count != 0)
                    {
                        foreach (var pixel in pixels)
                        {
                            pixArray[(int)(pixel.Position.Y / 10), (int)(pixel.Position.X / 10)] = pixel;
                        }
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        foreach (var pixel in pixels)
                        {
                            if (new Rectangle((int)mousePosVect.X, (int)mousePosVect.Y, 10, 10).Intersects(new Rectangle((int)pixel.Position.X, (int)pixel.Position.Y, 10, 10)))
                            {
                                colliding = true;
                            }
                            else
                            {
                                colliding = false;
                            }
                        }
                        if (!colliding)
                        {
                            pixels.Add(new Pixel(whitePixel, new Vector2(mousePosVect.X - mousePosVect.X % 10, mousePosVect.Y - mousePosVect.Y % 10), pixelsMade));
                            pixelsMade++;
                        }
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        using (StreamWriter sw = new StreamWriter("terrain.txt"))
                        {
                            string line = "";
                            foreach (var pixel in pixels)
                            {
                                line += "" + pixel.Position.X.ToString() + "," + pixel.Position.Y.ToString() + "," + pixel.pixelID.ToString() + ":";
                            }
                            sw.Write(line);
                        }
                    }


                    player.Update(gameTime, pixels, pixArray);
                    break;
                case "test":

                    if (pixels.Count != 0)
                    {
                        foreach (var pixel in pixels)
                        {
                            pixArray[(int)(pixel.Position.Y / 10), (int)(pixel.Position.X / 10)] = pixel;
                        }
                    }
                    player.Update(gameTime, pixels, pixArray);
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            switch (gameState)
            {
                case "terraincreator":
                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    player.Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                    break;
                case "test":
                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    player.Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                    break;

            }
            _spriteBatch.End();
            //TODO add menu and playing gamestate
            //TODO add logic to check if pixels are close to the edge
            base.Draw(gameTime);
        }
    }
}
