using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Avslutande_spelprojekt_Victor_Wahlund
{
    internal class LevelHandler
    {
        private int currentLevel;
        private Player player;
        private Tower playerTower;
        private List<Enemy> enemies;

        public LevelHandler(ContentManager content)
        {
            currentLevel = 1;
            enemies = new List<Enemy>();

            player = new Player(content.Load<Texture2D>("images/player/chassi"), 380, 400, 3f, 3f, 0f, 0.05f);
            playerTower = new Tower(content.Load<Texture2D>("images/player/tower"), 380, 400, true, 3f, 3f, content.Load<Texture2D>("images/bullet"), 0, 0.05f);
        }

        public void LoadLevel(ContentManager content, GameWindow window)
        {
            enemies.Clear();

            Random random = new Random();
            Texture2D mineSprite = content.Load<Texture2D>("images/enemy/mine");
            //Texture2D tripodSprite = Content.Load<Texture2D>("images/enemy/tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - mineSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);

                Mine mine = new Mine(mineSprite, rndX, rndY, 0f, 1f, player.X, player.Y);
                enemies.Add(mine);
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    int rndX = random.Next(0, Window.ClientBounds.Width - tripodSprite.Width);
            //    int rndY = random.Next(0, Window.ClientBounds.Height / 2);

            //    Tripod tripod = new Tripod(tripodSprite, rndX, rndY);
            //    enemies.Add(tripod);
            //}
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            player.Update(window, gameTime);
            playerTower.Update(window, gameTime, player.X, player.Y + ((player.Texture.Height - playerTower.Texture.Height) / 2));
            foreach (Enemy e in enemies.ToList())
            {
                foreach (Bullet b in playerTower.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        b.IsAlive = false;
                        player.Points++;
                    }
                }
                if (e.IsAlive)
                {
                    if (e.CheckCollision(player))
                        player.IsAlive = false;

                    if (e is ComplexEnemy)
                        (e as ComplexEnemy).Update(window, player.X, player.Y);
                    else
                        e.Update(window);
                }
                else
                {
                    enemies.Remove(e);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            playerTower.Draw(spriteBatch);
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }

        public void Reset(ContentManager content, GameWindow window)
        {
            player.Reset(380, 400, 3f, 3f);
            playerTower.Reset(380, 400, 3f, 3f);

            LoadLevel(content, window);
        }

        public Player Player
        {
            get { return player; }
        }
    }
}
