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
        private bool levelUp, win;
        private Texture2D bulletTexture;

        public LevelHandler(ContentManager content, GameWindow window)
        {
            currentLevel = 1;
            enemies = new List<Enemy>();

            bulletTexture = content.Load<Texture2D>("images/bullet");
            player = new Player(content.Load<Texture2D>("images/player/chassi"), window.ClientBounds.Width/2, window.ClientBounds.Height * 0.75f, 3f, 3f, 0f, 0.05f);
            playerTower = new Tower(content.Load<Texture2D>("images/player/tower"), window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, true, 3f, 3f, bulletTexture, 0, 0.08f);
        }

        public void LoadLevel(ContentManager content, GameWindow window)
        {
            enemies.Clear();
            win = false;

            Texture2D mineSprite = content.Load<Texture2D>("images/enemy/mine");
            Texture2D sprayMineSprite = content.Load<Texture2D>("images/enemy/spraymine");
            if (currentLevel == 1)   // Level 1
            {
                Spawn(content, window, mineSprite, 5, "mine");
            }
            else if (currentLevel == 2)   // Level 2
            {
                Spawn(content, window, sprayMineSprite, 5, "spraymine");
            }
            else if (currentLevel == 3)   // Level 3
            {
                Spawn(content, window, sprayMineSprite, 2, "spraymine");
                Spawn(content, window, mineSprite, 3, "mine");
            }
            else     // När alla levels är färdiga och man har klarat spelet
            {
                win = true;
                //currentLevel = 1;
            }

        }

        public void Update(ContentManager content, GameWindow window, GameTime gameTime)
        {
            levelUp = true;

            player.Update(window, gameTime);
            playerTower.Update(window, gameTime, player.X, player.Y + ((player.Texture.Height - playerTower.Texture.Height) / 2));
            foreach (Enemy e in enemies.ToList())
            {
                if (e is SprayMine)
                {
                    foreach (Bullet b in (e as SprayMine).Bullets)
                    {
                        if (player.CheckCollision(b))
                            player.IsAlive = false;
                    }
                }
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
                    levelUp = false;
                    if (e.CheckCollision(player))
                        player.IsAlive = false;


                    e.Update(window, player.X, player.Y);
                    e.Update(window, gameTime);
                    ////if (e is ComplexEnemy)
                    ////    (e as ComplexEnemy).Update(window, player.X, player.Y);
                    ////if (e is SprayMine)
                    ////    (e as SprayMine).Update(window, gameTime);
                    //else
                    //    e.Update(window);
                }
                else
                {
                    enemies.Remove(e);
                }
            }

            if (levelUp)
            {
                currentLevel++;
                Reset(content, window);
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
            player.Reset(window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, 3f, 3f);
            playerTower.Reset(window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, 3f, 3f);

            LoadLevel(content, window);
        }

        private void Spawn(ContentManager content, GameWindow window, Texture2D sprite, int amount, string type)
        {
            Random random = new Random();

            switch (type)
            {
                case "mine":
                    for (int i = 0; i < amount; i++)
                    {
                        int rndX = random.Next(0, window.ClientBounds.Width - sprite.Width);
                        int rndY = random.Next(0, window.ClientBounds.Height / 2);

                        Mine mine = new Mine(sprite, rndX, rndY, 0f, 1f, player.X, player.Y);
                        enemies.Add(mine);
                    }
                    break;

                case "spraymine":
                    for (int i = 0; i < amount; i++)
                    {
                        int rndX = random.Next(0, window.ClientBounds.Width - sprite.Width);
                        int rndY = random.Next(0, window.ClientBounds.Height / 2);

                        SprayMine sprayMine = new SprayMine(sprite, rndX, rndY, bulletTexture, 0f, 1f);
                        enemies.Add(sprayMine);
                    }
                    break;
            }
        }

        public Player Player
        {
            get { return player; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public bool Win
        {
            get { return win; }
        }
    }
}
