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

namespace TestGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        //Texture2D texture2;
        //Texture2D texture3;
        Texture2D alphaTexture;
        SpriteFont font;

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
            // TODO: Add your initialization logic here

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

            //texture = Content.Load<Texture2D>("glacier");
            //texture2 = Content.Load<Texture2D>("cat");
            //texture3 = Content.Load<Texture2D>("spriteanimation");
            texture = Content.Load<Texture2D>("layers");
            alphaTexture = Content.Load<Texture2D>("AlphaSprite");
            font = Content.Load<SpriteFont>("SpriteFont1");

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            int frame = (int)(gameTime.TotalGameTime.TotalSeconds * 20) % 10;
            int frame2 = (int)(gameTime.TotalGameTime.TotalSeconds * 100);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(alphaTexture, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Hello World", new Vector2(300, 100), Color.Red);
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            //spriteBatch.Draw(texture, GraphicsDevice.Viewport.Bounds, Color.White);
            //spriteBatch.Draw(texture2, new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
            //spriteBatch.Draw(texture3, new Vector2(100 + frame2, 200), new Rectangle(frame*96, 0, 96, 96), Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 1.0f);
            //for (int i = 0; i < 4; i++)
            //{
            //    Rectangle src = new Rectangle((i % 2) * (texture.Width/2), (i<2) ? 0 : (texture.Height / 2), texture.Width/2, texture.Height/2);
            //    spriteBatch.Draw(texture, new Vector2(50 + (50 * i), 50 + (50 * i)), src, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, i * 0.1f);
            //}
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
