using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsPixelDestructionGame
{
    class PlayerStatistics //also referred to throughout the commenting as the GUI, a box that shows certain statistics about the player
    {
        //initialise paramaters for the class
        public int currentMass;
        public int currentAngle;
        public ProjectileType currentSelectedType;
        public int currentPower;
        public bool cluster;
        public bool showing = true;
        public float currentHealth;
        public SpriteFont font;
        public Sprite bombs;
        public Sprite whitePixel;
        public Vector2 position;
        public Vector2 backgroundSize;
        public Rectangle bombSpritePosition;
        public Vector2 launchPreview;
        public Vector2 inputPosition;

        public PlayerStatistics(SpriteFont f, Texture2D b, Texture2D wp)
        {
            currentMass = 1; //set the initial values for the paramaters and create the sprites using thw passed in textures
            currentAngle = 0;
            currentSelectedType = ProjectileType.TNT;
            currentPower = 0;
            cluster = false;
            font = f;
            bombs = new Sprite(b);
            whitePixel = new Sprite(wp);
            backgroundSize = new Vector2(300, 200);
            position = new Vector2(0, 0);
            
            currentHealth = 0f;
        }

        public void Update(Vector2 inputPos, float hp, ProjectileType proj, int mass, int power, int angle, bool c, Vector2 launchVel) //called once per game tick
        {
            //every tick, update the values to be displayed to reflect the ones passed in from the player class
            position = inputPos - new Vector2(0, backgroundSize.Y + 10);
            currentHealth = hp;
            currentSelectedType = proj;
            currentMass = mass;
            currentPower = power;
            currentAngle = angle;
            cluster = c;
            launchPreview = new Vector2(3 * power * launchVel.X / launchVel.Length(), 3 * power * launchVel.Y / launchVel.Length()); //put the launch preview pixel 3 times as far from the player as the projectile will travel in its first tick alive
            inputPosition = inputPos;
        }

        public void Draw(SpriteBatch spriteBatch) //called once per draw call
        {
            if (showing) //if the GUI is showing
            {
                whitePixel.Draw(spriteBatch, position, backgroundSize, Color.White); //draw the background to the GUI in white
                if (cluster)
                {
                    whitePixel.Draw(spriteBatch, position + new Vector2(10, 10), new Vector2(10, 10), Color.Green); //if is is a cluster bomb, draw a green square in the top left of the gui
                }
                else
                {
                    whitePixel.Draw(spriteBatch, position + new Vector2(10, 10), new Vector2(10, 10), Color.Red); //if not a cluster bomb, draw a red square.
                }
                whitePixel.Draw(spriteBatch, inputPosition + launchPreview, new Vector2(5, 5), new Color(255, 0, 255)); //draw a pink pixel at the position of launchpreview
                switch (currentSelectedType) //select the correct image to draw the currently selected projectile type
                {
                    case ProjectileType.C4:
                        bombSpritePosition = new Rectangle(0, 0, 15, 15);
                        break;
                    case ProjectileType.Firework:
                        bombSpritePosition = new Rectangle(60, 0, 15, 15);
                        break;
                    case ProjectileType.Gunpowder:
                        bombSpritePosition = new Rectangle(30, 0, 15, 15);
                        break;
                    case ProjectileType.HolyHandGrenade:
                        bombSpritePosition = new Rectangle(75, 0, 15, 15);
                        break;
                    case ProjectileType.Nuclear:
                        bombSpritePosition = new Rectangle(45, 0, 15, 15);
                        break;
                    case ProjectileType.TNT:
                        bombSpritePosition = new Rectangle(15, 0, 15, 15);
                        break;
                }
                bombs.Draw(spriteBatch, bombSpritePosition, new Rectangle((int)position.X + 50, (int)position.Y + 50, 20, 20), Color.White); //draw the currently selected bomb type on the gui
                DrawText(spriteBatch, "Mass: " + currentMass, new Vector2(10, 100));// draw all of the text readouts on the gui
                DrawText(spriteBatch, "HP: " + currentHealth, new Vector2(100, 100));
                DrawText(spriteBatch, "Power: " + currentPower, new Vector2(10, 150));
                DrawText(spriteBatch, "Angle:" + currentAngle + " degrees", new Vector2(100, 150));
            }
        }
        private void DrawText(SpriteBatch sb, string text, Vector2 relativePos) //this method draws text given a position on the gui and text to draw.
        {
            relativePos += position;
            sb.DrawString(font, text, relativePos, Color.Black);
        }
    }
   
}
 