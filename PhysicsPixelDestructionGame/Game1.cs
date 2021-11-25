using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhysicsPixelDestructionGame
{
    //TODO: 
    //- Make Explosions
    //-- Add Players to a list
    //-- Pass list to Projectile Class
    //-- Allow locally edited list in projectile class to edit the global list of stuff
    //--- pixels = player.pixels; or similar
    //- add menu and playing gamestate
    //-- menu art and buttons and stuff
    //- add logic to check if pixels are close to the edge?????? What does this mean and why did i add this to my todo list 
    //- Documented design
    //- Finish analysis properly
    //- Turn-based gameplay
    public enum ProjectileType
    {
        C4,
        TNT,
        Gunpowder,
        Nuclear,
        Firework,
        HolyHandGrenade
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle window;
        private const float gravity = 16.35f; //real life g at 60fps assuming 100px = 1m
        private int pixelsMade = 0;
        private MouseState mouseState = new MouseState();
        private Vector2 mousePosVect;
        private Texture2D whitePixel;
        private Texture2D playerTexture;
        private SpriteFont font;
        private List<Player> players = new List<Player>();
        private Player player1;
        private Player player2;
        public Texture2D bombs;
        private enum GameState
        {
            TerrainCreator,
            Test
        }
        private GameState gameState = GameState.Test;
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
            bombs = Content.Load<Texture2D>("bombSprites");
            whitePixel = Content.Load<Texture2D>("whitePixel");
            playerTexture = Content.Load<Texture2D>("playerSheet");
            player1 = new Player(playerTexture, bombs);
            player2 = new Player(playerTexture, bombs);
            players.Add(player1);
            players.Add(player2);
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
            debugString = players[0].position.X.ToString() + "," + players[0].position.Y.ToString() + "," + players[0].velocity.Y.ToString();
            switch (gameState)
            {
                case GameState.TerrainCreator:

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


                    players[0].Update(gameTime, pixels, pixArray);
                    break;
                case GameState.Test:

                    if (pixels.Count != 0)
                    {
                        foreach (var pixel in pixels)
                        {
                            pixArray[(int)(pixel.Position.Y / 10), (int)(pixel.Position.X / 10)] = pixel;
                        }
                    }
                    players[0].Update(gameTime, pixels, pixArray);
                     
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
            _spriteBatch.DrawString(font, "\"WASD\" to move, \"R\" to reset, \"F\" to fire.", new Vector2(600, 500), Color.White);
            switch (gameState)
            {
                case GameState.TerrainCreator:
                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }

                    players[0].Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                   
                    break;
                case GameState.Test:
                    foreach (Pixel pixel in pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    players[0].Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                    break;

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
