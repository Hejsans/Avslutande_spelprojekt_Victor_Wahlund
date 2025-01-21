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
using System.Diagnostics;

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
        static bool diedLastGame;

        public enum State { Menu, Run, Quit, Win, Lose, EnterHighScore, PrintHighScore, Instructions };   // Skapar olika "states" spelet kan vara i
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
            menu.AddItem(Content.Load<Texture2D>("images/menu/highscore"), (int)State.PrintHighScore, Window);
            menu.AddItem(Content.Load<Texture2D>("images/menu/exit"), (int)State.Quit, Window);

            // Laddar in highscore från fil
            highscore.LoadFromFile("highscore.txt");

            // Skapar ett LevelHandler-objekt och laddar in första banan
            levelHandler = new LevelHandler(Content, Window);
            levelHandler.LoadLevel(Content, Window);

            // Spelaren har inte dött förra spelet
            diedLastGame = false;
        }

        public static State InstructionUpdate()    // Instruktionernas update-metod
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Gå till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;
            return State.Instructions;
        }

        public static void InstructionDraw(SpriteBatch spriteBatch, GameWindow window)  // Instruktionernas draw-metod
        {
            // Ger spelaren instruktioner om hur man spelar spelet
            printText.Print("Instructions", spriteBatch, 50, 20);
            printText.Print("-Use WASD to drive the tank and LEFT/RIGHT arrows to control the turret\r\n-Press SPACE to shoot\r\n" +
                "-Use UP/DOWN arrows to control the menu and ENTER to select\r\n-You can press ESCAPE at any time to return to the main menu", spriteBatch, 50, 40);
            printText.Print("You will earn one point for each enemy killed and you will keep your points as long as you keep winning\r\nIf you die your points will be reset and if you beat the game" +
                "you can replay all the levels keeping your points", spriteBatch, 50, 150);
            printText.Print("Press escape to go to the main menu", spriteBatch, 50, 250);
        }

        public static State MenuUpdate(GameTime gameTime)   // Menyns update-metod
        {
            // Olika States beroende på om man har tryckt på en knapp i menyn
            State menuState = (State)menu.Update(gameTime);  

            // Återställer poängen om man dog första rundan, detta utförs här så spelaren ska få en chans att skriva in sin highscore innan poängen återsälls
            if (menuState == State.Run && diedLastGame)
                levelHandler.Player.Points = 0;

            // Returnerar den State man har tryckt på i menyn
            return menuState;   
        }

        public static void MenuDraw(SpriteBatch spriteBatch)  // Menyns draw-metod
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        public static State RunUpdate(ContentManager Content, GameWindow Window, GameTime gameTime)   // Update-metoden till när spelet körs
        {
            // Uppdaterar levelHandler
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

            // Ger instruktioner om hur man använder Highscore-listan
            printText.Print("Use UP/DOWN arrows to select character and RIGHT arrow to choose the next character \r\n" +
                "You can enter a 3 character namne that will be saved along with your highscore\r\n" +
                "Press RIGHT one more time after entering your letters to see the highscore list", spriteBatch, 800, 1);
        }

        public static State PrintHighScoreUpdate(GameTime gameTime)    // Update-metoden för när highscore-listan visas
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Gå till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;

            // Byter state till EnterHighscore om man trycker på enter
            if (keyboardState.IsKeyDown(Keys.E))
                return State.EnterHighScore;
            return State.PrintHighScore;
        }

        public static void PrintHighScoreDraw(SpriteBatch spriteBatch)   // Draw-metoden för när highscore-listan visas
        {
            highscore.PrintDraw(spriteBatch, font);

            // Ger instruktioner om hur man använder Highscore-listan
            printText.Print("This is the highscore list, it contains the top 10 highscores saved on this computer\r\nPress E to enter your score into the list", spriteBatch, 800, 1);
        }

        public static State WinUpdate(GameWindow window, ContentManager content)    // Update-metoden för när man har vunnit
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Går till menyn och återställer spelet när man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                levelHandler.CurrentLevel = 1;
                Reset(window, content);
                diedLastGame = false;
                return State.Menu;
            }
            return State.Win;
        }

        public static void WinDraw(SpriteBatch spriteBatch, GameWindow window)    // Draw-metoden för när man har vunnit
        {
            background.Draw(spriteBatch);

            // Gratulerar spelaren och visar både vilken bana den var på och poäng samt säger hur man går tillbaka till menyn
            printText.Print($"You won!!! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}" +
                "\r\n Press escape to return to the menu", spriteBatch, window.ClientBounds.Width/2, window.ClientBounds.Height/2);
        }

        public static State LoseUpdate(GameWindow window, ContentManager content)   // Update-metoden för när man har förlorat
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Kallar reset-metoden och går till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Reset(window, content);
                diedLastGame = true;     // Registrerar att man har dött, detta innebär att ens poäng kommer återställas när man kör igen
                return State.Menu;
            }
                
            return State.Lose;
        }

        public static void LoseDraw(SpriteBatch spriteBatch, GameWindow window)    // Draw-metoden för när man har förlorat
        {
            background.Draw(spriteBatch);

            // Visar både vilken bana spelaren var på och poäng samt säger hur man går tillbaka till menyn
            printText.Print($"You died :(  Try again from the level you died! \r\nPoints: {levelHandler.Player.Points} \r\nLevel {levelHandler.CurrentLevel}" +
                "\r\n Press escape to return to the menu", spriteBatch, window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
        }

        private static void Reset(GameWindow Window, ContentManager Content)   // Återställer spelet
        {
            levelHandler.Reset(Content, Window);  // Kallar reset-metoden i levelHandler
        }
    }
}
