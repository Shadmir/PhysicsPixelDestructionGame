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
        public ClusterProjectile (ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, Texture2D sheet, Direction initDir, Vector2 launchVel) : base(type, Mass, Pos, Vel, sheet, initDir, launchVel)
        {
            initDirection = initDir;
        }
        public override void Explode(SoundEffect boom)
        {
            Split(boom);
            base.Explode(boom);
        }
        private void Split(SoundEffect boom)
        {
            mass /= 2;
            if (mass >= 1)
            {
                Split(boom);
                Explode(boom);
            }

        }
    }
}
