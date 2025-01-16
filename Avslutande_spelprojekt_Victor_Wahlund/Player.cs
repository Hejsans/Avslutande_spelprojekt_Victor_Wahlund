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
    internal class Player:PhysicalObject
    {
        // Medlemsvariabler
        int points = 0;

        // Konstruktor
        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y, true, speedX, speedY, rotation, rotationSpeed)
        {
            
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();   // Läs in tangenttryckning

            // Rotera chassit
            if (keyboardState.IsKeyDown(Keys.D))
                rotation += rotationSpeed;
            if (keyboardState.IsKeyDown(Keys.A))
                rotation -= rotationSpeed;

            // Kör fram eller bak dit chassit pekar
            if (keyboardState.IsKeyDown(Keys.W))
            {
                vector.Y += (float)Math.Sin(rotation - Math.PI / 2) * speed.Y;
                vector.X += (float)Math.Cos(rotation - Math.PI / 2) * speed.X;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                vector.Y -= (float)Math.Sin(rotation - Math.PI / 2) * speed.Y / 2;
                vector.X -= (float)Math.Cos(rotation - Math.PI / 2) * speed.X / 2;
            }

            // Kontrollera ifall skeppet har åkt ut från kanten, återställ i så fall dess position
            if (vector.X < 0)
                vector.X = 0;
            if (vector.X > window.ClientBounds.Width - texture.Width)
                vector.X = window.ClientBounds.Width - texture.Width;
            if (vector.Y < 0)
                vector.Y = 0;
            if (vector.Y > window.ClientBounds.Height - texture.Height)
                vector.Y = window.ClientBounds.Height - texture.Height;

            // Gå till menyn om man trycker på escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rita ut chassit
            spriteBatch.Draw(texture, vector, null,Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }

        // Egenskaper
        public int Points
        {
            get { return this.points; }
            set { this.points = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        // Återställ spelaren
        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            isAlive = true;
            rotation = 0;
        }
    }
}
