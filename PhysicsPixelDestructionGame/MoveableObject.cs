using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsPixelDestructionGame
{
    class MoveableObject
    {
        public Vector2 position;
        public Vector2 velocity;
        public int width;
        public int height;
        public float health;
        public long framesAlive;
        

        public void Teleport(Vector2 targetPos)
        {
            position = targetPos;
        }
        public void Move(Vector2 movement)
        {
            position += movement;
        }
        public void SetVel(Vector2 newVel)
        {
            velocity = newVel;
        }
        public void Accelerate(Vector2 acceleration)
        {
            velocity += acceleration;
        }
        public List<Pixel> checkFloorCollision(List<Pixel> targets, Rectangle futurePos)
        {
            List<Pixel> collidingWith = new List<Pixel>();
            foreach (Pixel target in targets)
            {
                if (futurePos.Intersects(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height))) ;
            }




            return collidingWith;
        }
    }
}
