using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class SprayMine:Enemy  // SprayMine är en fiende som står stilla och skjuter ut skott åt alla håll
    {
        // Medlemsvariabler
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        int timeUntilNextBullet = 0;
        Random random;

        // Konstruktor
        public SprayMine(Texture2D texture, float X, float Y, Texture2D bulletTexture, float rotation, float rotationSpeed):base(texture, X, Y, 0f, 0f, rotation, rotationSpeed)
        {
            bullets = new List<Bullet>();  // Skapar en ny lista med skott
            this.bulletTexture = bulletTexture;

            random = new Random();
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            // Väntar mellan 1,5 och 9 sekunder på att skjuta efter senaste gången
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + timeUntilNextBullet)
            {
                // Skjuter ut 8 olika skott i en cirkel runt fienden
                for (int i = 0; i < 8; i++)
                {
                    Bullet temp = new Bullet(bulletTexture, X, Y, false, (float)(i * Math.PI / 4), 0);
                    bullets.Add(temp);
                }
                // Bestämmer nästa gång som fienden ska skjuta
                timeUntilNextBullet = random.Next(1500, 9000);
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

            // Ritar ut spelaren
            spriteBatch.Draw(texture, vector, null, Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }

        // Egenskaper
        public List<Bullet> Bullets { get { return this.bullets; } }
    }
}
