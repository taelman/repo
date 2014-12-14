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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CameraComponent : Microsoft.Xna.Framework.GameComponent
    {
        private MouseState previousMouseState;

        //manipulating these fields will alter the camera properties
        public Vector3 cameraPosition;//position of camera
        public Vector3 cameraDirection;//direction of camera
        public Vector3 up;//this side up
        public float rotX;//degrees of rotation camera
        public float rotY;//degrees of rotation camera
        public float zoom;//times zoom (1 is no zoom)

        //keyboard properties for this camera
        public float keyboardTranslationSensitivity;
        public float keyboardRotationSensitivity;

        //mouse properties for this camera
        public float mouseHorizontalSensitivity;
        public float mouseVerticalSensitivity;
        public Boolean mouseVerticalReversed;
        public float mouseMinZoom;
        public float mouseMaxZoom;
        public float mouseScrollSensitivity;

        //other properties
        public float maxVerticalRotation;//in degrees, same for up as for down

        protected Matrix view;
        public Matrix View
        {
            get { return view; }
        }
        protected Matrix proj;
        public Matrix Proj
        {
            get { return proj; }
        }

        public CameraComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //startingconditions camera
            cameraPosition = new Vector3(0, 5, 0);
            cameraDirection = new Vector3(0, 0, 1);//start staring direction
            up = Vector3.Up;//up is up
            rotX = 0;//zero horizontal startrotation
            rotY = 0;//zero vertical startrotation
            //sensitivity settings for mouse
            mouseHorizontalSensitivity = 0.5f;
            mouseVerticalSensitivity = 0.5f;
            mouseScrollSensitivity = 0.5f;
            mouseMinZoom = 1.0f;
            mouseMaxZoom = 10.0f;
            zoom = 1.0f;
            //sensitivity settings for keyboard
            keyboardTranslationSensitivity = 0.5f;
            keyboardRotationSensitivity = 0.5f;
            previousMouseState = Mouse.GetState();
            maxVerticalRotation = 85.0f;

            calcViewProj();

            base.Initialize();
        }

        private void calcViewProj()
        {
            //calculate rotation of camera using the rotation angles
            Matrix rotationMatrix = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(rotX)), Matrix.CreateRotationY(MathHelper.ToRadians(rotY)));

            //calculate a point to look at with camera
            Vector3 transformedReference = Vector3.Transform(cameraDirection, rotationMatrix);
            Vector3 cameraLookAt = cameraPosition + transformedReference;

            //create view using camera position and point to look at with camera, with up direction
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, up);

            //create projection using viewing angle, aspect ratio, and closest and farthest view point
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / zoom, Game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000.0f * zoom);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            List<Keys> pressedKeys = Keyboard.GetState(PlayerIndex.One).GetPressedKeys().ToList<Keys>();
            MouseState mouseState = Mouse.GetState();

            //camera rotation using mouse
            if (mouseVerticalReversed == true) {
                rotX -= ((float)(mouseState.Y - previousMouseState.Y)) * mouseVerticalSensitivity;
            }
            else
            {
                rotX += ((float)(mouseState.Y - previousMouseState.Y)) * mouseVerticalSensitivity;
            }
            rotY -= ((float)(mouseState.X - previousMouseState.X)) * mouseHorizontalSensitivity;

            foreach(Keys k in pressedKeys){
                if (k == Keys.Escape) Game.Exit();

                //camera rotation using arrow keys
                if (k == Keys.Up)
                {
                    //move in direction of viewing direction
                    rotX -= 1.0f * keyboardRotationSensitivity;
                }else
                if (k == Keys.Down)
                {
                    //move in direction of viewing direction
                    rotX += 1.0f * keyboardRotationSensitivity;
                }else
                if (k == Keys.Left)
                {
                    //move in direction of viewing direction
                    rotY += 1.0f * keyboardRotationSensitivity;
                }else
                if (k == Keys.Right)
                {
                    //move in direction of viewing direction
                    rotY -= 1.0f * keyboardRotationSensitivity;
                }

                Vector3 translation = Vector3.Zero;
                //camera translation
                if (k == Keys.W)
                {
                    //move in direction of viewing direction
                    translation = new Vector3(0, 0, 1.0f * keyboardTranslationSensitivity);
                } else if (k == Keys.S)
                {
                    //move away from viewing direction
                    translation = new Vector3(0, 0, -1.0f * keyboardTranslationSensitivity);
                }
                else if (k == Keys.A)
                {
                    //move left from viewing direction
                    translation = new Vector3(1.0f * keyboardTranslationSensitivity, 0, 0);
                } else
                if (k == Keys.D)
                {
                    //move right from viewing direction
                    translation = new Vector3(-1.0f * keyboardTranslationSensitivity, 0, 0);
                }

                //perform translation

                //calculate rotation of camera using the rotation angles
                Matrix rotationMatrix = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(rotX)), Matrix.CreateRotationY(MathHelper.ToRadians(rotY)));
                //calculate a point to look at with camera
                Vector3 transformedReference = Vector3.Transform(translation, rotationMatrix);
                cameraPosition += transformedReference;


                if (k == Keys.LeftShift)
                {
                    //move vertically up
                    cameraPosition = cameraPosition + new Vector3(0.0f, -1.0f * keyboardTranslationSensitivity, 0);
                }
                if (k == Keys.Space)
                {
                    //move vertically down
                    cameraPosition = cameraPosition + new Vector3(0, 1.0f * keyboardTranslationSensitivity, 0);
                }
            }

            //zoom using mouse
            zoom = MathHelper.Max(mouseMinZoom, MathHelper.Min(mouseMaxZoom, zoom + (((float)mouseState.ScrollWheelValue - (float)previousMouseState.ScrollWheelValue) / 120.0f) * mouseScrollSensitivity));

            applyMaxVerticalRotation();
            //update camera
            calcViewProj();

            //reset mouse if game has focus
            if (Game.IsActive) Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        private void applyMaxVerticalRotation()
        {
            //set max vertical rotation
            if (rotX > maxVerticalRotation) rotX = maxVerticalRotation;
            else if (rotX < -1.0f * maxVerticalRotation) rotX = -1.0f * maxVerticalRotation;
        }

    }
}
