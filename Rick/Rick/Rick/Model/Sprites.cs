using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rick.Model
{
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
        public Sprite(Texture2D texture, int MaxX, int MaxY)
        {
            this.texture = texture;
            this.MaxX = MaxX;
            this.MaxY = MaxY;
            this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
        }
    }

    public class AnimatedSprite:Sprite
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int frameHeight { get; set; }
        public int frameWidth { get; set; }
        public int frameStagger = 3;
        public string oldDirection { get; set; }
        private int currentFrame;
        private int totalFrames;
        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            this.texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            frameWidth = texture.Width / Columns;
            frameHeight = texture.Height / Rows;
        }
        public void Update(string direction)
        {

            if (direction == oldDirection)
            {
                currentFrame++;
                if (direction == "RIGHT" && currentFrame == Columns - 1)
                {
                    currentFrame = 0;
                }
                else if (direction == "LEFT" && currentFrame == Columns * 2 - 1)
                {
                    currentFrame = Columns + 1;
                }

            }
            else
            {
                if (direction == "LEFT")
                {
                    currentFrame = Columns + 1;
                }
                else if (direction == "RIGHT")
                {
                    currentFrame = 0;
                }
            }
            oldDirection = direction;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(frameWidth * column, frameHeight * row, frameWidth, frameHeight);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, frameWidth, frameHeight);



            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);

        }
    }

    public class Thrower : Sprite
    {
        public Thrower(Texture2D texture, int MaxX, int MaxY) : base(texture, MaxX, MaxY) { }
        public new Vector2 speed = new Vector2(500, 0);
        public new Vector2 position = new Vector2(100, 50);
    }

    public class Catcher : AnimatedSprite
    {
        public Catcher(Texture2D texture, int rows, int columns):base(texture, rows, columns)
        {
            position = new Vector2(100, 425);
        }
        
    }

    public class Shot : Sprite
    {
        public Shot(Texture2D texture, int MaxX, int MaxY, Vector2 position)
            : base(texture, MaxX, MaxY)
        {
            this.MaxX = MaxX;
            this.MaxY = MaxY;
            this.position = position;
            this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
        }
    }
}
