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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BouncingImage : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Texture2D texture;
        Vector2 position;
        Vector2 velocity;

        public BouncingImage(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("XnaLogo");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            velocity = new Vector2(50, 30);
            position = Vector2.Zero;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            position += (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (!GraphicsDevice.Viewport.Bounds.Contains(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height)))
            {
                bool negateX = false;
                bool negateY = false;

                if ((position.X < 0) || (position.X + texture.Width > GraphicsDevice.Viewport.Width))
                {
                    negateX = true;
                }
                if ((position.Y < 0) || (position.Y + texture.Height > GraphicsDevice.Viewport.Height))
                {
                    negateY = true;
                }
                position -= (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (negateX) velocity.X *= -1;
                if (negateY) velocity.Y *= -1;
                position += (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }
    }
}
