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
using Rick.Model;

namespace Rick
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Thrower thrower;
        Catcher catcher;
        List<Shot> shots = new List<Shot>();
        double shotTimer = 0;
        int shotInterval = 2;
        static Texture2D shotTexture;
        int screenPadding = 0;
        private SpriteFont font;
        private int score = 0;

        public Texture2D koala;

        private KeyboardState oldState;

        

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

            font = Content.Load<SpriteFont>("default"); // Use the name of your sprite font file here

            koala = Content.Load<Texture2D>("koala");

            Texture2D texture = Content.Load<Texture2D>("lilguy");
            thrower = new Thrower(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width- screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);


            Texture2D caveman = Content.Load<Texture2D>("bin");
            catcher = new Catcher(caveman, 2, 4);
            catcher.MaxX = graphics.GraphicsDevice.Viewport.Width - catcher.frameWidth - screenPadding;
            catcher.MaxY = graphics.GraphicsDevice.Viewport.Height - catcher.frameHeight - screenPadding;

            shotTexture = Content.Load<Texture2D>("dickshot");
            //shotTemplate = new Shot(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);
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


            // Move the sprite by speed, scaled by elapsed time.
            thrower.position.X += thrower.speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            shotTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //make shots move down
            foreach (var shot in shots)
            {
                shot.position.Y += 3;
                shot.boundingBox.Y += 3;
            }

            //if the timer is up, shoot a shot
            if (shotTimer >= shotInterval)
            {
                shots.Add(new Shot(shotTexture, graphics.GraphicsDevice.Viewport.Width - shotTexture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - shotTexture.Height - screenPadding, thrower.position));
                shotTimer = 0;
            }

            KeyboardState newState = Keyboard.GetState();  // get the newest state

            if (newState.IsKeyDown(Keys.Left))
            {
                if (catcher.position.X - 5 > catcher.MinX)
                {
                    catcher.position.X -= 5;
                    catcher.boundingBox.X -= 5;
                    if (catcher.frameStagger < 3)
                    {
                        catcher.frameStagger++;
                    }
                    else
                    {
                        catcher.Update();
                        catcher.frameStagger = 0;
                    }
                }
            }
            else if (newState.IsKeyDown(Keys.Right))
            {
                if (catcher.position.X + 5 < catcher.MaxX)
                {
                    catcher.position.X += 5;
                    catcher.boundingBox.X += 5;
                    if (catcher.frameStagger < 3)
                    {
                        catcher.frameStagger++;
                    }
                    else
                    {
                        catcher.Update();
                        catcher.frameStagger = 0;
                    }
                }
            }

            // Check for bounce.
            if (thrower.position.X > thrower.MaxX)
            {
                thrower.speed.X *= -1;
                thrower.position.X = thrower.MaxX;
            }

            else if (thrower.position.X <thrower. MinX)
            {
                thrower.speed.X *= -1;
                thrower.position.X = thrower.MinX;
            }

            //check if shots hit catcher
            for (int i = 0; i < shots.Count;i++)
            {
                if (shots[i].boundingBox.Intersects(catcher.boundingBox))
                {
                    score++;
                    shots.Remove(shots[i]);
                    i--;
                }
            }

            

            oldState = newState;  // set the new state as the old state for next time

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(thrower.texture, thrower.position, Color.White);

            //spriteBatch.Draw(catcher.texture, catcher.position, Color.White);
            catcher.Draw(spriteBatch, catcher.position);
            //spriteBatch.Draw(koala, catcher.boundingBox, Color.White);

            spriteBatch.DrawString(font, "Score: "+ score.ToString(), new Vector2(10, 10), Color.Black);

            foreach (var shot in shots)
            {
                spriteBatch.Draw(shotTexture, shot.position, Color.White);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
