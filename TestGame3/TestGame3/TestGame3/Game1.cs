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


        protected VertexBuffer vertexBuffer;
        protected BasicEffect chunkEffect;

        bool updateChunk;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

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

            model = Content.Load<Model>("cube");

            (model.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            Components.Add(camera = new CameraComponent(this));//register camera as gamecomponent

            updateChunk = true;

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
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            int count = 0;

            /*Vector3 pos = new Vector3(0, 0, 0);
            if (updateChunk)
            {
                chunkEffect = new BasicEffect(GraphicsDevice);
                chunkEffect.EnableDefaultLighting();
                chunkEffect.World = Matrix.CreateTranslation(pos);
                chunkEffect.View = camera.View;
                chunkEffect.Projection = camera.Proj;
                chunkEffect.VertexColorEnabled = true;

                VertexPositionColor[] vertexData = new VertexPositionColor[8000000];

                int vertexOffset = 0;

                model.Meshes[0].MeshParts[0].VertexBuffer.;

                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), )
            }*/

            for (float x = -100; x <= 100; x += 2.1f)
            {
                for (float z = -100; z <= 100; z += 2.1f)
                {
                    Vector3 pos = new Vector3(x, 0, z);

                    BoundingFrustum boundingFrustum = new BoundingFrustum(camera.View * camera.Proj);

                    // Draw the model. A model can have multiple meshes, so loop.
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        if (boundingFrustum.Contains(mesh.BoundingSphere.Transform(Matrix.CreateTranslation(pos))) != ContainmentType.Disjoint)
                        {
                            // This is where the mesh orientation is set, as well as our camera and
                            // projection.
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.EnableDefaultLighting();
                                effect.World = Matrix.CreateTranslation(pos);
                                effect.View = camera.View;
                                effect.Projection = camera.Proj;
                            }

                            // Draw the mesh, using the effects set above.
                            mesh.Draw();

                            foreach(ModelMeshPart meshPart in mesh.MeshParts)
                            {
                                count += meshPart.PrimitiveCount;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(count);

            //display FPS
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsFont, "FPS:" + FPS, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
