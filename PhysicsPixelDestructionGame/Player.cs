using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace PhysicsPixelDestructionGame
{ 
    class Player : MoveableObject //inheriting from the MoveableObject superclass, contains everything to do with the player
    {
        //Defining attributes for the class;
        public Direction facing = Direction.Right;
        private KeyboardState keyState;
        private KeyboardState lastState;
        public MouseState mouse;
        public List<Projectile> bombs = new List<Projectile>();
        public Sprite playerPicture;
        public Texture2D bombSheet;
        public ProjectileType launchType = ProjectileType.C4;
        public int launchMass = 1;
        public int launchAngle = 0;
        public int launchPower = 1;
        public bool makeCluster = false;
        public Rectangle playerFuturePos;
        public Rectangle spriteRectangle = new Rectangle(0, 0, 20, 10);
        public long lastExplosion = 0L;
        public long lastFrameJumped = 0L;
        public int jumpStrength = 10;
        public Color color = Color.White;
        public PlayerStatistics statsBoard;
        private bool colliding = false;
        public Vector2 launchVel = new Vector2(1, 0); // Set the default direction to launch the projectile to be in the positive x-direction
        public Player(Texture2D texture, Texture2D bombs, SpriteFont f, Texture2D wp)
        {
            health = 100f; //Player starts out with 100 health
            framesAlive = 0L; //Initialise the counter for the number of game ticks since the start of the game
            width = 50; //The player is 50 pixels wide
            height = 25; //The player is 25 pixels tall
            playerPicture = new Sprite(texture); //Create a sprite for the game to register.
            position = new Vector2(0, 0); //Set the initial position of the player
            velocity = new Vector2(0, 0); //Set the initial velocity of the player
            bombSheet = bombs; //Assign the texture of the projectiles to a variable
            statsBoard = new PlayerStatistics(f, bombSheet, wp); //Create the GUI that follows the player around.
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerPicture.Draw(spriteBatch, spriteRectangle, new Rectangle((int)position.X, (int)position.Y, width, height), color); //Draw the player every draw call
            statsBoard.Draw(spriteBatch); //Draw the GUI every draw call
        }
        public void Damage(float Rg)
        {
            float pressure;
            if (Rg <= 0.3)
            {
                pressure = 1.379f / Rg + 0.542f / (float)Math.Pow(Rg, 2) - 0.035f / (float)Math.Pow(Rg, 3) + 0.006f / (float)Math.Pow(Rg, 4);
            }
            else if (0.3 <= Rg && Rg <= 1)
            {
                pressure = 0.607f / Rg - 0.032f / (float)Math.Pow(Rg, 2) + 0.209f / (float)Math.Pow(Rg, 3);
            }
            else
            {
                pressure = 0.065f / Rg + 0.397f / (float)Math.Pow(Rg, 2) + 0.322f / (float)Math.Pow(Rg, 3);
            }
            //Calculate the peak overpressure of the pressure wave at the position of the player.
            health -= pressure; //Apply the overpressure to subtract the health from the player

        }
        public void UpdatePhysics(GameTime gameTime)
        {
            colliding = false;
            Accelerate(new Vector2(0, 1)); //Apply gravity to the player
            playerFuturePos = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, height); //find the position that the player will be in one game tick in the future.
            foreach (Pixel pixel in PhysicsObjects.pixels)
            {
                Rectangle pixelPos = new Rectangle((int)pixel.position.X, (int)pixel.position.Y, pixel.width, pixel.height);
                Rectangle playerPos = new Rectangle((int)position.X, (int)position.Y, width, height);
                if (playerPos.Intersects(pixelPos))
                {
                    position.Y -= 10; //Check if the player has somehow entered a pixel, and move out of it.
                }
                if (playerFuturePos.Intersects(pixelPos))
                {
                    velocity.Y = 0; //If the player is going to intersect a pixel, don't allow it to move vertically.
                    colliding = true;
                }
                //speculative contact ^
            }

            Move(velocity); //Move the player by their current velocity.
        }
        public void Update(GameTime gameTime, SoundEffect boom, GameState state)
        {
            statsBoard.Update(position, health, launchType, launchMass, launchPower, launchAngle, makeCluster, launchVel); //Pass all the paramaters displayed by the GUI to it
            framesAlive++;//Add one to the number of ticks since the start of the game.
            keyState = Keyboard.GetState(); //Check if any buttons are being pressed on the kayboard.
            mouse = Mouse.GetState(); //Check the position of the mouse and if any buttons are being pressed.
            if (keyState.IsKeyDown(Keys.D)) //If the D key is being pressed, change the direction the player is facing and move them right
            {
                facing = Direction.Right;
                SetVel(new Vector2(5, velocity.Y));
                spriteRectangle = new Rectangle(20, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.A)) //If the A key is being pressed, make the player face left and begin moving them left.
            {
                facing = Direction.Left;
                SetVel(new Vector2(-5, velocity.Y));
                spriteRectangle = new Rectangle(0, 0, 20, 10);
            }
            if (keyState.IsKeyDown(Keys.W)) //If the W key is being pressed, accelerate the player upwards.
            {
                if (framesAlive - lastFrameJumped > 25 && state == GameState.Playing && colliding == true) //Conditions for the player being able to jump.
                {
                    lastFrameJumped = framesAlive;
                    Accelerate(new Vector2(0, -jumpStrength));
                }
                else if (state == GameState.Test)
                {
                    Accelerate(new Vector2(0, -jumpStrength));
                }
            }
            if (keyState.IsKeyDown(Keys.S)) //Allow the player to move down at a constant speed if they choose to.
            {
                SetVel(new Vector2(velocity.X, 5));
            }
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S) && (lastState.IsKeyDown(Keys.W) || lastState.IsKeyDown(Keys.S))) //If the player is not touching any vertical movement buttons, cancel their vertical movement
            {
                SetVel(new Vector2(velocity.X, 0));
            }
            if (keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D)) //if a player is not holding a horizontal movement button, cancel their horizontal movement
            {
                SetVel(new Vector2(0, velocity.Y));
            }
            if (keyState.IsKeyDown(Keys.Q) && lastState != keyState && launchType != ProjectileType.HolyHandGrenade) //if the player presses Q, select the next projectile type
            {
                launchType++;
            }
            if (keyState.IsKeyDown(Keys.E) && lastState != keyState && launchType != ProjectileType.C4) //if the player presses E, switch to the previous projectile type
            {
                launchType--;
            }
            if (keyState.IsKeyDown(Keys.PageUp) && launchMass < 10) //if the player presses PgUp, increase launch mass up to max of 10
            {
                launchMass++;
            }
            if (keyState.IsKeyDown(Keys.PageDown) && launchMass > 1) //if the player presses PgDwn, decrease launch mass to minimum of 1
            {
                launchMass--;
            }
            if (keyState.IsKeyDown(Keys.Up)) //If the up arrow key is pressed, increase the launch angle by one degree
            {
                launchAngle++;
                if (launchAngle > 359)
                {
                    launchAngle = 0;
                }
                launchVel = new Vector2((float)(launchVel.X * Math.Cos(Math.PI / 180) + launchVel.Y * -1 * Math.Sin(Math.PI / 180)), (float)(launchVel.X * Math.Sin(Math.PI / 180) + launchVel.Y * Math.Cos(Math.PI / 180)));
                //^ This line uses a standard rotation matrix transformation to rotate the angle of the vector by 1 degree while maintaining magnitude
                
            }
            if (keyState.IsKeyDown(Keys.Down)) //if the down arrow is pressed, decrease the launch angle by 1 degree
            {
                launchAngle--;
                if (launchAngle < 0)
                {
                    launchAngle = 359;
                }
                launchVel = new Vector2((float)(launchVel.X * Math.Cos(-Math.PI / 180) + launchVel.Y * -1 * Math.Sin(-Math.PI / 180)), (float)(launchVel.X * Math.Sin(-Math.PI / 180) + launchVel.Y * Math.Cos(-Math.PI / 180)));
                //^ This line uses a standard rotation matrix transformation to rotate the angle of the vector by negative 1 degree while maintaining magnitude
            }
            if (keyState.IsKeyDown(Keys.Left) && launchPower > 1) //if the left arrow key is pressed, reduce the power of the launch by 1 if it is currently greater than 1
            {
                launchPower--;
                launchVel = new Vector2(launchPower * launchVel.X / launchVel.Length(), launchPower * launchVel.Y / launchVel.Length());
                //^ This line normalises the vector to the current selected launch power, changing the magnitude while maintaining direction.
            }
            if (keyState.IsKeyDown(Keys.Right) && launchPower < 100) //If the right arrow key is pressed, increase the power of the launch by 1 if it is less than 100
            {
                launchPower++;
                launchVel = new Vector2(launchPower * launchVel.X / launchVel.Length(), launchPower * launchVel.Y / launchVel.Length());
                //^ This line normalises the vector to the current selected launch power, changing the magnitude while maintaining direction.
            }
            if (keyState.IsKeyDown(Keys.C) && lastState != keyState) //If the C key is pressed, change if the next bomb launched is a cluster bomb or not
            {
                makeCluster = !makeCluster;
            }

            if (keyState.IsKeyDown(Keys.F) && lastState != keyState) //if the F key is pressed, launch a projectile
            {

                if (!makeCluster)
                {
                    PhysicsObjects.projectiles.Add(new Projectile(launchType, launchMass, position, velocity, bombSheet, facing, launchVel));
                    //if the current selected projectile type is not a cluster bomb, create a normal projectile with all of the current parameters and add it to the global list of projectiles.
                }
                else
                {
                    PhysicsObjects.projectiles.Add(new ClusterProjectile(launchType, launchMass, position, velocity, bombSheet, facing, launchVel));
                    //if the current selected type is a cluster bomb, create a cluster bomb with all of the current parameters and add it to the global list of projectiles using polymorphism
                }

            } 
            if (keyState.IsKeyDown(Keys.R)) //If the R key is pressed, reset the physics of the player
            {
                Teleport(new Vector2(0, 0)); //Teleport the player to the top left corner of the screen
                SetVel(new Vector2(0, 0)); //Stop the player from moving
            }
            UpdatePhysics(gameTime); //Update the physics of the player
            lastState = keyState; //Set the last pressed key to the currently pressed key.
        }
    }
}
