using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal static class GameElements
    {
        static Background background;
        static Menu menu;
        static PrintText printText;
        static LevelHandler levelHandler;

        public enum State { Menu, Run, HighScore, Quit };
        public static State currentState;

        public static void Initialize()
        {
        }

        public static void LoadContent(ContentManager Content, GameWindow Window)
        {
            background = new Background(Content.Load<Texture2D>("images/background"), Window);

            menu = new Menu((int)State.Menu);
            menu.AddItem(Content.Load<Texture2D>("images/menu/start"), (int)State.Run);
            menu.AddItem(Content.Load<Texture2D>("images/menu/highscore"), (int)State.HighScore);
            menu.AddItem(Content.Load<Texture2D>("images/menu/exit"), (int)State.Quit);

            levelHandler = new LevelHandler(Content);
            levelHandler.LoadLevel(Content, Window);
            
            printText = new PrintText(Content.Load<SpriteFont>("myFont"));
        }

        public static State MenuUpdate(GameTime gameTime)
        {
            return (State)menu.Update(gameTime);
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        public static State RunUpdate(ContentManager Content, GameWindow Window, GameTime gameTime)
        {
            background.Update(Window);
            levelHandler.Update(Window, gameTime);

            if (!levelHandler.Player.IsAlive)
            {
                Reset(Window, Content);
                return State.Menu;
            }

            return State.Run;
        }

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            levelHandler.Draw(spriteBatch);

            printText.Print($"Points: {levelHandler.Player.Points} Rotation: {levelHandler.Player.Rotation}", spriteBatch, 0, 0);
        }

        public static State HighScoreUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;
            return State.HighScore;
        }

        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {
            // Rita highscore-listan
        }

        private static void Reset(GameWindow Window, ContentManager Content)
        {
            levelHandler.Reset(Content, Window);
        }
    }
}
