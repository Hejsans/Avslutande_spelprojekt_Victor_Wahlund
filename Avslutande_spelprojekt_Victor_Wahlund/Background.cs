using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    class BackgroundSprite : GameObject
    {
        // Konstruktor
        public BackgroundSprite(Texture2D texture, float X, float Y) : base(texture, X, Y)  
        {
        }
    }

    class Background
    {
        // Medlemsvariabler
        BackgroundSprite[,] background;
        int nrBackgroundsX, nrBackgroundsY;

        // Konstruktor
        public Background(Texture2D texture, GameWindow window)
        {
            // Räknar ut hur många BackgroundSprite som behövs för att fylla skärmen
            double tmpX = (double)window.ClientBounds.Width / texture.Width;
            nrBackgroundsX = (int)Math.Ceiling(tmpX);
            double tmpY = (double)window.ClientBounds.Height / texture.Height;
            nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;

            // Skapar en 2D-array med BackgroundSprite
            background = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];

            // Placerar ut BackgroundSprites över hela skärmen
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    int posX = i * texture.Width;
                    int posY = j * texture.Height - texture.Height;
                    background[i, j] = new BackgroundSprite(texture, posX, posY);  // Säter in BackgroundSpriten i arrayen
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Ritar ut bakgrunden
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i, j].Draw(spriteBatch);
        }
    }
}
