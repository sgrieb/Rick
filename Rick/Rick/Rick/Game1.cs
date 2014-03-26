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

namespace Rick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite lilguy;

        private KeyboardState oldState;

        public class Sprite
        {
            public Vector2 position;
            public Texture2D texture;
            public Sprite(Vector2 position, Texture2D texture)
            {
                this.position = position;
                this.texture = texture;
            }

        }

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
            lilguy = new Sprite(new Vector2(0, 0),  Content.Load<Texture2D>("lilguy"));
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
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
            KeyboardState newState = Keyboard.GetState();  // get the newest state

            // handle the input
            if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left))
            {
                lilguy.position.X -= 5;
            }
            if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right))
            {
                lilguy.position.X += 5;
            }
            if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
            {
                lilguy.position.Y -= 5;
            }
            if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))
            {
                lilguy.position.Y += 5;
            }

            oldState = newState;  // set the new state as the old state for next time

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(lilguy.texture, lilguy.position, Color.White);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
