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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum Gamestate
        {
            MainMenu,
            Options,
            Playing,
            EndScreen,
            Exit
        }
        Gamestate CurrentGameState = Gamestate.MainMenu;

        //screen adjustments
        int screenWidth = 960, screenHeight = 640;

        Cbutton btnPlay;
        Cbutton btnExit;

        // Variables
        public bool[] leverIsActivated = new bool[3];
        public bool[] hasKey = new bool[3];
        public bool drawCoin;
        public bool[] coinPickedUp = new bool[6];
        public int currentLevel = 1;
        public int coinCount;
        Vector2 startingPosOne = new Vector2(0f, 64f);
        Vector2 startingPosTwo = new Vector2(448f, 576f);
        Vector2 startingPosThree = new Vector2(128, 128);
        public bool drawCharFront = true;
        public bool drawCharBack = false;
        public bool drawCharLeft = false;
        public bool drawCharRight = false;

        // Objects
        Character player;
        AnimatedObject playerAnimationFront;
        AnimatedObject playerAnimationBack;
        AnimatedObject playerAnimationLeft;
        AnimatedObject playerAnimationRight;
        GameObject lvlOneBackground;
        GameObject lvlTwoBackground;
        GameObject lvlThreeBackground;
        AnimatedObject backgroundFog;
        MovingObject[] rocks = new MovingObject[18];
        AnimatedObject[] levers = new AnimatedObject[3];
        AnimatedObject[] coins = new AnimatedObject[6];
        AnimatedObject[] keys = new AnimatedObject[3];
        AnimatedObject[] doors = new AnimatedObject[3];
        AnimatedObject[] bars = new AnimatedObject[4];
        Rectangle[] walls = new Rectangle[42];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set resolution
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.ApplyChanges();

            IsMouseVisible = true;

            btnPlay = new Cbutton(Content.Load<Texture2D>("Sprites/button"), GraphicsDevice);
            btnPlay.setPosition(new Vector2(341, 160));
            btnExit = new Cbutton(Content.Load<Texture2D>("Sprites/button2"), GraphicsDevice);
            btnExit.setPosition(new Vector2(340, 270));


            lvlOneBackground = new GameObject(Content.Load<Texture2D>("Sprites/backgroundOne"), Vector2.Zero, Color.White, 0f);
            lvlTwoBackground = new GameObject(Content.Load<Texture2D>("Sprites/backgroundTwo"), Vector2.Zero, Color.White, 0f);
            lvlThreeBackground = new GameObject(Content.Load<Texture2D>("Sprites/backgroundThree"), Vector2.Zero, Color.White, 0f);
            backgroundFog = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Fog_Animation"), new Vector2(480, 320), Color.White, 0f, new Point(960, 640), new Point(4, 2), SpriteEffects.None, new Point(0, 0), new Point(2, 1));
            player = new Character(Content.Load<Texture2D>("Sprites/blankPlayerBackground"), startingPosOne, Color.White, 0f, 2.5f);
            playerAnimationFront = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Ghost_Front_Animation_Strip"), new Vector2(player.position.X + 32, player.position.Y + 32)
                , Color.White, 0f, new Point(64, 64), new Point(8, 1), SpriteEffects.None, new Point(0, 0), new Point(8, 0));
            playerAnimationBack = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Ghost_Back_Animation_Strip"), new Vector2(player.position.X + 32, player.position.Y + 32)
                , Color.White, 0f, new Point(64, 64), new Point(8, 1), SpriteEffects.None, new Point(0, 0), new Point(8, 0));
            playerAnimationLeft = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Ghost_Left_Animation_Strip"), new Vector2(player.position.X + 32, player.position.Y + 32)
                , Color.White, 0f, new Point(64, 64), new Point(8, 1), SpriteEffects.None, new Point(0, 0), new Point(8, 0));
            playerAnimationRight = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Ghost_Right_Animation_Strip"), new Vector2(player.position.X + 32, player.position.Y + 32)
                , Color.White, 0f, new Point(64, 64), new Point(8, 1), SpriteEffects.None, new Point(0, 0), new Point(8, 0));
            levers[0] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Lever_Animation_Spritesheet"), new Vector2(320 + 32, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(5, 1), SpriteEffects.None, new Point(0, 0), new Point(5, 0));
            levers[1] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Lever_Animation_Spritesheet"), new Vector2(64 + 32, 320 + 32), Color.White, 0f, new Point(64, 64), new Point(5, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            levers[2] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Lever_Animation_Spritesheet"), new Vector2(320 + 32, 384 + 32), Color.White, 0f, new Point(64, 64), new Point(5, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[0] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(640 + 32, 64 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[1] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(64 + 32, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[2] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(832 + 32, 320 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[3] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(704 + 32, 512 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[4] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(32 + 320, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            coins[5] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Coin_Animation_Spritesheet"), new Vector2(576 + 32, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(4, 1), SpriteEffects.None, new Point(0, 0), new Point(4, 0));
            keys[0] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Key_Animation_Spritesheet"), new Vector2(382 + 32, 576 + 32), Color.White, 0f, new Point(64, 64), new Point(6, 1), SpriteEffects.None, new Point(0, 0), new Point(6, 0));
            keys[1] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Key_Animation_Spritesheet"), new Vector2(832 + 32, 192 + 32), Color.White, 0f, new Point(64, 64), new Point(6, 1), SpriteEffects.None, new Point(0, 0), new Point(6, 0));
            keys[2] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Key_Animation_Spritesheet"), new Vector2(576 + 32, 384 + 32), Color.White, 0f, new Point(64, 64), new Point(6, 1), SpriteEffects.None, new Point(0, 0), new Point(6, 0));
            doors[0] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Door_animation_side"), new Vector2(896 + 32, 512 + 32), Color.White, 0f, new Point(64, 64), new Point(15, 1), SpriteEffects.None, new Point(0, 0), new Point(15, 0));
            doors[1] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/door_animation_front"), new Vector2(448 + 32, 0 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            doors[2] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/door_animation_front"), new Vector2(768 + 32, 0 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            bars[0] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Gate_Front_Animation_Spritesheet"), new Vector2(640 + 32, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            bars[1] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Gate_Front_Animation_Spritesheet"), new Vector2(128 + 32, 128 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            bars[2] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Gate_Front_Animation_Spritesheet"), new Vector2(640 + 32, 64 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            bars[3] = new AnimatedObject(Content.Load<Texture2D>("Sprites/Animationstrips/Gate_Front_Animation_Spritesheet"), new Vector2(576 + 32, 320 + 32), Color.White, 0f, new Point(64, 64), new Point(7, 1), SpriteEffects.None, new Point(0, 0), new Point(7, 0));
            
            // Level one rocks
            rocks[0] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(384, 192), Color.White, 0f, 0.5f);
            rocks[1] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(384, 448), Color.White, 0f, 0.5f);

            // Level two rocks
            rocks[2] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(512, 64), Color.White, 0f, 0.5f);
            rocks[3] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(704, 64), Color.White, 0f, 0.5f);
            rocks[4] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(128, 384), Color.White, 0f, 0.5f);
            rocks[5] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(256, 384), Color.White, 0f, 0.5f);
            rocks[6] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(384, 384), Color.White, 0f, 0.5f);
            rocks[7] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(64, 448), Color.White, 0f, 0.5f);
            rocks[8] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(320, 448), Color.White, 0f, 0.5f);

            // Level three rocks
            rocks[9] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(320, 64), Color.White, 0f, 0.5f);
            rocks[10] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(576, 64), Color.White, 0f, 0.5f);
            rocks[11] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(256, 128), Color.White, 0f, 0.5f);
            rocks[12] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(384, 128), Color.White, 0f, 0.5f);
            rocks[13] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(512, 128), Color.White, 0f, 0.5f);
            rocks[14] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(448, 192), Color.White, 0f, 0.5f);
            rocks[15] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(640, 192), Color.White, 0f, 0.5f);
            rocks[16] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(320, 256), Color.White, 0f, 0.5f);
            rocks[17] = new MovingObject(Content.Load<Texture2D>("Sprites/rock"), new Vector2(384, 320), Color.White, 0f, 0.5f);

            // Level one walls
            walls[0] = new Rectangle(0, 0, 960, 64);
            walls[1] = new Rectangle(128, 64, 64, 512);
            walls[2] = new Rectangle(192, 64, 128, 128);
            walls[3] = new Rectangle(320, 64, 128, 64);
            walls[4] = new Rectangle(448, 64, 192, 128);
            walls[5] = new Rectangle(704, 64, 64, 384);
            walls[6] = new Rectangle(768, 64, 192, 448);
            walls[7] = new Rectangle(640, 320, 64, 128);
            walls[8] = new Rectangle(256, 256, 128, 192);
            walls[9] = new Rectangle(256, 448, 64, 64);
            walls[10] = new Rectangle(256, 512, 128, 128);
            walls[11] = new Rectangle(448, 256, 128, 192);
            walls[12] = new Rectangle(448, 512, 256, 128);

            // Level two walls
            walls[13] = new Rectangle(0, 0, 448, 64);
            walls[14] = new Rectangle(0, 64, 64, 448);
            walls[15] = new Rectangle(64, 192, 128, 128);
            walls[16] = new Rectangle(192, 128, 384, 256);
            walls[17] = new Rectangle(0, 512, 448, 128);
            walls[18] = new Rectangle(512, 0, 192, 64);
            walls[19] = new Rectangle(512, 512, 128, 128);
            walls[20] = new Rectangle(640, 384, 64, 256);
            walls[21] = new Rectangle(704, 128, 64, 320);
            walls[22] = new Rectangle(704, 576, 128, 64);
            walls[23] = new Rectangle(832, 384, 64, 256);
            walls[24] = new Rectangle(896, 0, 64, 640);
            walls[25] = new Rectangle(832, 256, 64, 64);
            walls[26] = new Rectangle(832, 0, 64, 192);

            // Level three walls
            walls[27] = new Rectangle(0, 0, 128, 640);
            walls[28] = new Rectangle(128, 576, 704, 64);
            walls[29] = new Rectangle(832, 0, 128, 640);
            walls[30] = new Rectangle(192, 0, 576, 64);
            walls[31] = new Rectangle(192, 64, 64, 320);
            walls[32] = new Rectangle(704, 64, 64, 320);
            walls[33] = new Rectangle(256, 320, 64, 192);
            walls[34] = new Rectangle(640, 320, 64, 192);
            walls[35] = new Rectangle(320, 448, 64, 64);
            walls[36] = new Rectangle(576, 448, 64, 64);
            walls[37] = new Rectangle(384, 384, 64, 128);
            walls[38] = new Rectangle(512, 384, 64, 128);
            walls[39] = new Rectangle(320, 192, 64, 64);
            walls[40] = new Rectangle(576, 192, 64, 64);
            walls[41] = new Rectangle(448, 256, 64, 64);
        }

        protected override void UnloadContent()
        {
                   
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (btnExit.isClicked)
                this.Exit();

            MovePlayer();

            AnimateCharacter(gameTime);

            // Checks if its level one and runs appropriate functions if so
            if (currentLevel == 1)
            {
                CheckLvlOneCollisions();
                AnimateLvlOne(gameTime);
                CheckLvlOneActivations(gameTime);
            }
            // Checks if its level two and runs appropriate functions if so
            else if (currentLevel == 2)
            {
                CheckLvlTwoCollisions();
                AnimateLvlTwo(gameTime);
                CheckLvlTwoActivations(gameTime);
            }
            // Checks if its level three and runs appropriate functions if so
            //else if (currentLevel == 3)
            //{
            //    CheckLvlThreeCollisions();
            //    AnimateLvlThree(gameTime);
            //    CheckLvlThreeActivations(gameTime);
            //}

            // Checks collsisions that always need to be checked
            CheckGeneralCollisions();

            backgroundFog.PlayAnimation(gameTime, 640, true);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            MouseState mouse = Mouse.GetState();

            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = Gamestate.Playing;
                    btnPlay.Update(mouse);
                    break;

                case Gamestate.Playing:

                    break;


            }

            MouseState mouse2 = Mouse.GetState();

            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    if (btnExit.isClicked == true) CurrentGameState = Gamestate.Exit;
                    btnExit.Update(mouse);
                    break;

                case Gamestate.Exit:

                    break;

            }

            spriteBatch.Begin();

            // Draws level one
            if (currentLevel == 1)
            {
                lvlOneBackground.Draw(spriteBatch);
                // Draws the coin on the screen if the player hasn't picked it up
                if (!coinPickedUp[0])
                    coins[0].Draw(spriteBatch);
                // Draws the key on the screen if the player hasn't picked it up
                if (!hasKey[0])
                    keys[0].Draw(spriteBatch);
                bars[0].Draw(spriteBatch);
                doors[0].Draw(spriteBatch);
                levers[0].Draw(spriteBatch);
                rocks[0].Draw(spriteBatch);
                rocks[1].Draw(spriteBatch);
            }

            // Draws level two
            else if (currentLevel == 2)
            {
                lvlTwoBackground.Draw(spriteBatch);
                levers[1].Draw(spriteBatch);
                for (int i = 1; i < 4; i++)
                {
                    if (!coinPickedUp[i])
                        coins[i].Draw(spriteBatch);
                }
                for (int i = 2; i < 9; i++)
                    rocks[i].Draw(spriteBatch);
                bars[1].Draw(spriteBatch);
                bars[2].Draw(spriteBatch);
                if (!hasKey[1])
                    keys[1].Draw(spriteBatch);
                doors[1].Draw(spriteBatch);
            }

            // Draws level three
            //else if (currentLevel == 3)
            //{
            //    lvlThreeBackground.Draw(spriteBatch);
            //    levers[2].Draw(spriteBatch);
            //    for (int i = 4; i < coins.Length; i++)
            //    {
            //        if (!coinPickedUp[i])
            //            coins[i].Draw(spriteBatch);
            //    }
            //    for (int i = 9; i < rocks.Length; i++)
            //        rocks[i].Draw(spriteBatch);
            //    bars[3].Draw(spriteBatch);
            //    if (!hasKey[2])
            //        keys[2].Draw(spriteBatch);
            //    doors[2].Draw(spriteBatch);
            //}

            // Draws the player
            player.Draw(spriteBatch);
            if (drawCharFront)
            {
                playerAnimationFront.Draw(spriteBatch);
            }
            else if (drawCharBack)
            {
                playerAnimationBack.Draw(spriteBatch);
            }
            else if (drawCharLeft)
            {
                playerAnimationLeft.Draw(spriteBatch);
            }
            else if (drawCharRight)
            {
                playerAnimationRight.Draw(spriteBatch);
            }

            backgroundFog.Draw(spriteBatch);

            // Draws the main menu
            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Sprites/Starting_menu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch); btnExit.Draw(spriteBatch);

                    break;

                case Gamestate.Playing:

                    break;

                // Draws the end screen
                case Gamestate.EndScreen:

                    spriteBatch.Draw(Content.Load<Texture2D>("Sprites/WinScreen"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
            
    
        }

        
        // LEVEL ONE COLLISIONS
        public void CheckLvlOneCollisions()
        {
            // Makes the player collide with rocks on level one
            for (int i = 0; i < 2; i++)
            {
                // Left
                if (player.position.X >= rocks[i].position.X - 64 && player.position.X < rocks[i].position.X + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == 1)
                {
                    player.position.X = rocks[i].position.X - 48;
                    // Lets the player push the rock to the right
                    if (player.Interact())
                    {
                        rocks[i].position.X += rocks[i].speed;
                        rocks[i].direction.X = 1;
                        rocks[i].direction.Y = 0;
                    }
                }
                // Right
                else if (player.position.X <= rocks[i].position.X + 64 && player.position.X > rocks[i].position.X + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == -1)
                {
                    player.position.X = rocks[i].position.X + 48;
                    // Lets the player push the rock to the left
                    if (player.Interact())
                    {
                        rocks[i].position.X -= rocks[i].speed;
                        rocks[i].direction.X = -1;
                        rocks[i].direction.Y = 0;
                    }
                }
                // Top
                if (player.position.Y >= rocks[i].position.Y - 64 && player.position.Y < rocks[i].position.Y + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.Y == 1)
                {
                    player.position.Y = rocks[i].position.Y - 64;
                    // Lets the player push the rock down
                    if (player.Interact())
                    {
                        rocks[i].position.Y += rocks[i].speed;
                        rocks[i].direction.X = 0;
                        rocks[i].direction.Y = 1;
                    }
                }
                // Bottom
                else if (player.position.Y <= rocks[i].position.Y + 48 && player.position.Y > rocks[i].position.Y + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.Y == -1)
                {
                    player.position.Y = rocks[i].position.Y + 48;
                    // Lets the player push the rock up
                    if (player.Interact())
                    {
                        rocks[i].position.Y -= rocks[i].speed;
                        rocks[i].direction.X = 0;
                        rocks[i].direction.Y = -1;
                    }
                }
            }

            // Makes the rocks collide with the walls on level one
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < 13; y++)
                {
                    // Moving down
                    if (rocks[i].direction.Y == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.Y = walls[y].Y - 64;
                    }
                    // Moving up
                    else if (rocks[i].direction.Y == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.Y = walls[y].Y + walls[y].Height;
                    }
                    // Moving left
                    else if (rocks[i].direction.X == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.X = walls[y].X + walls[y].Width;
                    }
                    // Moving right
                    else if (rocks[i].direction.X == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.X = walls[y].X - 64;
                    }
                }
            }

            // Makes the player collide with the walls on level one
            for (int i = 0; i < 13; i++)
            {
                // Moving down
                if (player.direction.Y == 1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.Y = walls[i].Y - 64;
                }
                // Moving up
                else if (player.direction.Y == -1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.Y = walls[i].Y + walls[i].Height - 16;
                }
                // Moving left
                else if (player.direction.X == -1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.X = walls[i].X + walls[i].Width - 16;
                }
                // Moving right
                else if (player.direction.X == 1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.X = walls[i].X - 48;
                }
            }

            // Makes the player collide with the bars on level one
            // Bottom
            if (!leverIsActivated[0] && player.position.Y <= bars[0].position.Y + 48 && player.GetRectangle().Intersects(bars[0].GetRectangle()) && player.direction.Y == -1)
            {
                player.position.Y = bars[0].GetRectangle().Bottom - 16;
            }
            // Top
            else if (!leverIsActivated[0] && player.position.Y >= bars[0].GetRectangle().Top - 64 && player.GetRectangle().Intersects(bars[0].GetRectangle()) && player.direction.Y == 1)
            {
                player.position.Y = bars[0].GetRectangle().Top - 64;
            }
            // Left
            else if (!leverIsActivated[0] && player.position.X >= bars[0].GetRectangle().Left - 48 && player.GetRectangle().Intersects(bars[0].GetRectangle()) && player.direction.X == 1)
            {
                player.position.X = bars[0].GetRectangle().Left - 48;
            }
            // Right
            else if (!leverIsActivated[0] && player.position.X >= bars[0].GetRectangle().Right + 48 && player.GetRectangle().Intersects(bars[0].GetRectangle()) && player.direction.X == 1)
            {
                player.position.X = bars[0].GetRectangle().Right + 48;
            }

            // Makes the rocks collide with eachother
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < 2; y++)
                {
                    if (rocks[i].position.Y > rocks[y].position.Y - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.Y != rocks[y].position.Y)
                    {
                        rocks[i].position.Y = rocks[y].position.Y - 64;
                    }

                    if (rocks[i].position.X > rocks[y].position.X - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.X != rocks[y].position.X)
                    {
                        rocks[i].position.X = rocks[y].position.X - 64;
                    }
                }
            }
        }


        // LEVEL TWO COLLISIONS
        public void CheckLvlTwoCollisions()
        {
            // Makes the player collide with rocks on level two
            for (int i = 2; i < 9; i++)
            {
                // Left
                if (player.position.X >= rocks[i].position.X - 64 && player.position.X < rocks[i].position.X + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == 1)
                {
                    player.position.X = rocks[i].position.X - 48;
                    // Lets the player push the rock to the right
                    if (player.Interact())
                    {
                        rocks[i].position.X += rocks[i].speed;
                        rocks[i].direction.X = 1;
                        rocks[i].direction.Y = 0;
                    }
                }
                // Right
                else if (player.position.X <= rocks[i].position.X + 64 && player.position.X > rocks[i].position.X + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == -1)
                {
                    player.position.X = rocks[i].position.X + 48;
                    // Lets the player push the rock to the left
                    if (player.Interact())
                    {
                        rocks[i].position.X -= rocks[i].speed;
                        rocks[i].direction.X = -1;
                        rocks[i].direction.Y = 0;
                    }
                }
                // Top
                if (player.position.Y >= rocks[i].position.Y - 64 && player.position.Y < rocks[i].position.Y + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.Y == 1)
                {
                    player.position.Y = rocks[i].position.Y - 64;
                    // Lets the player push the rock down
                    if (player.Interact())
                    {
                        rocks[i].position.Y += rocks[i].speed;
                        rocks[i].direction.X = 0;
                        rocks[i].direction.Y = 1;
                    }
                }
                // Bottom
                else if (player.position.Y <= rocks[i].position.Y + 48 && player.position.Y > rocks[i].position.Y + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.Y == -1)
                {
                    player.position.Y = rocks[i].position.Y + 48;
                    // Lets the player push the rock up
                    if (player.Interact())
                    {
                        rocks[i].position.Y -= rocks[i].speed;
                        rocks[i].direction.X = 0;
                        rocks[i].direction.Y = -1;
                    }
                }
            }

            // Makes the rocks collide with the walls on level two
            for (int i = 2; i < 9; i++)
            {
                for (int y = 13; y < 27; y++)
                {
                    // Moving down
                    if (rocks[i].direction.Y == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.Y = walls[y].Y - 64;
                    }
                    // Moving up
                    else if (rocks[i].direction.Y == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.Y = walls[y].Y + walls[y].Height;
                    }
                    // Moving left
                    else if (rocks[i].direction.X == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.X = walls[y].X + walls[y].Width;
                    }
                    // Moving right
                    else if (rocks[i].direction.X == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
                    {
                        rocks[i].position.X = walls[y].X - 64;
                    }
                }
            }

            // Makes the player collide with the walls on level two
            for (int i = 13; i < 27; i++)
            {
                // Moving down
                if (player.direction.Y == 1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.Y = walls[i].Y - 64;
                }
                // Moving up
                else if (player.direction.Y == -1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.Y = walls[i].Y + walls[i].Height - 16;
                }
                // Moving left
                else if (player.direction.X == -1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.X = walls[i].X + walls[i].Width - 16;
                }
                // Moving right
                else if (player.direction.X == 1 && player.GetRectangle().Intersects(walls[i]))
                {
                    player.position.X = walls[i].X - 48;
                }
            }

            // Makes the player collide with the bars on level two
            for (int i = 1; i < 3; i++)
            {
                // Bottom
                if (!leverIsActivated[1] && player.position.Y <= bars[i].position.Y + 48 && player.GetRectangle().Intersects(bars[i].GetRectangle()) && player.direction.Y == -1)
                {
                    player.position.Y = bars[i].GetRectangle().Bottom - 16;
                }
                // Top
                else if (!leverIsActivated[1] && player.position.Y >= bars[i].GetRectangle().Top - 64 && player.GetRectangle().Intersects(bars[i].GetRectangle()) && player.direction.Y == 1)
                {
                    player.position.Y = bars[i].GetRectangle().Top - 64;
                }
                // Left
                else if (!leverIsActivated[1] && player.position.X >= bars[i].GetRectangle().Left - 48 && player.GetRectangle().Intersects(bars[i].GetRectangle()) && player.direction.X == 1)
                {
                    player.position.X = bars[i].GetRectangle().Left - 48;
                }
                // Right
                else if (!leverIsActivated[1] && player.position.X >= bars[i].GetRectangle().Right + 48 && player.GetRectangle().Intersects(bars[i].GetRectangle()) && player.direction.X == 1)
                {
                    player.position.X = bars[i].GetRectangle().Right + 48;
                }
            }

            // Makes the rocks collide with eachother
            for (int i = 2; i < 9; i++)
            {
                for (int y = 2; y < 9; y++)
                {
                    if (rocks[i].position.Y > rocks[y].position.Y - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.Y != rocks[y].position.Y)
                    {
                        rocks[i].position.Y = rocks[y].position.Y - 64;
                    }

                    if (rocks[i].position.X > rocks[y].position.X - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.X != rocks[y].position.X)
                    {
                        rocks[i].position.X = rocks[y].position.X - 64;
                    }
                }
            }
        }


        //// LEVEL THREE COLLISIONS
        //public void CheckLvlThreeCollisions()
        //{
        //    // Makes the player collide with rocks on level three
        //    for (int i = 9; i < rocks.Length; i++)
        //    {
        //        // Left
        //        if (player.position.X >= rocks[i].position.X - 64 && player.position.X < rocks[i].position.X + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == 1)
        //        {
        //            player.position.X = rocks[i].position.X - 48;
        //            // Lets the player push the rock to the right
        //            if (player.Interact())
        //            {
        //                rocks[i].position.X += rocks[i].speed;
        //                rocks[i].direction.X = 1;
        //                rocks[i].direction.Y = 0;
        //            }
        //        }
        //        // Right
        //        else if (player.position.X <= rocks[i].position.X + 64 && player.position.X > rocks[i].position.X + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == -1)
        //        {
        //            player.position.X = rocks[i].position.X + 48;
        //            // Lets the player push the rock to the left
        //            if (player.Interact())
        //            {
        //                rocks[i].position.X -= rocks[i].speed;
        //                rocks[i].direction.X = -1;
        //                rocks[i].direction.Y = 0;
        //            }
        //        }
        //        // Top
        //        if (player.position.Y >= rocks[i].position.Y - 64 && player.position.Y < rocks[i].position.Y + 1 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == 1)
        //        {
        //            player.position.Y = rocks[i].position.Y - 64;
        //            // Lets the player push the rock down
        //            if (player.Interact())
        //            {
        //                rocks[i].position.Y += rocks[i].speed;
        //                rocks[i].direction.X = 0;
        //                rocks[i].direction.Y = 1;
        //            }
        //        }
        //        // Bottom
        //        else if (player.position.Y <= rocks[i].position.Y + 48 && player.position.Y > rocks[i].position.Y + 33 && player.GetRectangle().Intersects(rocks[i].GetRectangle()) && player.direction.X == -1)
        //        {
        //            player.position.Y = rocks[i].position.Y + 48;
        //            // Lets the player push the rock up
        //            if (player.Interact())
        //            {
        //                rocks[i].position.Y -= rocks[i].speed;
        //                rocks[i].direction.X = 0;
        //                rocks[i].direction.Y = -1;
        //            }
        //        }
        //    }

        //    // Makes the rocks collide with the walls on level three
        //    for (int i = 9; i < rocks.Length; i++)
        //    {
        //        for (int y = 27; y < walls.Length; y++)
        //        {
        //            // Moving down
        //            if (rocks[i].direction.Y == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
        //            {
        //                rocks[i].position.Y = walls[y].Y - 64;
        //            }
        //            // Moving up
        //            else if (rocks[i].direction.Y == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
        //            {
        //                rocks[i].position.Y = walls[y].Y + walls[y].Height;
        //            }
        //            // Moving left
        //            else if (rocks[i].direction.X == -1 && rocks[i].GetRectangle().Intersects(walls[y]))
        //            {
        //                rocks[i].position.X = walls[y].X + walls[y].Width;
        //            }
        //            // Moving right
        //            else if (rocks[i].direction.X == 1 && rocks[i].GetRectangle().Intersects(walls[y]))
        //            {
        //                rocks[i].position.X = walls[y].X - 64;
        //            }
        //        }
        //    }

        //    // Makes the player collide with the walls on level three
        //    for (int i = 27; i < walls.Length; i++)
        //    {
        //        // Moving down
        //        if (player.direction.Y == 1 && player.GetRectangle().Intersects(walls[i]))
        //        {
        //            player.position.Y = walls[i].Y - 64;
        //        }
        //        // Moving up
        //        else if (player.direction.Y == -1 && player.GetRectangle().Intersects(walls[i]))
        //        {
        //            player.position.Y = walls[i].Y + walls[i].Height - 16;
        //        }
        //        // Moving left
        //        else if (player.direction.X == -1 && player.GetRectangle().Intersects(walls[i]))
        //        {
        //            player.position.X = walls[i].X + walls[i].Width - 16;
        //        }
        //        // Moving right
        //        else if (player.direction.X == 1 && player.GetRectangle().Intersects(walls[i]))
        //        {
        //            player.position.X = walls[i].X - 48;
        //        }
        //    }

        //    // Makes the player collide with the bars on level three
        //    // Bottom
        //    if (!leverIsActivated[2] && player.position.Y <= bars[3].position.Y + 48 && player.GetRectangle().Intersects(bars[3].GetRectangle()) && player.direction.Y == -1)
        //    {
        //        player.position.Y = bars[3].GetRectangle().Bottom - 16;
        //    }
        //    // Top
        //    else if (!leverIsActivated[2] && player.position.Y >= bars[3].GetRectangle().Top - 64 && player.GetRectangle().Intersects(bars[3].GetRectangle()) && player.direction.Y == 1)
        //    {
        //        player.position.Y = bars[3].GetRectangle().Top - 64;
        //    }
        //    // Left
        //    else if (!leverIsActivated[2] && player.position.X >= bars[3].GetRectangle().Left - 48 && player.GetRectangle().Intersects(bars[3].GetRectangle()) && player.direction.X == 1)
        //    {
        //        player.position.X = bars[3].GetRectangle().Left - 48;
        //    }
        //    // Right
        //    else if (!leverIsActivated[2] && player.position.X >= bars[3].GetRectangle().Right + 48 && player.GetRectangle().Intersects(bars[3].GetRectangle()) && player.direction.X == 1)
        //    {
        //        player.position.X = bars[3].GetRectangle().Right + 48;
        //    }

        //    // Makes the rocks collide with eachother
        //    for (int i = 9; i < rocks.Length; i++)
        //    {
        //        for (int y = 9; y < rocks.Length; y++)
        //        {
        //            if (rocks[i].position.Y > rocks[y].position.Y - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.Y != rocks[y].position.Y)
        //            {
        //                rocks[i].position.Y = rocks[y].position.Y - 64;
        //            }

        //            if (rocks[i].position.X > rocks[y].position.X - 64 && rocks[i].GetRectangle().Intersects(rocks[y].GetRectangle()) && rocks[i].position.X != rocks[y].position.X)
        //            {
        //                rocks[i].position.X = rocks[y].position.X - 64;
        //            }
        //        }
        //    }
        //}

        public void CheckGeneralCollisions()
        {
            // Makes the player collide with the screen
            // Left/Right
            if (player.position.X <= -8)
                player.position.X = -8;
            else if (player.position.X >= screenWidth - 56)
                player.position.X = screenWidth - 56;

            // Up/Down
            if (player.position.Y <= 0)
                player.position.Y = 0;
            else if (player.position.Y >= screenHeight - 64)
                player.position.Y = screenHeight - 64;

            // Makes the rocks collide with the screen
            for (int i = 0; i < rocks.Length; i++)
            {
                // Left/Right
                if (rocks[i].position.X <= 0)
                    rocks[i].position.X = 0;
                else if (rocks[i].position.X >= screenWidth - 64)
                    rocks[i].position.X = screenWidth - 64;

                // Up/Down
                if (rocks[i].position.Y <= 0)
                    rocks[i].position.Y = 0;
                else if (rocks[i].position.Y >= screenHeight - 64)
                    rocks[i].position.Y = screenHeight - 64;
            }
        }

        public void AnimateCharacter(GameTime gameTime)
        {
            // Checks which part of the character to animate
            if (drawCharFront)
            {
                playerAnimationFront.position.X = player.position.X + 32;
                playerAnimationFront.position.Y = player.position.Y + 32;
                playerAnimationFront.PlayAnimation(gameTime, 128, true);
            }
            else if (drawCharBack)
            {
                playerAnimationBack.position.X = player.position.X + 32;
                playerAnimationBack.position.Y = player.position.Y + 32;
                playerAnimationBack.PlayAnimation(gameTime, 128, true);
            }
            else if (drawCharLeft)
            {
                playerAnimationLeft.position.X = player.position.X + 32;
                playerAnimationLeft.position.Y = player.position.Y + 32;
                playerAnimationLeft.PlayAnimation(gameTime, 128, true);
            }
            else if (drawCharRight)
            {
                playerAnimationRight.position.X = player.position.X + 32;
                playerAnimationRight.position.Y = player.position.Y + 32;
                playerAnimationRight.PlayAnimation(gameTime, 128, true);
            }
        }

        public void MovePlayer()
        {
            // Moves the player
            // Down
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                player.position.Y += player.speed;
                player.direction.X = 0;
                player.direction.Y = 1;
                drawCharFront = true;
                drawCharBack = false;
                drawCharLeft = false;
                drawCharRight = false;
            }
            // Up
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                player.position.Y -= player.speed;
                player.direction.X = 0;
                player.direction.Y = -1;
                drawCharBack = true;
                drawCharFront = false;
                drawCharLeft = false;
                drawCharRight = false;
            }
            // Right
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.position.X += player.speed;
                player.direction.X = 1;
                player.direction.Y = 0;
                drawCharRight = true;
                drawCharFront = false;
                drawCharBack = false;
                drawCharLeft = false;
            }
            // Left
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.position.X -= player.speed;
                player.direction.X = -1;
                player.direction.Y = 0;
                drawCharLeft = true;
                drawCharFront = false;
                drawCharBack = false;
                drawCharRight = false;
            }
        }

        public void CheckLvlOneActivations(GameTime gameTime)
        {
            // Checks if the player picks up a coin
            if (player.GetRectangle().Intersects(coins[0].GetRectangle()) && !coinPickedUp[0])
            {
                coinPickedUp[0] = true;
                coinCount++;
            }

            // Checks if the player picks up keys
            if (!hasKey[0] && player.GetRectangle().Intersects(keys[0].GetRectangle()))
            {
                hasKey[0] = true;
            }

            // Activates levers
            if (player.GetRectangle().Intersects(levers[0].GetRectangle()) && player.Interact())
            {
                leverIsActivated[0] = true;
            }

            if (leverIsActivated[0])
            {
                bars[0].PlayAnimation(gameTime, 128, false);
                levers[0].PlayAnimation(gameTime, 128, false);
            }

            // Checks if a door gets unlocked
            if (hasKey[0] && player.GetRectangle().Intersects(doors[0].GetRectangle()) && player.Interact())
            {
                currentLevel++;
                player.position = startingPosTwo;
            }
        }

        public void CheckLvlTwoActivations(GameTime gameTime)
        {
            // Checks if the player picks up a coin
            for (int i = 1; i < 4; i++)
            {
                if (player.GetRectangle().Intersects(coins[i].GetRectangle()) && !coinPickedUp[i])
                {
                    coinPickedUp[i] = true;
                    coinCount++;
                }
            }

            // Checks if the player picks up keys
            if (!hasKey[1] && player.GetRectangle().Intersects(keys[1].GetRectangle()))
            {
                hasKey[1] = true;
            }

            // Activates levers
            if (player.GetRectangle().Intersects(levers[1].GetRectangle()) && player.Interact())
            {
                leverIsActivated[1] = true;
            }

            if (leverIsActivated[1])
            {
                bars[1].PlayAnimation(gameTime, 128, false);
                bars[2].PlayAnimation(gameTime, 128, false);
                levers[1].PlayAnimation(gameTime, 128, false);
            }

            // Checks if a door gets unlocked
            if (hasKey[1] && player.GetRectangle().Intersects(doors[1].GetRectangle()) && player.Interact())
            {
                //player.position = startingPosThree;
                //currentLevel++;
                CurrentGameState = Gamestate.EndScreen;
            }
        }

        //public void CheckLvlThreeActivations(GameTime gameTime)
        //{
        //    // Checks if the player picks up a coin
        //    for (int i = 4; i < coins.Length; i++)
        //    {
        //        if (player.GetRectangle().Intersects(coins[i].GetRectangle()) && !coinPickedUp[i])
        //        {
        //            coinPickedUp[i] = true;
        //            coinCount++;
        //        }
        //    }

        //    // Checks if the player picks up keys
        //    if (!hasKey[2] && player.GetRectangle().Intersects(keys[2].GetRectangle()))
        //    {
        //        hasKey[2] = true;
        //    }

        //    // Activates levers
        //    if (player.GetRectangle().Intersects(levers[2].GetRectangle()) && player.Interact())
        //    {
        //        leverIsActivated[2] = true;
        //    }

        //    if (leverIsActivated[2])
        //    {
        //        bars[3].PlayAnimation(gameTime, 128, false);
        //        levers[2].PlayAnimation(gameTime, 128, false);
        //    }

        //    // Checks if a door gets unlocked
        //    if (hasKey[2] && player.GetRectangle().Intersects(doors[2].GetRectangle()) && player.Interact())
        //    {
        //        CurrentGameState = Gamestate.EndScreen;
        //    }
        //}

        public void AnimateLvlOne(GameTime gameTime)
        {
            coins[0].PlayAnimation(gameTime, 128, true);
            keys[0].PlayAnimation(gameTime, 192, true);
        }

        public void AnimateLvlTwo(GameTime gameTime)
        {
            for (int i = 1; i < 4; i++)
                coins[i].PlayAnimation(gameTime, 128, true);
            keys[1].PlayAnimation(gameTime, 192, true);
        }

        public void AnimateLvlThree(GameTime gameTime)
        {
            for (int i = 4; i < coins.Length; i++)
                coins[i].PlayAnimation(gameTime, 128, true);
            keys[2].PlayAnimation(gameTime, 192, true);
        }
    }
}
