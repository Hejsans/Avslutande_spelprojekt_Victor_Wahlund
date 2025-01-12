﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private StreamWriter sw;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            sw = new StreamWriter("highscore.txt", true);
            sw.Close();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
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

                case GameElements.State.EnterHighScore:
                    GameElements.currentState = GameElements.EnterHighScoreUpdate(gameTime);
                    break;

                case GameElements.State.PrintHighScore:
                    GameElements.currentState = GameElements.PrintHighScoreUpdate(gameTime);
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                case GameElements.State.Win:
                    GameElements.currentState = GameElements.WinUpdate(Window, Content);
                    break;

                case GameElements.State.Lose:
                    GameElements.currentState = GameElements.LoseUpdate(Window, Content);
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

                case GameElements.State.EnterHighScore:
                    GameElements.EnterHighScoreDraw(_spriteBatch);
                    break;

                case GameElements.State.PrintHighScore:
                    GameElements.PrintHighScoreDraw(_spriteBatch);
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                case GameElements.State.Win:
                    GameElements.WinDraw(_spriteBatch, Window);
                    break;

                case GameElements.State.Lose:
                    GameElements.LoseDraw(_spriteBatch, Window);
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
