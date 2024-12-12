using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class ComplexEnemy:Enemy
    {
        protected float playerX, playerY;

        public ComplexEnemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed, float playerX, float playerY):base(texture, X, Y, speedX, speedY, rotation, rotationSpeed)
        {
            this.playerX = playerX;
            this.playerY = playerY;
        }

        public abstract void Update(GameWindow window, float playerX, float playerY);
    }
}
