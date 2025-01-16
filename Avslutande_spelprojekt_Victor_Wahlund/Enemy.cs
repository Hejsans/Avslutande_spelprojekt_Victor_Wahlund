using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class Enemy:PhysicalObject
    {
        // Medlemsvariabler
        protected float playerX, playerY;

        // Konstruktor 1, för fiender som inte behöver spelarens position
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation,float rotationSpeed) : base(texture, X, Y, false, speedX, speedY, rotation, rotationSpeed)
        {
        }

        // Konstruktor 2, för fiender som behöver spelarens position
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed, float playerX, float playerY) : base(texture, X, Y, false, speedX, speedY, rotation, rotationSpeed)
        {
            this.playerX = playerX;
            this.playerY = playerY;
        }

        // Update-metod 1 för fiender som inte behöver följa spelaren
        public virtual void Update(GameWindow window, GameTime gameTime)
        {

        }

        // Update-metod 2 för fiender som behöver följa spelaren
        public virtual void Update(GameWindow window, float playerX, float playerY)
        {

        }
    }
}
