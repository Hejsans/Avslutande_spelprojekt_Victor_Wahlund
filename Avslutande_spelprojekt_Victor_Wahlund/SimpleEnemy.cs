using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal abstract class SimpleEnemy:Enemy
    {
        public SimpleEnemy(Texture2D texture, float X, float Y, float speedX, float speedY, float rotation, float rotationSpeed) : base(texture, X, Y, speedX, speedY, rotation, rotationSpeed)
        {
        }
    }
}
