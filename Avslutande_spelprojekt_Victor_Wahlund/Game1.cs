﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameElements.LoadContent(Content, Window);

        }

        protected override void Update(GameTime gameTime)
        {
            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;

                case GameElements.State.HighScore:
                    GameElements.currentState = GameElements.HighScoreUpdate();
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.RunDraw(_spriteBatch);
                    break;

                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(_spriteBatch);
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                default:
                    GameElements.MenuDraw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}