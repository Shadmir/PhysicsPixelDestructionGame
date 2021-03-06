using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsPixelDestructionGame
{
    static class PhysicsObjects
    {
        //three globally accessible lists for objects that obey the laws of physics
        public static List<Pixel> pixels = new List<Pixel>();
        public static List<Player> players = new List<Player>();
        public static List<Projectile> projectiles = new List<Projectile>();
    }
}