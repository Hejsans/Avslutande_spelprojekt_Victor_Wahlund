using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class TowerEnemy:Enemy   // TowerEnemy är en stationär fiende som skjuter mot spelaren
    {
        // Medlemsvariabler
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        int timeUntilNextBullet = 0;
        Random random;

        // Konstruktor
        public TowerEnemy(Texture2D texture, float X, float Y, Texture2D bulletTexture, float rotation, float rotationSpeed, float playerX, float playerY) : base(texture, X, Y, 0f, 0f, rotation, rotationSpeed, playerX, playerY)
        {
            bullets = new List<Bullet>();  // Skapar en ny lista med skott
            this.bulletTexture = bulletTexture;

            random = new Random();

            rotationOrigin.Y = 24; // Texturen på tornet är annorlunda än de andra och roterar inte runt mitten
        }

        public override void Update(GameWindow window, GameTime gameTime, float playerX, float playerY)
        {
            // Räknar ut spelarens koordinater i et koordiatsystem där fienden är origo
            playerX = playerX - X;
            playerY = playerY - Y;

            // Räknar ut vinkeln som fienden behöver ha för att peka på spelaren
            if (playerX > 0)
                rotation = (float)(Math.Asin(playerY / Math.Sqrt(Math.Pow(playerY, 2) + Math.Pow(playerX, 2))) + Math.PI / 2);
            else
                rotation = (float)-(Math.Asin(playerY / Math.Sqrt(Math.Pow(playerY, 2) + Math.Pow(playerX, 2))) + Math.PI / 2);

            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + timeUntilNextBullet)
            {
                // Skjuter ut ett skott ur kanonen på texturen åt den riktning tornet pekar
                Bullet temp = new Bullet(bulletTexture, vector.X - 20 * (float)Math.Cos(rotation + (Math.PI / 2)), vector.Y - 20 * (float)Math.Sin(rotation + (Math.PI / 2)), false, rotation, 0);
                bullets.Add(temp);

                // Bestämmer nästa gång som fienden ska skjuta
                timeUntilNextBullet = random.Next(500, 2000);
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }

            // Uppdaterar alla bullets och tar bort de som är "döda"
            foreach (Bullet b in bullets.ToList())
            {
                b.Update(window);

                if (!b.IsAlive)
                    bullets.Remove(b);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Ritar ut alla bullets
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);

            // Ritar ut fienden
            spriteBatch.Draw(texture, vector, null, Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }

        // Egenskaper
        public List<Bullet> Bullets { get { return this.bullets; } }
    }
}
