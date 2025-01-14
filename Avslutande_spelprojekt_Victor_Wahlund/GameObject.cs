using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class GameObject
    {
        // Medlemsvariabler
        protected Texture2D texture;
        protected Vector2 vector;

        // Konstruktor
        /// <summary>
        /// Grundläggande mall för alla objekt i spelet
        /// </summary>
        /// <param name="texture"> Objektets textur </param>
        /// <param name="X"> Objektets X-koordinat </param>
        /// <param name="Y"> Objektets Y-koordinat </param>
        public GameObject(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            this.vector.X = X;
            this.vector.Y = Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);   // Ritar ut objektet
        }

        // Egenskaper
        public float X
        {
            get { return this.vector.X; }
        }

        public float Y
        {
            get { return this.vector.Y; }
        }

        public float Width
        {
            get { return texture.Width; }
        }

        public float Height
        {
            get { return texture.Height; }
        }
    }
}
