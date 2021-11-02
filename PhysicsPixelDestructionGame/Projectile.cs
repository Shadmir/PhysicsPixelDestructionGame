using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PhysicsPixelDestructionGame
{
    class Projectile : Game1
    {
        public Sprite projectile;
        public Vector2 position;
        public Vector2 velocity;
        public ProjectileType projectileType;
        //public Dictionary<ProjectileType, int> TntEquiv = new Dictionary<ProjectileType, int> { {ProjectileType.TNT, 1 }, {},{}
        //};
        public int mass;

        public Projectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel) {
            projectileType = type;
            mass = Mass;
            position = Pos;
            velocity = Vel;
        }
    }
}
