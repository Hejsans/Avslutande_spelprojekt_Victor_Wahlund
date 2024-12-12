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
        protected bool isAlive = true;
        private bool isFriendly;

        public PhysicalObject(Texture2D texture, float X, float Y, bool isFriendly, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y, speedX, speedY, rotation, rotationSpeed)
        {
            this.isFriendly = isFriendly;
        }

        public bool CheckCollision(PhysicalObject other)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(other.Width), Convert.ToInt32(other.Height));

            return myRect.IntersectsWith(otherRect);
        }

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
