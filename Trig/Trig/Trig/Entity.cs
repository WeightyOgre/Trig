using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Trig
{
    class Entity
    {

        //texture, position, destination=null, color, origin, scale, spriteEffects, something(float = 0f);

        Texture2D entityTexture;
        public Texture2D EntityTexture
        {
            get { return entityTexture; }
            set { entityTexture = value; }
        }
        
        Vector2 entityPosition;
        public Vector2 EntityPosition
        {
            get { return entityPosition; }
            set { entityPosition = value; }
        }

        Color aColor;
        public Color AColor
        {
            get { return aColor; }
            set { aColor = value; }
        }

        float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        float currentRotation;
        public float CurrentRotation
        {
            get { return currentRotation; }
            set { currentRotation = value; }
        }

        private float rotationSpeed;
        public float RoationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        private float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private float acceleration;
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        private float horsePower;
        public float HorsePower
        {
            get { return horsePower; }
            set { horsePower = value; }
        }

        private float gravity;
        public float Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        private float targetRotation;
        public float TargetRotation
        {
            get { return targetRotation; }
            set { targetRotation = value; }
        }

        private float turningArc;
        public float TurningArc
        {
            get { return turningArc; }
            set { turningArc = value; }
        }

        public Entity()
        {
            rotationSpeed = 0.05f;
            speed = 0.00f;
            acceleration = 0.00f;
            gravity = 0.0f;
            scale = 1.0f;
            horsePower = 0.01f;
            turningArc = 0.0f;
        }

        public void rotateLeft()
        {
            targetRotation -= rotationSpeed;
        }

        public void rotateRight()
        {
            targetRotation += rotationSpeed;
        }

        public void accelerate()
        {
            acceleration += horsePower /4;
        }

        public void decelerate()
        {
            acceleration -= horsePower *4;
        }

        public void minMaxAcceleration()
        {
            if (acceleration <= 0)
            {
                acceleration = 0;
                //speed = 0;
            }
            if (acceleration >= 1)
            {
                acceleration = 1;
            }
        }

        public void minMaxSpeed()
        {
            if (speed >= 5)
            {
                speed = 5;
            }
            if (speed <= -3)
            {
                speed = -3;
            }
        }

        public void stop()
        {
            acceleration = 0;
            speed = 0;
        }

        public string getRotationAsDegrees()
        {
          return Convert.ToString( MathHelper.ToDegrees(currentRotation));
        }

        public void move()
        {
            Vector2 direction = new Vector2((float)Math.Cos(currentRotation),
                                            (float)Math.Sin(currentRotation));

            direction.Normalize();
            entityPosition += direction * speed;
        }

        public void anotherMove()
        {
            //http://www.rodedev.com/tutorials/gamephysics/
            //calculate x and y scales (this will give the x/y equivalent of the angle
            float scaleX = (float)Math.Cos(currentRotation);//x is cosine of the angle
            float scaleY = (float)Math.Sin(currentRotation);//y is the sin of the angle

            float velocityX = (speed * scaleX);//speed times scale is our x velocity
            float velocityY = (speed * scaleY);//speed times scale is our y velocity

            entityPosition.X = entityPosition.X + velocityX;
            entityPosition.Y = entityPosition.Y + velocityY;
        }

        //this is enough for a top down car game.
        public void addAcceleration()
        {
            //calculate x and y scales (this will give the x/y equivalent of the angle
            float scaleX = (float)Math.Cos(currentRotation);//x is cosine of the angle
            float scaleY = (float)Math.Sin(currentRotation);//y is the sin of the angle

            speed = speed + acceleration;//continously add acceleration to speed

            float velocityX = (speed * scaleX);
            float velocityY = (speed * scaleY);            

            entityPosition.X = entityPosition.X + velocityX;
            entityPosition.Y = entityPosition.Y + velocityY;

        }

        public void addGravity()
        {
            //max speed
            if(speed >= 10)
            {
                speed = 10;
            }

            //calculate x and y scales (this will give the x/y equivalent of the angle
            float scaleX = (float)Math.Cos(currentRotation);//x is cosine of the angle
            float scaleY = (float)Math.Sin(currentRotation);//y is the sin of the angle

            speed = speed + acceleration;//continously add acceleration to speed

            float velocityX = (speed * scaleX);
            float velocityY = (speed * scaleY);

            velocityY = velocityY + gravity;

            entityPosition.X = entityPosition.X + velocityX;
            entityPosition.Y = entityPosition.Y + velocityY;
        }

        //http://www.raywenderlich.com/35866/trigonometry-for-game-programming-part-1
        public void rotationBlending()
        {

            updateTurningArc();

            //calculate x and y scales (this will give the x/y equivalent of the angle
            float scaleX = (float)Math.Cos(currentRotation);//x is cosine of the angle
            float scaleY = (float)Math.Sin(currentRotation);//y is the sin of the angle

            speed = speed + acceleration;//continously add acceleration to speed

            float velocityX = (speed * scaleX);
            float velocityY = (speed * scaleY);

            velocityY = velocityY + gravity;

            entityPosition.X = entityPosition.X + velocityX;
            entityPosition.Y = entityPosition.Y + velocityY;
            const float RotationBlendFactor = 0.5f;
            //if current rotation is more than target, rotate left until it is equal
            if (currentRotation > targetRotation)
            {
                currentRotation -= rotationSpeed * RotationBlendFactor;
            }
            //turn right
            else if (currentRotation < targetRotation)
            {
                currentRotation += rotationSpeed * RotationBlendFactor;
            }

            //turning arc right
            if ((float)MathHelper.ToDegrees(targetRotation) > (float)MathHelper.ToDegrees(currentRotation) + turningArc)
            {
                targetRotation = currentRotation + (float)MathHelper.ToRadians(turningArc);
            }
            //turning arc left
            if ((float)MathHelper.ToDegrees(targetRotation) < (float)MathHelper.ToDegrees(currentRotation) - turningArc)
            {
                targetRotation = currentRotation - (float)MathHelper.ToRadians(turningArc);
            }

            minMaxSpeed();
            minMaxAcceleration();
        }

        public void updateTurningArc()
        {
            if (speed == 5)
            {
                turningArc = 5;
            }
            else if (speed >= 4)
            {
                turningArc = 15;
            }
            else if (speed >= 3 && speed < 4)
            {
                turningArc = 25;
            }
            else if (speed >= 2 && speed < 3)
            {
                turningArc = 35;
            }
            else if (speed >= 1 && speed < 2)
            {
                turningArc = 45;
            }
            else if (speed >= 0 && speed < 1)
            {
                turningArc = 0;
            }

        }

    }
}
