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

namespace Zouls
{
    class GameObject
    {
        // Properties
        public Texture2D texture;
        public Vector2 position;
        protected Color color;
        public int width, height;
        public Vector2 origin;
        public float rotation;

        // Constructor
        public GameObject(Texture2D texture, Vector2 position, Color color, float rotation)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            this.height = texture.Height;
            this.width = texture.Width;
            this.origin = new Vector2(width / 2, height / 2);
        }

        // Methods
        public virtual Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
