﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class Mine:Enemy   // Mine är en fiende som alltid följer efter spelaren
    {
        // Konstruktor
        public Mine(Texture2D texture, float X, float Y, float rotation, float rotationSpeed, float playerX, float playerY) : base(texture, X, Y, 1.4f, 1.4f, rotation, rotationSpeed, playerX, playerY)
        {
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

            // Åker dit fienden pekar
            vector.Y += (float)Math.Sin(rotation - Math.PI / 2) * speed.Y;
            vector.X += (float)Math.Cos(rotation - Math.PI / 2) * speed.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Ritar ut fienden
            spriteBatch.Draw(texture, vector, null, Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
