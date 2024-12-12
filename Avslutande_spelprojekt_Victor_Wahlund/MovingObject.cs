using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class MovingObject:GameObject
    {
        protected Vector2 speed;
        protected float rotation = 0f;
        protected Vector2 rotationOrigin;
        protected float rotationSpeed;

        public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y)
        {
            this.speed.X = speedX;
            this.speed.Y = speedY;
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;
            rotationOrigin.X = texture.Width / 2;
            rotationOrigin.Y = texture.Height / 2;
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}
