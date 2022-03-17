using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PhysicsPixelDestructionGame
{
    class ClusterProjectile : Projectile //a special projectile that explodes recursively multiple times
    {
        public ClusterProjectile(ProjectileType type, int Mass, Vector2 Pos, Vector2 Vel, Texture2D sheet, Direction initDir, Vector2 launchVel) : base(type, Mass, Pos, Vel, sheet, initDir, launchVel)
        {
            //Inhertit the consatructor from the projectile class and call it.
        }
        public override void Explode(SoundEffect boom)
        {
            Split(boom);
            base.Explode(boom);
            //When the projectile is called to explode, it splits and then calls the explode method from the parent class.
        }
        private void Split(SoundEffect boom)
        {
            //A recusrive method to split the bomb in half and cause an explosion from the smaller bomb.
            mass /= 2;
            if (mass >= 1)
            {
                Split(boom);
                Explode(boom);
            }

        }
    }
}
