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

namespace TestGame3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        float elapseTime;
        int frameCounter;
        float FPS;
        SpriteFont fpsFont;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        CameraComponent camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            fpsFont = Content.Load<SpriteFont>("font1");

            model = Content.Load<Model>("box");
            (model.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            Components.Add(camera = new CameraComponent(this));//register camera as gamecomponent

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //calculate framerate
            elapseTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter++;
            if (elapseTime > 1)
            {
                FPS = frameCounter;
                frameCounter = 0;
                elapseTime = 0;
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);

            for (float x = -10; x <= 10; x += 1.5f)
            {
                for (float z = -10; z <= 10; z += 1.5f)
                {
                    Vector3 pos = new Vector3(x, 0, z);
                    //model.Draw(Matrix.Multiply(Matrix.CreateRotationZ(MathHelper.ToRadians(45)),Matrix.CreateTranslation(pos)), view, proj);
                    model.Draw(Matrix.CreateTranslation(pos), camera.View, camera.Proj);
                }
            }


            //display FPS
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsFont, "FPS:" + FPS, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
