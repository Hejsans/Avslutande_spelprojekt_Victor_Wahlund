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
        // Medlemsvariabler
        protected Vector2 speed;
        protected float rotation = 0f;
        protected Vector2 rotationOrigin;
        protected float rotationSpeed;

        // Konstruktor
        /// <summary>
        /// Grundläggande mall för alla objekt som rör på sig
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="speedX"> Objektets hastighet i Y-led </param>
        /// <param name="speedY"> Objektets hastighet i X-led </param>
        /// <param name="rotation"> Objektets rotation i radianer (0 är rakt upp) </param>
        /// <param name="rotationSpeed"> Objektets rotationshastighet (positivt är medurs och negativt är moturs) </param>
        public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y)
        {
            this.speed.X = speedX;
            this.speed.Y = speedY;
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;

            // rotationOrigin är punkten som objektet roteras från, vilket som standard är mitt i texturen
            rotationOrigin.X = texture.Width / 2;
            rotationOrigin.Y = texture.Height / 2;
        }

        // Egenskaper
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}
