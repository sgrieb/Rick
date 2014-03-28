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
        DickThrower dickthrower;
        DickCatcher dickcatcher;
        List<DickShot> dickshots = new List<DickShot>();
        double dickshotTimer = 0;
        int dickshotInterval = 2;
        static DickShot dickshotTemplate;
        int screenPadding = 0;
        private SpriteFont font;
        private int score = 0;

        public Texture2D koala;

        private KeyboardState oldState;

        public class Sprite
        {
            public Vector2 position;
            public Texture2D texture;
            public Vector2 speed;
            public int MaxX;
            public int MinX = 0;
            public int MaxY;
            public int MinY = 0;
            public Rectangle boundingBox;
            public Sprite()
            {
            }
            public Sprite (Texture2D texture, int MaxX, int MaxY)
            {
                this.texture = texture;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
                this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
            }
        }

        public class DickThrower:Sprite
        {
            public DickThrower(Texture2D texture, int MaxX, int MaxY)
            {
                speed = new Vector2(500, 0);
                position = new Vector2(100, 50);
                this.texture = texture;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
                this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
            }
        }

        public class DickCatcher : Sprite
        {
            public DickCatcher(Texture2D texture, int MaxX, int MaxY)
            {
                
                position = new Vector2(100, 425);
                this.texture = texture;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
                this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
            }
        }

        public class DickShot : Sprite
        {
            public DickShot(Texture2D texture, int MaxX, int MaxY)
            {

                position = new Vector2(200, 200);
                this.texture = texture;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
                this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
            }
            public DickShot(Texture2D texture, int MaxX, int MaxY, Vector2 position)
            {

                this.position = position;
                this.texture = texture;
                this.MaxX = MaxX;
                this.MaxY = MaxY;
                this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
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

            font = Content.Load<SpriteFont>("default"); // Use the name of your sprite font file here

            koala = Content.Load<Texture2D>("koala");

            Texture2D texture = Content.Load<Texture2D>("lilguy");
            dickthrower = new DickThrower(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width- screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);

            dickcatcher = new DickCatcher(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);


            texture = Content.Load<Texture2D>("dickshot");
            dickshotTemplate = new DickShot(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);
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
            //this is getting disorganized fast.  im going to seperate things soon

            // Move the sprite by speed, scaled by elapsed time.
            dickthrower.position.X += dickthrower.speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            dickshotTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //make dickshots move down
            foreach (var shot in dickshots)
            {
                shot.position.Y += 3;
                shot.boundingBox.Y += 3;
            }

            //if the timer is up, shoot a dickshot
            if (dickshotTimer >= dickshotInterval)
            {
                dickshots.Add(new DickShot(dickshotTemplate.texture, dickshotTemplate.MaxX, dickshotTemplate.MaxY,dickthrower.position));
                dickshotTimer = 0;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState newState = Keyboard.GetState();  // get the newest state

            if (newState.IsKeyDown(Keys.Left))
            {
                if (dickcatcher.position.X - 5 > dickcatcher.MinX)
                {
                    dickcatcher.position.X -= 5;
                    dickcatcher.boundingBox.X -= 5;
                }
            }
            else if (newState.IsKeyDown(Keys.Right))
            {
                if (dickcatcher.position.X + 5 < dickcatcher.MaxX)
                {
                    dickcatcher.position.X += 5;
                    dickcatcher.boundingBox.X += 5;
                }
            }

            // Check for bounce.
            if (dickthrower.position.X > dickthrower.MaxX)
            {
                dickthrower.speed.X *= -1;
                dickthrower.position.X = dickthrower.MaxX;
            }

            else if (dickthrower.position.X <dickthrower. MinX)
            {
                dickthrower.speed.X *= -1;
                dickthrower.position.X = dickthrower.MinX;
            }

            //check if dickshots hit dickcatcher
            for (int i = 0; i < dickshots.Count;i++)
            {
                if (dickshots[i].boundingBox.Intersects(dickcatcher.boundingBox))
                {
                    score++;
                    dickshots.Remove(dickshots[i]);
                    i--;
                }
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

            spriteBatch.Draw(dickthrower.texture, dickthrower.position, Color.White);

            spriteBatch.Draw(dickcatcher.texture, dickcatcher.position, Color.White);

            spriteBatch.Draw(koala, dickcatcher.boundingBox, Color.White);

            spriteBatch.DrawString(font, "Score: "+ score.ToString(), new Vector2(10, 10), Color.Black);

            foreach (var shot in dickshots)
            {
                spriteBatch.Draw(dickshotTemplate.texture, shot.position, Color.White);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
