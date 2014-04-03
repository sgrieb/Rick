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
        Ground ground;
        List<Shot> shots = new List<Shot>();
        double shotTimer = 0;
        int shotInterval = 2;
        static Texture2D shotTexture;
        int screenPadding = 0;
        private SpriteFont font;
        private int score = 0;
        private int groundY = 425;
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

            var groundTexture = Content.Load<Texture2D>("ground");

            ground = new Ground(groundTexture, graphics.GraphicsDevice.Viewport.Width - groundTexture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - groundTexture.Height - screenPadding, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - groundTexture.Height)); 

            Texture2D texture = Content.Load<Texture2D>("lilguy");
            thrower = new Thrower(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width- screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);


            groundY = graphics.GraphicsDevice.Viewport.Height - groundTexture.Height;
            Texture2D caveman = Content.Load<Texture2D>("caveman");
            catcher = new Catcher(caveman, 2, 8);
            catcher.position = new Vector2(100, graphics.GraphicsDevice.Viewport.Height - groundTexture.Height - caveman.Height);
            catcher.MaxX = graphics.GraphicsDevice.Viewport.Width - catcher.frameWidth - screenPadding ;
            catcher.MaxY = graphics.GraphicsDevice.Viewport.Height - catcher.frameHeight - screenPadding - groundTexture.Height - caveman.Height;
            catcher.boundingBox = new Rectangle((int)catcher.position.X, (int)catcher.position.Y, catcher.frameWidth, catcher.frameHeight);

            shotTexture = Content.Load<Texture2D>("dickshot");
            
            //shotTemplate = new Shot(texture, graphics.GraphicsDevice.Viewport.Width - texture.Width - screenPadding, graphics.GraphicsDevice.Viewport.Height - texture.Height - screenPadding);
        }

        private void Gravity(Sprite sprite)
        {
            //not sure why this works and dont feel like figuring it out right now
            if (sprite.position.Y + ground.texture.Height*2 < groundY)
            {
                sprite.position.Y += 5;
                sprite.boundingBox.Y += 5;
            }
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
            //i dont think this does anyhting right now because there is no speed
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

            //catcher actions
            //jumping :)
            if (!newState.IsKeyDown(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                if (catcher.position.X - 5 > catcher.MinX)
                {
                    catcher.position.Y -= 100;
                    catcher.boundingBox.Y -= 100;
                    
                }
            }
            if (newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A))
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
                        catcher.Update("LEFT");
                        catcher.frameStagger = 0;
                    }
                }
            }
            else if (newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D))
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
                        catcher.Update("RIGHT");
                        catcher.frameStagger = 0;
                    }
                }
            }
            Gravity(catcher);

            // Check for window boundaries
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

            

            oldState = newState;  // set the old state as the new state for next time

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(thrower.texture, thrower.position, Color.White);

            spriteBatch.Draw(ground.texture, ground.position, Color.White);

            catcher.Draw(spriteBatch, catcher.position);

            //you can use this line below to test bounding boxes and shite
            //spriteBatch.Draw(koala, catcher.boundingBox, Color.White);

            spriteBatch.DrawString(font, "Score: "+ score.ToString(), new Vector2(10, 10), Color.Black);

            foreach (var shot in shots)
            {
                spriteBatch.Draw(shotTexture, shot.position, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
