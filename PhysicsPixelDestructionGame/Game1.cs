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
    public enum ProjectileType //Enumerable for defining the different projectile types
    {
        C4,
        TNT,
        Gunpowder,
        Nuclear,
        Firework,
        HolyHandGrenade
    }
    public enum Direction //Enumerable to define directions
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum GameState //Enumerable to define the different gamestates
    {
        TerrainCreator,
        Test,
        Menu,
        Playing,
        Won
    }
    public class Game1 : Game
    {
        //Defining parameters for the game. 
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
        private Texture2D bombs;
        private Song toLoop;
        private SoundEffect boom;
        private int playerTurn = 1;
        private GameState gameState = GameState.Menu;
        private string debugString = "";
        private int winner = 0;

        public Game1() //constructor for the game class 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // All initialization logic for the game
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; // makes the game into a fullscreen application, with dimensions equal to the current monitor resolution
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent() //loads all textures and sounds needed for the game, alongside generating the terrain and players.
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            bombs = Content.Load<Texture2D>("bombSprites");
            whitePixel = Content.Load<Texture2D>("whitePixel");
            playerTexture = Content.Load<Texture2D>("playerSheet");
            font = Content.Load<SpriteFont>("font");
            player1 = new Player(playerTexture, bombs, font, whitePixel);
            player2 = new Player(playerTexture, bombs, font, whitePixel);
            PhysicsObjects.players.Add(player1);
            PhysicsObjects.players.Add(player2);
            PhysicsObjects.players[1].position = new Vector2(1800, 115);
            PhysicsObjects.players[1].facing = Direction.Left;
            menu = Content.Load<Texture2D>("menu");
            toLoop = Content.Load<Song>("ShadmirGameSong");
            boom = Content.Load<SoundEffect>("Explosion");
            mainMen = new Menu(menu);
            PlaySound();
            GenerateTerrain(0);
        }
        protected void PlaySound() //Play the background music for the game
        {
            MediaPlayer.Play(toLoop);
            MediaPlayer.IsRepeating = true;
        }
        protected void GenerateTerrain(int level) //generate the terrain from a text file that contains a long string of text.
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
                    string[] inputSplit = inputLine.Split(":"); //split the string at every colon
                    for (int i = 0; i < inputSplit.Length - 1; i++)
                    {
                        string[] pixDefSplit = inputSplit[i].Split(","); //split the definition for each pixel at every comma
                        debugString = pixDefSplit[pixDefSplit.Length - 1];
                        PhysicsObjects.pixels.Add(new Pixel(whitePixel, new Vector2(float.Parse(pixDefSplit[0]), float.Parse(pixDefSplit[1])), int.Parse(pixDefSplit[2]))); //Create a new pixel using the parameters defined after splitting the text file definitions
                        pixelsMade++; //add one to the umber of pixels made
                    }
                }
            }
            catch (Exception e)
            {
                debugString = "No terrain file found.";
                //If there is no terrain file found under the correct name, let the user know
            }

        }

        protected override void Update(GameTime gameTime) //This method is called once per game tick, and is where the "game" takes place.
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) //If the escape key is pressed, exit the game
            {
                Exit();
            }
            mouseState = Mouse.GetState(); //Take the current position and if any buttons are currently being pressed on the mouse
            lastState = keyState; //set the last pressed key to the current key
            keyState = Keyboard.GetState(); //set the currently being pressed key to the current key
            mousePosVect = new Vector2(mouseState.X, mouseState.Y); //Create a vector for the current mouse position
            bool colliding = false;
            debugString = PhysicsObjects.players[0].position.X.ToString() + "," + PhysicsObjects.players[0].position.Y.ToString() + "," + PhysicsObjects.players[0].velocity.Y.ToString();
            switch (gameState)
            {
                case GameState.TerrainCreator: //If the game is in the terrain creator state
                    if (mouseState.LeftButton == ButtonState.Pressed) //check if the mouse is intersecting any of the current terrain pixels
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
                        if (!colliding) //if it isn't create a pixel at the current mouse position snapped to the nearest valid grid position.
                        {
                            PhysicsObjects.pixels.Add(new Pixel(whitePixel, new Vector2(mousePosVect.X - mousePosVect.X % 10, mousePosVect.Y - mousePosVect.Y % 10), pixelsMade));
                            pixelsMade++;
                        }
                    }
                    if (mouseState.RightButton == ButtonState.Pressed) //if the right mouse button is pressed, save the terrain to a text file
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


                    PhysicsObjects.players[0].Update(gameTime, boom, gameState); //Update player 1
                    break;
                case GameState.Test://if the game is in the debugging (test) state
                    if (PhysicsObjects.pixels.Count != 0) //If any pixels currently exist
                    {
                        for (int i = 0; i < PhysicsObjects.pixels.Count; i++) //Call the update method in every pixel
                        {
                            PhysicsObjects.pixels[i].Update(gameTime);
                        }
                    }
                    PhysicsObjects.players[0].Update(gameTime, boom, gameState); //Update the player
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++) //Check every projectile
                    {
                        if (PhysicsObjects.projectiles[i].exploded)
                        {
                            PhysicsObjects.projectiles.RemoveAt(i); //if the projectile has been flagged to be removed, remove it
                        }
                        else
                        {
                            PhysicsObjects.projectiles[i].Update(gameTime); //If they do not need to be removed, call the update method for the projectile
                        }
                    }
                    debugString = PhysicsObjects.players[0].health.ToString();
                    break;

                case GameState.Menu: //if the game is currently in the menu state
                    gameState = mainMen.Update(); //call the update method for the menu
                    break;

                case GameState.Playing: //if the game is in the playing state
                    if (playerTurn % 2 != 0) // if it is player 1's turn
                    {
                        PhysicsObjects.players[0].Update(gameTime, boom, gameState); //update player 1
                        PhysicsObjects.players[1].UpdatePhysics(gameTime); //update the physics only of player 2
                        PhysicsObjects.players[0].statsBoard.showing = true; //show player 1's gui
                        PhysicsObjects.players[1].statsBoard.showing = false; //don't show player 2's gui
                        UpdateProjectiles(gameTime); //update every projectile
                        if (keyState.IsKeyDown(Keys.Enter) && lastState != keyState) //if the Enter key is pressed, add one to the current turn counter.
                        {
                            playerTurn++;
                        }
                    }
                    else if (playerTurn % 2 == 0) // if it is player 2's turn
                    {
                        PhysicsObjects.players[1].Update(gameTime, boom, gameState); //update player 2
                        PhysicsObjects.players[0].UpdatePhysics(gameTime); // update the physics of player 1
                        PhysicsObjects.players[1].statsBoard.showing = true; //show player 2's gui
                        PhysicsObjects.players[0].statsBoard.showing = false; //hide player 1's gui
                        UpdateProjectiles(gameTime); // update every projectile
                        if (keyState.IsKeyDown(Keys.Enter) && lastState != keyState) //if the enter key is pressed
                        {
                            foreach (Projectile projectile in PhysicsObjects.projectiles) //explode every projectile
                            {
                                projectile.Explode(boom);
                            }
                            foreach (Pixel pixel in PhysicsObjects.pixels) //check for dead pixels
                            {
                                if (pixel.health <= 0)
                                {
                                    pixelIDsToRemove.Add(pixel.pixelID); 
                                }
                            }
                            for (int i = 0; i < PhysicsObjects.pixels.Count; i++)
                            {
                                foreach (int id in pixelIDsToRemove) //remove dead pixels
                                {
                                    if (PhysicsObjects.pixels[i].pixelID == id)
                                    {
                                        PhysicsObjects.pixels.RemoveAt(i);
                                    }
                                }
                            }
                            RemoveProjectiles(); //remove explkoded projectiles
                            while (PhysicsObjects.players[0].velocity.Length() != 0 && PhysicsObjects.players[1].velocity.Length() != 0)
                            {
                                foreach (Player player in PhysicsObjects.players) //update both players until they stop moving
                                {
                                    player.UpdatePhysics(gameTime);
                                }
                            }
                            playerTurn++; //add one to the current turn counter
                        }
                    }
                    if(player1.health <= 0 && player2.health > 0) //check if player 2 has won and if so, set them the winner and end the game
                    {
                        winner = 2;
                        gameState = GameState.Won;
                    }
                    if (player2.health <= 0 && player1.health > 0) //check if player 1 has won and if so, set them the winner and end the game
                    {
                        winner = 1;
                        gameState = GameState.Won;
                    }
                    if (player2.health <= 0 && player1.health <= 0) //check if it is a draw and if so, set the game as a draw and end the game
                    {
                        winner = 3;
                        gameState = GameState.Won;
                    }
                    break;

                case GameState.Won: //if the game has been won
                    System.Threading.Thread.Sleep(10000); //wait for 10 seconds
                    PhysicsObjects.players = new List<Player>(); //reset the game 
                    PhysicsObjects.pixels = new List<Pixel>();
                    PhysicsObjects.projectiles = new List<Projectile>();
                    LoadContent();
                    gameState = GameState.Menu;
                    break;
                default:
                    break;
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }
        protected void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in PhysicsObjects.projectiles) //iterate through each projectile, and if it has not exploded, update it
            {
                if (projectile.exploded != true)
                {
                    projectile.Update(gameTime);
                }
            }
        }

        protected void RemoveProjectiles()
        {
            for (int i = 0; i < PhysicsObjects.projectiles.Count; i++) //iterate through each projectile, and if it has exploded, remove it
            {
                if (PhysicsObjects.projectiles[i].exploded)
                {
                    PhysicsObjects.projectiles.RemoveAt(i);
                }
            }
        }

        protected override void Draw(GameTime gameTime) //This draw call is what renders every sprite in the game, and is called independently of the update call
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); //unrender all sprites, and set the background colour to blue

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null); //Make the game able to draw all sprites. The parameters passed into this method change the sampling type to nearest neighbour, allowing for clean upscaling of the game's pixel art style.
            switch (gameState)
            {
                case GameState.TerrainCreator: //if the game is in terrain creator mode 
                    foreach (Pixel pixel in PhysicsObjects.pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime); // draw every pixel on to the screen
                    }

                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime); //draw player 1 
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black); //draw debugging information to the screen
                    break;
                case GameState.Test: //if the game is in the testing state
                    foreach (Pixel pixel in PhysicsObjects.pixels)
                    {
                        pixel.Draw(_spriteBatch, gameTime); //draw every terrain pixel
                    }
                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime); //draw player 1
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++)
                    {
                        PhysicsObjects.projectiles[i].Draw(_spriteBatch, gameTime); //draw every projectile
                    }
                    _spriteBatch.DrawString(font, debugString, new Vector2(200, 200), Color.Black); //draw debugging information to the screen
                    break;

                case GameState.Playing: //if the game is in the playing state
                    _spriteBatch.DrawString(font, "\"WASD\" to move, \"R\" to reset, \"F\" to fire, \"Q\" & \"E\" to switch projectile type, up, down, left, right, pgup, pgdwn to change launch.", new Vector2(600, 550), Color.White); //Draw the controls to the screen
                    foreach (Pixel pixel in PhysicsObjects.pixels) //draw every pixel
                    {
                        pixel.Draw(_spriteBatch, gameTime);
                    }
                    PhysicsObjects.players[0].Draw(_spriteBatch, gameTime); //draw player 1
                    PhysicsObjects.players[1].Draw(_spriteBatch, gameTime); //draw player 2
                    for (int i = 0; i < PhysicsObjects.projectiles.Count; i++) //draw every projectiles
                    {
                        PhysicsObjects.projectiles[i].Draw(_spriteBatch, gameTime);
                    }
                    break;

                case GameState.Menu: //if the game is in the menu state
                    mainMen.Draw(_spriteBatch); //draw the menu
                    break;

                case GameState.Won: //if the game has been won
                    GraphicsDevice.Clear(new Color(184, 51, 73)); //clear all drawn objects and make the background red
                    string toWrite = "The winner is: "; //construct the correct string to display the winner
                    if(winner == 1)
                    {
                        toWrite += "player 1!";
                    }
                    if (winner == 2)
                    {
                        toWrite += "player 2!";
                    }
                    if (winner == 3)
                    {
                        toWrite += "both!";
                    }
                    _spriteBatch.DrawString(font, toWrite, new Vector2(500, 500), Color.White); //draw the string that contains the winner to the screen
                    break;
            }
            _spriteBatch.End(); //end the capability to change sprites on the screen for this draw call?

            base.Draw(gameTime); //call the draw method in the game class
        }
    }
}
