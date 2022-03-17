using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsPixelDestructionGame
{
    class MoveableObject //this is a superclass that pixels, players, and projectiles inherit from
    {
        //initialising attributes that all objects that move need
        public Vector2 position;
        public Vector2 velocity;
        public int width;
        public int height;
        public float health;
        public long framesAlive;
        

        public void Teleport(Vector2 targetPos) //allow moving objects to instantly move to a certain point
        {
            position = targetPos;
        }
        public void Move(Vector2 movement) //allow moving objects to move by a certain amount
        {
            position += movement;
        }
        public void SetVel(Vector2 newVel) //allow moving objects to instantly change their velocity
        {
            velocity = newVel;
        }
        public void Accelerate(Vector2 acceleration) //allow moving objects to change their velocity by a certain amount
        {
            velocity += acceleration;
        }
    }
}
