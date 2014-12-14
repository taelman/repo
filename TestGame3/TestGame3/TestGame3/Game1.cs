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
        CameraComponent camera;


        protected VertexBuffer vertexBuffer;
        protected IndexBuffer indexBuffer;

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

            int primitiveCount = 0;

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
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            BasicEffect effect = new BasicEffect(GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.Proj;
            effect.VertexColorEnabled = true;

            if (vertexBuffer == null)
            {
                VertexPositionColor[] vertexData = new VertexPositionColor[1500000];
                int[] indexData = new int[1500000];
                int vertexCount = 0;
                for (float x = -250; x < 250; x += 1)
                {
                    for (float z = -250; z < 250; z += 1)
                    {
                        vertexData[vertexCount] = new VertexPositionColor();
                        vertexData[vertexCount].Position = new Vector3(x, getHeight(x, z), z);
                        vertexData[vertexCount].Color = Color.Azure;
                        vertexData[vertexCount + 1] = new VertexPositionColor();
                        vertexData[vertexCount + 1].Position = new Vector3(x + 1, getHeight(x + 1, z), z);
                        vertexData[vertexCount + 1].Color = Color.Aquamarine;
                        vertexData[vertexCount + 2] = new VertexPositionColor();
                        vertexData[vertexCount + 2].Position = new Vector3(x, getHeight(x, z + 1), z + 1);
                        vertexData[vertexCount + 2].Color = Color.Aqua;

                        vertexData[vertexCount + 3] = new VertexPositionColor();
                        vertexData[vertexCount + 3].Position = new Vector3(x + 1, getHeight(x + 1, z + 1), z + 1);
                        vertexData[vertexCount + 3].Color = Color.Azure;
                        vertexData[vertexCount + 4] = new VertexPositionColor();
                        vertexData[vertexCount + 4].Position = new Vector3(x, getHeight(x, z + 1), z + 1);
                        vertexData[vertexCount + 4].Color = Color.Aqua;
                        vertexData[vertexCount + 5] = new VertexPositionColor();
                        vertexData[vertexCount + 5].Position = new Vector3(x + 1, getHeight(x + 1, z), z);
                        vertexData[vertexCount + 5].Color = Color.Aquamarine;

                        indexData[vertexCount] = vertexCount;
                        indexData[vertexCount + 1] = vertexCount + 1;
                        indexData[vertexCount + 2] = vertexCount + 2;
                        indexData[vertexCount + 3] = vertexCount + 3;
                        indexData[vertexCount + 4] = vertexCount + 4;
                        indexData[vertexCount + 5] = vertexCount + 5;

                        vertexCount += 6;
                        primitiveCount += 2;
                    }
                }

                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertexCount, BufferUsage.None);
                vertexBuffer.SetData<VertexPositionColor>(vertexData);
                indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, vertexCount, BufferUsage.None);
                indexBuffer.SetData<int>(indexData);
            }

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;
            effect.CurrentTechnique.Passes[0].Apply();
            primitiveCount = vertexBuffer.VertexCount / 3;

            //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexData, 0, primitiveCount);
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, primitiveCount);

            //display primitives
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsFont, "Primitives:" + primitiveCount, new Vector2(0, 100), Color.Black);
            spriteBatch.End();

            //display FPS
            spriteBatch.Begin();
            spriteBatch.DrawString(fpsFont, "FPS:" + FPS, Vector2.Zero, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public float getHeight(float x, float z)
        {
            return -1.0f;
        }

    }
}
