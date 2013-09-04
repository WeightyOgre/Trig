using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Trig
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int screenResolutionWidth = 1920;
        const int screenResolutionHeight = 1080;

        Color aColor;

        TextDisplay debugText;

        Entity anEntity;

        Texture2D testTexture;

        Texture2D wheelTexture;

        Vector2 origin;

        Texture2D obstacle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = screenResolutionWidth;
            graphics.PreferredBackBufferHeight = screenResolutionHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            this.IsMouseVisible = true;

            aColor = Color.White;

            debugText = new TextDisplay(this.Content, "test", new Vector2(0, 20));

            anEntity = new Entity();
            anEntity.EntityPosition = new Vector2(100,100);
            anEntity.AColor = Color.Red;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            anEntity.EntityTexture = Content.Load<Texture2D>("Entity40x20");

            testTexture = Content.Load<Texture2D>("1x1");
            wheelTexture = Content.Load<Texture2D>("blank10x5");

            obstacle = Content.Load<Texture2D>("blank20x20");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                if (anEntity.Speed < 0)
                {
                    anEntity.rotateRight();
                }
                else
                {
                    anEntity.rotateLeft();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                if (anEntity.Speed < 0)
                {
                    anEntity.rotateLeft();
                }
                else
                {
                    anEntity.rotateRight();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
            {
                anEntity.accelerate();
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
            {
                anEntity.decelerate();
            }

            //anEntity.move();
            //anEntity.anotherMove();
            //anEntity.addAcceleration();
            //anEntity.addGravity();
            anEntity.rotationBlending();

            updateBoundary();

            debugText.stringValue = Convert.ToString("Rotation Value: " + anEntity.CurrentRotation + "\r\n" + 
                                                    "Rotation Value in Degrees: " + anEntity.getRotationAsDegrees() + "\r\n" + 
                                                    "Speed: " + anEntity.Speed + "\r\n" + 
                                                    "Acceleration: " + anEntity.Acceleration + "\r\n" + 
                                                    "Rotation Speed: " + anEntity.RoationSpeed + "\r\n" + 
                                                    "Gravity: " + anEntity.Gravity + "\r\n" + 
                                                    "Target Rotation: " + anEntity.TargetRotation);

            base.Update(gameTime);
        }

        public void updateBoundary()
        {
            if (anEntity.EntityPosition.X < 0)
            {
                Vector2 tempVector = new Vector2();
                tempVector = anEntity.EntityPosition;
                tempVector.X = 0;
                anEntity.EntityPosition = tempVector;
                anEntity.stop();
            }
            if (anEntity.EntityPosition.X > 1920)
            {
                Vector2 tempVector = new Vector2();
                tempVector = anEntity.EntityPosition;
                tempVector.X = 1920;
                anEntity.EntityPosition = tempVector;
                anEntity.stop();
            }
            if (anEntity.EntityPosition.Y < 0)
            {
                Vector2 tempVector = new Vector2();
                tempVector = anEntity.EntityPosition;
                tempVector.Y = 0;
                anEntity.EntityPosition = tempVector;
                anEntity.stop();
            }
            if (anEntity.EntityPosition.Y > 1080)
            {
                Vector2 tempVector = new Vector2();
                tempVector = anEntity.EntityPosition;
                tempVector.Y = 1080;
                anEntity.EntityPosition = tempVector;
                anEntity.stop();
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(aColor);

            // TODO: Add your drawing code here

            //texture, position, destination=null, color, rotation, origin, scale, spriteEffects, something(float = 0f);
            spriteBatch.Begin();

                origin = new Vector2(anEntity.EntityTexture.Width, anEntity.EntityTexture.Height / 2);
                //draw wheels
                Vector2 wheelOrigin = new Vector2(+11, 12);
                Vector2 testvector = anEntity.EntityPosition;
                spriteBatch.Draw(wheelTexture, testvector, new Rectangle(0, 0, 10, 5), Color.Black, anEntity.TargetRotation, wheelOrigin, anEntity.Scale, SpriteEffects.None, 0f);

                //draw wheel 2

                Vector2 wheel2Origin = new Vector2(+11, -7);
                spriteBatch.Draw(wheelTexture, new Vector2(anEntity.EntityPosition.X, anEntity.EntityPosition.Y), new Rectangle(0, 0, 10, 5), Color.Black, anEntity.TargetRotation, wheel2Origin, anEntity.Scale, SpriteEffects.None, 0f);

                //draw the entity
                spriteBatch.Draw(anEntity.EntityTexture, anEntity.EntityPosition, null, anEntity.AColor, anEntity.CurrentRotation, origin, anEntity.Scale, SpriteEffects.None, 0f);

                //draw current rotation line
                Vector2 anotherOrigin = new Vector2(0, 0);
                spriteBatch.Draw(testTexture, new Vector2(anEntity.EntityPosition.X, anEntity.EntityPosition.Y), new Rectangle((int)anEntity.EntityPosition.X, (int)anEntity.EntityPosition.Y, 100, 1), Color.Green, anEntity.CurrentRotation, anotherOrigin, anEntity.Scale, SpriteEffects.None, 0f);
                //draw target rotation line
                Vector2 yetAnotherOrigin = new Vector2(0, 0);
                spriteBatch.Draw(testTexture, new Vector2(anEntity.EntityPosition.X, anEntity.EntityPosition.Y), new Rectangle((int)anEntity.EntityPosition.X, (int)anEntity.EntityPosition.Y, 100, 1), Color.Red, anEntity.TargetRotation, yetAnotherOrigin, anEntity.Scale, SpriteEffects.None, 0f);

                spriteBatch.Draw(obstacle, new Vector2(500, 500), Color.Black);

            spriteBatch.End();

            SpriteBatch fontBatch = new SpriteBatch(GraphicsDevice);
            debugText.DrawFont(fontBatch);
            base.Draw(gameTime);
        }

    }
}
