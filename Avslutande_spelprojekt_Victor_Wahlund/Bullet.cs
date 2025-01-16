using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class Bullet:PhysicalObject
    {
        // Konstruktor
        public Bullet(Texture2D texture, float X, float Y, bool isFriendly, float rotation, float rotationSpeed) : base(texture, X, Y, isFriendly, 4f, 4f, rotation, rotationSpeed)
        {
        }

        public void Update(GameWindow window)
        {
            // Åker åt det håll den pekar
            vector.Y += (float)Math.Sin(rotation - Math.PI / 2) * speed.Y;
            vector.X += (float)Math.Cos(rotation - Math.PI / 2) * speed.X;

            // Skottet tas bort om den nuddar någon av kanterna på skärmen
            if (vector.X < 0)
                isAlive = false;
            if (vector.X > window.ClientBounds.Width - texture.Width)
                isAlive = false;
            if (vector.Y < 0)
                isAlive = false;
            if (vector.Y > window.ClientBounds.Height - texture.Height)
                isAlive = false;
        }
    }
}
