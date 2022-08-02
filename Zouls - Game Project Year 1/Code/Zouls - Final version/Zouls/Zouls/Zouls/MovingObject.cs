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
    class MovingObject : GameObject
    {
        // Properties
        public float speed;
        public Vector2 direction;

        // Constructor
        public MovingObject(Texture2D texture, Vector2 position, Color color, float rotation, float speed):base(texture, position, color, rotation)
        {
            this.speed = speed;
        }

        // Methods
        public void UpdatePosition()
        {
            position += this.direction * this.speed;
        }
    }
}
