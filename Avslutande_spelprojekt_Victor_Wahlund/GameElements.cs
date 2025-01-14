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
        // Mendlemsvariabler
        static Background background;
        static Menu menu;
        static PrintText printText;
        static LevelHandler levelHandler;
        static HighScore highscore;
        static SpriteFont font;

        public enum State { Menu, Run, Quit, Win, Lose, EnterHighScore, PrintHighScore };   // Skapar olika "states" spelet kan vara i
        public static State currentState;

        public static void Initialize()
        {
        }

        public static void LoadContent(ContentManager Content, GameWindow Window)    // Laddar in texturer, font och skapar objekt
        {
            // Laddar in en font
            font = Content.Load<SpriteFont>("myFont");

            // Skapar Background, Highscore och PrintText objekt
            background = new Background(Content.Load<Texture2D>("images/background"), Window);
            highscore = new HighScore(10);
            printText = new PrintText(font);

            // Laddar in och skapar menyn
            menu = new Menu((int)State.Menu);
            menu.AddItem(Content.Load<Texture2D>("images/menu/start"), (int)State.Run, Window);
            menu.AddItem(Content.Load<Texture2D>("images/menu/highscore"), (int)State.EnterHighScore, Window);
            menu.AddItem(Content.Load<Texture2D>("images/menu/exit"), (int)State.Quit, Window);

            // Laddar in highscore från fil
            highscore.LoadFromFile("highscore.txt");

            // Skapar ett LevelHandler-objekt och laddar in första banan
            levelHandler = new LevelHandler(Content, Window);
            levelHandler.LoadLevel(Content, Window);  
        }

        public static State MenuUpdate(GameTime gameTime)   // Menyns update-metod
        {
            return (State)menu.Update(gameTime); // Returnerar olika States beroende på om man har tryckt på en knapp i menyn
        }

        public static void MenuDraw(SpriteBatch spriteBatch)  // Menyns draw-metod
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        public static State RunUpdate(ContentManager Content, GameWindow Window, GameTime gameTime)   // Update-metoden till när spelet körs
        {
            // Uppdaterar bakgrunden och levelHandler
            background.Update(Window);
            levelHandler.Update(Content, Window, gameTime);

            // Ändrar currentState till Lose om spelaren är död (gör så man förlorar) och till Win om LevelHandler.Win är sant (vilket den är när alla banor är avklarade)
            if (!levelHandler.Player.IsAlive)
                return State.Lose;
            else if (levelHandler.Win)
                return State.Win;

            return State.Run;    // Fortsätter köra spelet om man varken har förlorat eller vunnit
        }

        public static void RunDraw(SpriteBatch spriteBatch)   // Draw-metoden till när spelet körs
        {
            background.Draw(spriteBatch);
            levelHandler.Draw(spriteBatch);

            // Skapar en text uppe i vänstra hörnet som visar vilken bana spelaren är på och hur många poäng spelaren har
            printText.Print($"Points: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, 0, 0);
        }

        public static State EnterHighScoreUpdate(GameTime gameTime)   // Update-metoden för när man skriver in sin highscore
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Gå till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;

            // Byter state till PrintHighScore när man har skrivit in sitt highscore och sparar highscoren i filen highscore.txt
            if (highscore.EnterUpdate(gameTime, levelHandler.Player.Points))
            {
                highscore.SaveToFile("highscore.txt");
                return State.PrintHighScore;
            }

            return State.EnterHighScore;
        }

        public static void EnterHighScoreDraw(SpriteBatch spriteBatch)   // Draw-metoden för när man skriver in sin highscore
        {
            highscore.EnterDraw(spriteBatch, font);
        }

        public static State PrintHighScoreUpdate(GameTime gameTime)    // Update-metoden för när highscore-listan visas
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Gå till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;

            // Byter state till EnterHighscore om man trycker på enter
            if (keyboardState.IsKeyDown(Keys.Enter))
                return State.EnterHighScore;
            return State.PrintHighScore;
        }

        public static void PrintHighScoreDraw(SpriteBatch spriteBatch)   // Draw-metoden för när highscore-listan visas
        {
            highscore.PrintDraw(spriteBatch, font);
        }

        public static State WinUpdate(GameWindow window, ContentManager content)    // Update-metoden för när man har vunnit
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Går till menyn och återställer spelet när man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                levelHandler.CurrentLevel = 1;
                Reset(window, content);
                return State.Menu;
            }
            return State.Win;
        }

        public static void WinDraw(SpriteBatch spriteBatch, GameWindow window)    // Draw-metoden för när man har vunnit
        {
            background.Draw(spriteBatch);

            // Gratulerar spelaren och visar både vilken bana den var på och poäng samt säger hur man går tillbaka till menyn
            printText.Print($"You won!!! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, window.ClientBounds.Width/2, window.ClientBounds.Height/2);
            printText.Print($"Press escape to return to menu", spriteBatch, window.ClientBounds.Width / 2, (window.ClientBounds.Height / 2) + 60);
        }

        public static State LoseUpdate(GameWindow window, ContentManager content)   // Update-metoden för när man har förlorat
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Kallar reset-metoden och går till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Reset(window, content);
                return State.Menu;
            }
                
            return State.Lose;
        }

        public static void LoseDraw(SpriteBatch spriteBatch, GameWindow window)    // Draw-metoden för när man har förlorat
        {
            background.Draw(spriteBatch);

            // Visar både vilken bana spelaren var på och poäng samt säger hur man går tillbaka till menyn
            printText.Print($"You died :(  Try again from the level you died! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}", spriteBatch, window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
            printText.Print($"Press escape to return to menu", spriteBatch, window.ClientBounds.Width / 2, (window.ClientBounds.Height / 2) + 50);
        }

        private static void Reset(GameWindow Window, ContentManager Content)   // Återställer spelet
        {
            levelHandler.Reset(Content, Window);  // Kallar reset-metoden i levelHandler
        }
    }
}
