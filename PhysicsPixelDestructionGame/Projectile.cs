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
        public Dictionary<ProjectileType, float> TntEquiv = new Dictionary<ProjectileType, float> { { ProjectileType.TNT, 1 }, { ProjectileType.C4, 30.6f }, { ProjectileType.Gunpowder, 0.7f }, { ProjectileType.Nuclear, 490245.5f }, { ProjectileType.Firework, 1.5f }, { ProjectileType.HolyHandGrenade, 50f }
        };
        public int mass;

        public Projectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel) {
            projectileType = type;
            mass = Mass;
            position = Pos;
            velocity = Vel;
        }
        public void Update(GameTime gameTime)
        {
            position += velocity;
            velocity.Y -= 1;
        }
    }
}
