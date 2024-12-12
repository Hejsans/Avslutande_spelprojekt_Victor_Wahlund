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
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;

        public Tower(Texture2D texture, float X, float Y, bool isFriendly, float speedX, float speedY, Texture2D bulletTexture, float rotation, float rotationSpeed) : base(texture, X, Y, isFriendly, speedX, speedY, 0, rotationSpeed)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
            rotationOrigin.Y = 24;


            //towerX = X - ((texture.Width - towerTexture.Width) / 2);
            //towerY = Y - ((texture.Height - towerTexture.Height) / 2);
            
        }

        public void Update(GameWindow window, GameTime gameTime, float X, float Y)
        {
            vector.X = X;
            vector.Y = Y - 12;

            if (IsFriendly)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Right))
                    rotation += rotationSpeed;
                if (keyboardState.IsKeyDown(Keys.Left))
                    rotation -= rotationSpeed;

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                    {
                        Bullet temp = new Bullet(bulletTexture, vector.X - 20 * (float)Math.Cos(rotation + (Math.PI / 2)), vector.Y - 20 * (float)Math.Sin(rotation + (Math.PI / 2)), true, rotation, 0);
                        bullets.Add(temp);
                        timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
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

        public Texture2D Texture
        {
            get { return texture; }
        }

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
