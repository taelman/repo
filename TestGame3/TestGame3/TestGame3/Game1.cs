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
            model = Content.Load<Model>("box");
            (model.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            Components.Add(camera = new CameraComponent(this));

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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            const int numberBoxes = 3;
            const int radiusMultiple = numberBoxes + 1;
            float radius = model.Meshes[0].BoundingSphere.Radius;
            Matrix view = Matrix.CreateLookAt(new Vector3(0, radius*radiusMultiple, radius*(radiusMultiple*radiusMultiple)),new Vector3((numberBoxes / 2) * (radius * radiusMultiple) - 1,(numberBoxes / 2) * (radius * radiusMultiple) - 1, 0),Vector3.Up);
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f);
            for(int x = 0; x < numberBoxes; x++){
                for (int y = 0; y < numberBoxes; y++)
                {
                    Vector3 pos = new Vector3((y * (radius * radiusMultiple)) - 1, (x * (radius * radiusMultiple)) - 1, -(y + x));
                    //model.Draw(Matrix.Multiply(Matrix.CreateRotationZ(MathHelper.ToRadians(45)),Matrix.CreateTranslation(pos)), view, proj);
                    model.Draw(Matrix.CreateTranslation(pos), camera.View, camera.Proj);
                }
            }

            base.Draw(gameTime);
        }
    }
}
