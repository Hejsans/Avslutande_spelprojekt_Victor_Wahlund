using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class PrintText
    {
        // Medlemsvariabler
        SpriteFont font;

        // Konstruktor
        /// <summary>
        /// Skriver ut en text
        /// </summary>
        /// <param name="font"> Textens font </param>
        public PrintText(SpriteFont font)
        {
            this.font = font;
        }

        public void Print(string text, SpriteBatch spriteBatch, int x, int y)
        {
            // Ritar ut texten
            spriteBatch.DrawString(font, text, new Vector2(x, y), Color.White);
        }
    }
}
