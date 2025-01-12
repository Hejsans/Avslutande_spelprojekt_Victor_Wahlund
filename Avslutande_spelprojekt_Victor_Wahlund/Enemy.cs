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
        protected float playerX, playerY;

        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation,float rotationSpeed) : base(texture, X, Y, false, speedX, speedY, rotation, rotationSpeed)
        {
        }

        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed, float playerX, float playerY) : base(texture, X, Y, false, speedX, speedY, rotation, rotationSpeed)
        {
            this.playerX = playerX;
            this.playerY = playerY;
        }

        public virtual void Update(GameWindow window, GameTime gameTime)
        {

        }
        public virtual void Update(GameWindow window, float playerX, float playerY)
        {

        }
    }
}
