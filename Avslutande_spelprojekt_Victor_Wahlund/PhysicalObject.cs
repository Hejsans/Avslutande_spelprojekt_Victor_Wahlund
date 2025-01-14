using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class PhysicalObject:MovingObject
    {
        // Medlemsvariabler
        protected bool isAlive = true;
        private bool isFriendly;

        // Konstruktor
        /// <summary>
        /// Grundläggande mall för alla objekt som kan kollidera eller dö
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="isFriendly"> Visar om objektet är vänligt mot spelaren eller inte </param>
        /// <param name="speedX"></param>
        /// <param name="speedY"></param>
        /// <param name="rotation"></param>
        /// <param name="rotationSpeed"></param>
        public PhysicalObject(Texture2D texture, float X, float Y, bool isFriendly, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y, speedX, speedY, rotation, rotationSpeed)
        {
            this.isFriendly = isFriendly;
        }

        public bool CheckCollision(PhysicalObject other)  // Metod som kollar om objektet har kolliderat med ett annat (other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(other.Width), Convert.ToInt32(other.Height));

            return myRect.IntersectsWith(otherRect);  // Returnerar true om de har kolliderat och false om inte
        }

        // Egenskaper

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public bool IsFriendly
        {
            get { return isFriendly; }
        }
    }
}
