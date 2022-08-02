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
    class Character : MovingObject
    {
        // Properties


        // Constructor
        public Character(Texture2D texture, Vector2 position, Color color, float rotation, float speed):base(texture, position, color, rotation, speed)
        {

        }

        // Methods
        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X + width / 4, (int)position.Y + height / 4, width / 2, height / 4 * 3);
        }

        public bool Interact()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                return true;
            else
                return false;
        }
    }
}
