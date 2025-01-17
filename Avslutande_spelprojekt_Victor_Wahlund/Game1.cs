using Microsoft.Xna.Framework;
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
            sw = new StreamWriter("highscore.txt", true);   // Skapar en highscore-fil ifall det int efinns någon så det inte blir error när highscore.txt ska loadas 
            sw.Close();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Gör så spelfönstret blir större (upplösning 1080p)
            _graphics.PreferredBackBufferWidth = 1920;    
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            GameElements.currentState = GameElements.State.Instructions;   // Gör så spelet startar i instruktioner
            GameElements.Initialize();   // Kallar Initialize() i GameElements

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameElements.LoadContent(Content, Window);  // Kallar LoadContent() i GameElements
        }

        protected override void Update(GameTime gameTime)
        {
            switch (GameElements.currentState)   // Kallar rätt update-metod beroende på vilken state spelet är i
            {
                case GameElements.State.Run:   // Om spelet körs
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;

                case GameElements.State.EnterHighScore:  // När man skriver in sin highscore
                    GameElements.currentState = GameElements.EnterHighScoreUpdate(gameTime);
                    break;

                case GameElements.State.PrintHighScore:  // När highscore visas
                    GameElements.currentState = GameElements.PrintHighScoreUpdate(gameTime);
                    break;

                case GameElements.State.Quit: // Spelet avslutas
                    this.Exit();
                    break;

                case GameElements.State.Win:  // När man har vunnit
                    GameElements.currentState = GameElements.WinUpdate(Window, Content);
                    break;

                case GameElements.State.Lose:  // När man har förlorat
                    GameElements.currentState = GameElements.LoseUpdate(Window, Content);
                    break;

                case GameElements.State.Instructions:  // När man kollar på instruktioner i början av spelet
                    GameElements.currentState = GameElements.InstructionUpdate();
                    break;

                default:   // När man är inne i menyn
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)  // Kallar rätt draw-metod beroende på vilken state spelet är i
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:    // Om spelet körs
                    GameElements.RunDraw(_spriteBatch);
                    break;

                case GameElements.State.EnterHighScore:   // När man skriver in sin highscore
                    GameElements.EnterHighScoreDraw(_spriteBatch);
                    break;

                case GameElements.State.PrintHighScore:  // När highscore visas
                    GameElements.PrintHighScoreDraw(_spriteBatch);
                    break;

                case GameElements.State.Quit:   // Spelet avslutas
                    this.Exit();
                    break;

                case GameElements.State.Win:   // När man har vunnit
                    GameElements.WinDraw(_spriteBatch, Window);
                    break;

                case GameElements.State.Lose:   // När man har förlorat
                    GameElements.LoseDraw(_spriteBatch, Window);
                    break;

                case GameElements.State.Instructions:   // När man Kollar på instruktioner i början av spelet
                    GameElements.InstructionDraw(_spriteBatch, Window);
                    break;

                default:    // När man är inne i menyn
                    GameElements.MenuDraw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
