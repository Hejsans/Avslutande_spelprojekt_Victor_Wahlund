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
    internal class Tower : PhysicalObject
    {
        // Medlemsvariabler
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;

        // Konstruktor
        /// <summary>
        /// Torn som kan vara vänligt (spelarens torn) eller inte och som skjuter bullets
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="isFriendly"></param>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        /// <param name="bulletTexture"> Skottets textur </param>
        /// <param name="rotation"></param>
        /// <param name="rotationSpeed"></param>
        public Tower(Texture2D texture, float X, float Y, bool isFriendly, float speedX, float speedY, Texture2D bulletTexture, float rotation, float rotationSpeed) : base(texture, X, Y, isFriendly, speedX, speedY, 0, rotationSpeed)
        {
            bullets = new List<Bullet>();  // Skapar en lista med Bullets 
            this.bulletTexture = bulletTexture;
            rotationOrigin.Y = 24; // Texturen på tornet är annorlunda än de andra och roterar inte runt mitten
            
        }

        /// <summary>
        /// Uppdaterar tornet och alla skott
        /// </summary>
        /// <param name="window"></param>
        /// <param name="gameTime"></param>
        /// <param name="X"> Spelarens X </param>
        /// <param name="Y"> Spelarens Y </param>
        public void Update(GameWindow window, GameTime gameTime, float X, float Y)
        {
            // Tornets x och y ställs så det är rakt över spelaren hela tiden
            vector.X = X;
            vector.Y = Y - 12;  // -12 eftersom tornets textur är annorlunda och måste hamna rätt

            // Kollar om tornet är vänligt (är spelarens torn) och går att styra om den är det
            if (IsFriendly)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                // Roterar tornet när man trycker på pilknapparna
                if (keyboardState.IsKeyDown(Keys.Right))
                    rotation += rotationSpeed;
                if (keyboardState.IsKeyDown(Keys.Left))
                    rotation -= rotationSpeed;

                // Skjuter när man trycker på mellanslag
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    // Kollar om det har gått 200 millisekunder sen senaste gången man sköt
                    if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                    {
                        // Skapar en bullet som pekar åt samma håll som tornet
                        Bullet temp = new Bullet(bulletTexture, vector.X - 20 * (float)Math.Cos(rotation + (Math.PI / 2)), vector.Y - 20 * (float)Math.Sin(rotation + (Math.PI / 2)), true, rotation, 0);
                        
                        // Lägger den i listan bullets och uppdaterar timeSinceLastBullet
                        bullets.Add(temp);
                        timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }

            // Uppdaterar varje bullet i listan bullets
            foreach (Bullet b in bullets.ToList())
            {
                b.Update(window);

                // Tar bort alla som inte "lever" längre
                if (!b.IsAlive)
                    bullets.Remove(b);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Ritar alla bullets
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);

            spriteBatch.Draw(texture, vector, null, Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }

        // Egenskaper

        public List<Bullet> Bullets { get { return this.bullets; } }

        public Texture2D Texture
        {
            get { return texture; }
        }

        // Återställer tornet och tömmer listan bullets
        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            bullets.Clear();
            timeSinceLastBullet = 0;
            isAlive = true;
            rotation = 0;
        }
    }
}
