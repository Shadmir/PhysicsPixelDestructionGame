using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PhysicsPixelDestructionGame
{
    class ClusterProjectile : Projectile
    {
        Direction initDirection;
        int angle, power;
        public ClusterProjectile (ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, Texture2D sheet, Direction initDir, int initAngle, int initPower) : base(type, Mass, Pos, Vel, sheet, initDir, initAngle, initPower)
        {
            initDirection = initDir;
            angle = initAngle;
            power = initPower;
        }
        public override void Explode(SoundEffect boom)
        {
            Split(boom);
            base.Explode(boom);
        }
        private void Split(SoundEffect boom)
        {
            if (mass >= 1)
            {
                Split(boom);
            }
            for (int i = 0; i < 2; i++)
            {
                ClusterProjectile child = new ClusterProjectile(projectileType, mass, position, velocity, projectile.Texture, initDirection, angle, power);
                child.Explode(boom);
            }
        }
    }
}
