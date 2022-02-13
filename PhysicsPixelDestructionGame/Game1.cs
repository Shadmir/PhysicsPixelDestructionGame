using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhysicsPixelDestructionGame
{
    //HELLO SIR
    //THIS IS VERY CLOSE TO BEING DONE, PROBABLY CLOSER THAN THE TODO MAKES IT LOOK
    // I KNOW HOW I WANT TO DO EVERYTHING, I JUST NEED TO PUT CODE TO PAPER


    // UPDATE 13/2/22. I was going to finish coding this weekend but my dad instead decided i must spend the weekend replumbing
    // the central heating -_-


    //TODO: 
    //- ^^^^ checking if they want to fall
    //refactor rest of movable objects to use proper movement methods
    //- Documented design
    // making the projectile path simulated before the projectile fires
    // power slider
    // change the angle
    // change the  projectile you can shoot
    // mass slider ??????????
    // damage work properly ???? possibly done
    //cluster bombs for polymorphism and recursion
    /* 
    - move players with f=ma when they get hit by an explosion
    - destructible terrain
    */
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
    public enum GameState
    {
        TerrainCreator,
        Test,
        Menu,
        Playing
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int pixelsMade = 0;
        private KeyboardState keyState = new KeyboardState();
        private KeyboardState lastState = new KeyboardState();
        private MouseState mouseState = new MouseState();
        private Vector2 mousePosVect;
        private Texture2D whitePixel;
        private Texture2D playerTexture;
        private Texture2D menu;
        private SpriteFont font;
        private Player player1;
        private Player player2;
        private Menu mainMen;
        private List<int> pixelIDsToRemove = new List<int>();
        public Texture2D bombs;
        public Song toLoop;
        public SoundEffect boom;
        public int playerTurn = 1;
        public GameState gameState { get; private set; } = GameState.Menu;
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
            PhysicsObjects.players.Add(player1);
            PhysicsObjects.players.Add(player2);
            font = Content.Load<SpriteFont>("font");
            menu = Content.Load<Texture2D>("menu");
            toLoop = Content.Load<Song>("ShadmirGameSong");
            boom = Content.Load<SoundEffect>("Explosion");
            mainMen = new Menu(menu);
            PlaySound();
            GenerateTerrain(0);
        }
        protected void PlaySound()
        {
            MediaPlayer.Play(toLoop);
            MediaPlayer.IsRepeating = true;
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
                        PhysicsObjects.pixels.Add(new Pixel(whitePixel, new Vector2(float.Parse(pixDefSplit[0]), float.Parse(pixDefSplit[1])), int.Parse(pixDefSplit[2])));
                        pixelsMade++;
                    }
                }
            }
            catch (Exception e)
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
            lastState = keyState;
            keyState = Keyboard.GetState();
            mousePosVect = new Vector2(mouseState.X, mouseState.Y);
            bool colliding = false;
            debugString = PhysicsObjects.players[0].position.X.ToString() + "," + PhysicsObjects.players[0].position.Y.ToString() + "," + PhysicsObjects.players[0].velocity.Y.ToString();
            switch (gameState)
            {
                case GameState.TerrainCreator:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        foreach (var pixel in PhysicsObjects.pixels)
                        {
                            if (new Rectangle((int)mousePosVect.X, (int)mousePosVect.Y, 10, 10).Intersects(new Rectangle((int)pixel.position.X, (int)pixel.position.Y, 10, 10)))
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
                            PhysicsObjects.pixels.Add(new Pixel(whitePixel, new Vector2(mousePosVect.X - mousePosVect.X % 10, mousePosVect.Y - mousePosVect.Y % 10), pixelsMade));
                            pixelsMade++;
                        }
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        using (StreamWriter sw = new StreamWriter("terrain.txt"))
                        {
                            string line = "";
                            foreach (var pixel in PhysicsObjects.pixels)
                            {
                                line += "" + pixel.position.X.ToString() + "," + pixel.position.Y.ToString() + "," + pixel.pixelID.ToString() + ":";
                            }
                            sw.Write(line);
                        }
                    }


                    PhysicsObjects.players[0].Update(gameTime, boom, gameState);
                    break;
                case GameState.Test:

                    if (PhysicsObjects.pixels.Count != 0)
                    {
                        for (int i = 0; i < PhysicsObjects.pixels.Count; i++)
                        {
                            PhysicsObjects.pixels[i].Update(gameTime);
                        }
                    }
                    PhysicsObjects.players[0].Update(gameTime, boom, gameState);
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
                    {
                        if (PhysicsObjects.projectiles[i].exploded)
                        {
                            PhysicsObjects.projectiles.RemoveAt(i);
                        }
                        else
                        {
                            PhysicsObjects.projectiles[i].Update(gameTime);
                        }
                    }
                    debugString = PhysicsObjects.players[0].health.ToString();
                    break;

                case GameState.Menu:
                    gameState = mainMen.Update();
                    break;

                case GameState.Playing:
                    if (playerTurn % 2 != 0)
                    {
                        PhysicsObjects.players[0].Update(gameTime, boom, gameState);
                        UpdateProjectiles(gameTime);
                        if (keyState.IsKeyDown(Keys.Enter) && lastState != keyState)
                        {
                            playerTurn++;
                        }
                    }
                    else if (playerTurn % 2 == 0)
                    {
                        PhysicsObjects.players[1].Update(gameTime, boom, gameState);
                        UpdateProjectiles(gameTime);
                        if (keyState.IsKeyDown(Keys.Enter) && lastState != keyState)
                        {
                            foreach (Projectile projectile in PhysicsObjects.projectiles)
                            {
                                projectile.Explode(boom);
                            }
                            foreach (Pixel pixel in PhysicsObjects.pixels)
                            {
                                if (pixel.health <= 0)
                                {
                                    pixelIDsToRemove.Add(pixel.pixelID);
                                }
                            }
                            for (int i = 0; i < PhysicsObjects.pixels.Count; i++)
                            {
                                foreach (int id in pixelIDsToRemove)
                                {
                                    if (PhysicsObjects.pixels[i].pixelID == id)
                                    {
                                        PhysicsObjects.pixels.RemoveAt(i);
                                    }
                                }
                            }
                            RemoveProjectiles();
                            while (PhysicsObjects.players[0].velocity.Length() != 0 && PhysicsObjects.players[1].velocity.Length() != 0)
                            {
                                foreach (Player player in PhysicsObjects.players)
                                {
                                    player.Update(gameTime, boom, gameState);
                                }
                            }
                            playerTurn++;
                        }
                    }

                    break;

                default:
                    break;
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }
        protected void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in PhysicsObjects.projectiles)
            {
                projectile.Update(gameTime);
            }
        }

        protected void RemoveProjectiles()
        {
            for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
            {
                if (PhysicsObjects.projectiles[i].exploded)
                {
                    PhysicsObjects.projectiles.RemoveAt(i);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            _spriteBatch.DrawString(font, "\"WASD\" to move, \"R\" to reset, \"F\" to fire.", new Vector2(600, 500), Color.White);
            switch (gameState)
            {
                case GameState.TerrainCreator:
                    foreach (Pixel pixel in PhysicsObjects.pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }

                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);

                    break;
                case GameState.Test:
                    foreach (Pixel pixel in PhysicsObjects.pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime);
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
                    {
                        PhysicsObjects.projectiles[i].Draw(_spriteBatch, gameTime);
                    }
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                    break;

                case GameState.Playing:
                    foreach (Pixel pixel in PhysicsObjects.pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime);
                    PhysicsObjects.players[1].Draw(_spriteBatch, gameTime);
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
                    {
                        PhysicsObjects.projectiles[i].Draw(_spriteBatch, gameTime);
                    }
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black);
                    break;

                case GameState.Menu:
                    mainMen.Draw(_spriteBatch);
                    break;

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
