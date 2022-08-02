/* AnimatedObject
 * Author: Thea Nilsson
 * v 0.1
 * Known bugs:
 * to-do:
 * Kunna sätta start och stoppunkter i spritesheetet
 */

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
    class AnimatedObject : GameObject
    {
        // Properties
        private Point frameSize;     // Hur stor är en frame i pixlar
        private Point gridSize;      // Hur många frames som finns i kartan
        private Point currentFrame;  // Nuvarande frame i kartan
        public int msPerFrame;             // Hur länge varje frame visas
        private int timeSinceLastFrame;      // Hur lång tid det har gått sen framen byttes
        public bool isRunning = true;
        public SpriteEffects spriteEffect;
        public Point endingFrame;
        public Point startingFrame;

        // Constructor
        public AnimatedObject(Texture2D texture, Vector2 position, Color color, float rotation, Point frameSize, Point gridSize, SpriteEffects spriteEffect, Point startingFrame, Point endingFrame)
            : base(texture, position, color, rotation)
        {
            this.frameSize = frameSize;
            this.gridSize = gridSize;
            this.currentFrame = startingFrame;
            this.msPerFrame = 60;
            base.origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            base.rotation = rotation;
            this.spriteEffect = spriteEffect;
            this.endingFrame = endingFrame;
            this.startingFrame = startingFrame;
        }

        // Methods
        public void PlayAnimation(GameTime gameTime, int msPerFrame, bool loop)
        {
            // Checks if it's time to change frame
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > msPerFrame)
            {
                // Changes frame
                currentFrame.X++;
                timeSinceLastFrame -= msPerFrame;
            }
            // Changes row if the end of x is reached
            if (currentFrame.X >= gridSize.X)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
                if (loop)
                    if (currentFrame.Y >= endingFrame.Y)
                        currentFrame.Y = startingFrame.Y;
            }
            this.msPerFrame = msPerFrame;
        }

        // Override bypasses superclass functions
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(base.texture, base.position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), base.color, base.rotation, base.origin, 1f, spriteEffect, 0f);
        }

        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X - 32, (int)position.Y - 32, frameSize.X, frameSize.Y);
        }
    }
}
