using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class SprayMine:SimpleEnemy
    {
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        Random random;

        public SprayMine(Texture2D texture, float X, float Y, Texture2D bulletTexture, float rotation, float rotationSpeed):base(texture, X, Y, 0f, 0f, rotation, rotationSpeed)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;

            random = new Random();
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + random.Next(1500, 9000))
            {
                for (int i = 0; i < 8; i++)
                {
                    Bullet temp = new Bullet(bulletTexture, X, Y, false, (float)(i * Math.PI / 4), 0);
                    bullets.Add(temp);
                }
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }

            foreach (Bullet b in bullets.ToList())
            {
                b.Update(window);

                if (!b.IsAlive)
                    bullets.Remove(b);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);

            spriteBatch.Draw(texture, vector, null, Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }

        public List<Bullet> Bullets { get { return this.bullets; } }
    }
}
