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
    class MenuItem
    {
        // Medlemsvariabler
        Texture2D texture;
        Vector2 position;
        int currentState;

        // Konstruktor
        /// <summary>
        /// Varje knapp i menyn är en MenuItem
        /// </summary>
        /// <param name="texture"> Knappens textur </param>
        /// <param name="position"> Knappens position </param>
        /// <param name="currentState"> Vad som ska hända när man trycker på knappen (vilken state man hamnar i) </param>
        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.texture = texture;
            this.position = position;
            this.currentState = currentState;
        }

        // Egenskaper
        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public int State
        {
            get { return currentState; }
        }
    }

    class Menu
    {
        // Medlemsvariabler
        List<MenuItem> menu;
        int selected = 0;
        float currentHeight = 0;
        double lastChange = 0;
        int defaultMenuState;

        // Konstruktor
        /// <summary>
        /// Hela menyn
        /// </summary>
        /// <param name="defaultMenuState"> Den state menyn är i normalt </param>
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();  // Skapar en lista med flera MenuItem
            this.defaultMenuState = defaultMenuState;
        }

        // Metod för att lägga till en knapp i menyn
        public void AddItem(Texture2D itemTexture, int state, GameWindow window)
        {
            // Sätter menyn mitt i skärmen
            float X = (window.ClientBounds.Width/2) - itemTexture.Width/2;
            float Y = (window.ClientBounds.Height/2) - (itemTexture.Height * 3) + currentHeight;

            // Skapar mellanrum mellan knapparna
            currentHeight += itemTexture.Height + 20;

            // Skapar en ny MenuItem och lägger till den i listan "menu"
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);
            menu.Add(temp);
        }

        public int Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)  // Kollar om det har gått 130 ms sedan senaste knapptryckningen
            {
                // Bläddrar i listan när man trycker på upp/ner pilarna
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    selected--;
                    if (selected < 0)
                        selected = menu.Count - 1;
                }

                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }

            // Returnerar den state som spelet ska gå in i när man har tryckt på enter (valt ett alternativ i listan)
            if (keyboardState.IsKeyDown(Keys.Enter))
                return menu[selected].State;

            return defaultMenuState;  // Returnerar menyns state om användaren inte har tryckt på någon av knapparna i menyn
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Ritar ut alla knappar i menyn
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selected)
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.RosyBrown);  // Den valda knappen är ljusbrun
                else
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);  // Alla andra knappar är vita
            }
        }
    }
}
