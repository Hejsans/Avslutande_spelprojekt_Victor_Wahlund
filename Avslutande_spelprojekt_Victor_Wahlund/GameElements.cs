using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal static class GameElements
    {
        static Background background;
        static Menu menu;
        static PrintText printText;
        static LevelHandler levelHandler;
        static HighScore highscore;
        static SpriteFont font;

        public enum State { Menu, Run, Quit, Win, Lose, EnterHighScore, PrintHighScore };
        public static State currentState;

        public static void Initialize()
        {
        }

        public static void LoadContent(ContentManager Content, GameWindow Window)
        {
            background = new Background(Content.Load<Texture2D>("images/background"), Window);
            highscore = new HighScore(10);
            font = Content.Load<SpriteFont>("myFont");

            menu = new Menu((int)State.Menu);
            menu.AddItem(Content.Load<Texture2D>("images/menu/start"), (int)State.Run, Window);
            menu.AddItem(Content.Load<Texture2D>("images/menu/highscore"), (int)State.EnterHighScore, Window);
            menu.AddItem(Content.Load<Texture2D>("images/menu/exit"), (int)State.Quit, Window);

            levelHandler = new LevelHandler(Content, Window);
            levelHandler.LoadLevel(Content, Window);
            
            printText = new PrintText(font);
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
            levelHandler.Update(Content, Window, gameTime);

            if (!levelHandler.Player.IsAlive)
                return State.Lose;
            else if (levelHandler.Win)
                return State.Win;

            return State.Run;
        }

        public static void RunDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            levelHandler.Draw(spriteBatch);

            printText.Print($"Points: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, 0, 0);
        }

        public static State EnterHighScoreUpdate(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;

            if (highscore.EnterUpdate(gameTime, levelHandler.Player.Points))
            {
                highscore.SaveToFile("highscore.txt");
                return State.PrintHighScore;
            }

            return State.EnterHighScore;
        }

        public static void EnterHighScoreDraw(SpriteBatch spriteBatch)
        {
            highscore.EnterDraw(spriteBatch, font);
        }

        public static State PrintHighScoreUpdate(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;

            if (keyboardState.IsKeyDown(Keys.Enter))
                return State.EnterHighScore;
            return State.PrintHighScore;
        }

        public static void PrintHighScoreDraw(SpriteBatch spriteBatch)
        {
            highscore.PrintDraw(spriteBatch, font);
        }

        public static State WinUpdate(GameWindow window, ContentManager content)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                levelHandler.CurrentLevel = 1;
                Reset(window, content);
                return State.Menu;
            }
            return State.Win;
        }

        public static void WinDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            background.Draw(spriteBatch);

            printText.Print($"You won!!! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, window.ClientBounds.Width/2, window.ClientBounds.Height/2);
            printText.Print($"Press escape to return to menu", spriteBatch, window.ClientBounds.Width / 2, (window.ClientBounds.Height / 2) + 50);
        }

        public static State LoseUpdate(GameWindow window, ContentManager content)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Reset(window, content);
                return State.Menu;
            }
                
            return State.Lose;
        }

        public static void LoseDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            background.Draw(spriteBatch);

            printText.Print($"You died :(  Try again from the level you died! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
            printText.Print($"Press escape to return to menu", spriteBatch, window.ClientBounds.Width / 2, (window.ClientBounds.Height / 2) + 50);
        }

        private static void Reset(GameWindow Window, ContentManager Content)
        {
            levelHandler.Reset(Content, Window);
        }
    }
}
