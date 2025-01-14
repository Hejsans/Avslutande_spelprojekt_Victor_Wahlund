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
        // Medlemsvariabler
        private int currentLevel;
        private Player player;
        private Tower playerTower;
        private List<Enemy> enemies;
        private bool levelUp, win;
        private Texture2D bulletTexture;

        public LevelHandler(ContentManager content, GameWindow window)    // Konstruktor
        {
            // Sätter currentLevel till första banan och skapar en lista med fiender
            currentLevel = 1;
            enemies = new List<Enemy>();

            // Laddar in texturer till bullet och spelare
            bulletTexture = content.Load<Texture2D>("images/bullet");
            player = new Player(content.Load<Texture2D>("images/player/chassi"), window.ClientBounds.Width/2, window.ClientBounds.Height * 0.75f, 3f, 3f, 0f, 0.05f);
            playerTower = new Tower(content.Load<Texture2D>("images/player/tower"), window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, true, 3f, 3f, bulletTexture, 0, 0.08f);
        }

        public void LoadLevel(ContentManager content, GameWindow window)   // Laddar in en bana
        {
            // Tömmer listan enemies och ser till så man inte har vunnit
            enemies.Clear();
            win = false;

            // Laddar in fiendernas texturer
            Texture2D mineSprite = content.Load<Texture2D>("images/enemy/mine");
            Texture2D sprayMineSprite = content.Load<Texture2D>("images/enemy/spraymine");

            // Skapar de fiender som ska finnas beroende på vilken bana man är på
            switch(currentLevel)
            {
                case 1:  // Level 1
                    Spawn(content, window, mineSprite, 5, "mine");   // 5 "mine" fiender
                    break;

                case 2:  // Level 2
                    Spawn(content, window, sprayMineSprite, 5, "spraymine");   // 5 "spraymine" fiender
                    break;

                case 3:  // Level 3
                    Spawn(content, window, sprayMineSprite, 2, "spraymine");  // 2 "spraymine" fiender
                    Spawn(content, window, mineSprite, 3, "mine");  // 3 "mine" fiender
                    break;

                default:  // När alla levels är färdiga och man har klarat spelet
                    win = true;  // Spelaren vinner
                    break;
            }
        }

        public void Update(ContentManager content, GameWindow window, GameTime gameTime)
        {
            // Sätter levelUp till true, den kommer bli false om det finns en levande fiende kvar
            levelUp = true;

            // Uppdaterar spelaren
            player.Update(window, gameTime);
            playerTower.Update(window, gameTime, player.X, player.Y + ((player.Texture.Height - playerTower.Texture.Height) / 2));

            // Går igenom alla fiender i listan
            foreach (Enemy e in enemies.ToList())
            {
                // Kollar om spelaren har kolliderat med någon av spraymines skott och dödar spelaren om det har hänt
                if (e is SprayMine)
                {
                    foreach (Bullet b in (e as SprayMine).Bullets)
                    {
                        if (player.CheckCollision(b))
                            player.IsAlive = false;
                    }
                }
                
                // Kollar om någon av spelarens skott har kolliderat med en fiende. Dödar fienden och tar bort skottet om de har kolliderat samt ger spelaren ett poäng
                foreach (Bullet b in playerTower.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        b.IsAlive = false;
                        player.Points++;
                    }
                }

                // Kollar om fienden e lever
                if (e.IsAlive)
                {
                    // Gör så man inte kan levla upp om en fiende lever
                    levelUp = false;

                    // Dödar spelaren om den har kolliderat med en fiende
                    if (e.CheckCollision(player))
                        player.IsAlive = false;

                    // Uppdaterar fienden e (Enemy använder polymorfism och har två olika Update-metoder) 
                    e.Update(window, player.X, player.Y);
                    e.Update(window, gameTime);
                }
                else
                {
                    enemies.Remove(e);   // Tar bort fienden från listan om den inte lever
                }
            }

            // Ökar currentLevel och kallar Reset() om man kan levla upp (om alla fiender är döda)
            if (levelUp)
            {
                currentLevel++;
                Reset(content, window);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Ritar ut spelaren 
            player.Draw(spriteBatch);
            playerTower.Draw(spriteBatch);

            // Ritar ut alla fiender
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }

        public void Reset(ContentManager content, GameWindow window)   // Återställer spelet
        {
            // Återställer spelaren
            player.Reset(window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, 3f, 3f);
            playerTower.Reset(window.ClientBounds.Width / 2, window.ClientBounds.Height * 0.75f, 3f, 3f);

            // Laddar in en bana
            LoadLevel(content, window);
        }

        /// <summary>
        /// Metod för att lättare skapa fiender vid varje bana 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="window"></param>
        /// <param name="sprite"> Fiendens sprite </param>
        /// <param name="amount"> Hur många av fienden som ska skapas </param>
        /// <param name="type"> Vilken sorts fiende som ska skapas </param>
        private void Spawn(ContentManager content, GameWindow window, Texture2D sprite, int amount, string type)  
        {
            Random random = new Random();

            // Kollar vilken sorts fiende som ska skapas
            switch (type)
            {
                case "mine":  // för att skapa fienden "mine"

                    for (int i = 0; i < amount; i++)
                    {
                        // Placerar fienden på en slumpmässig position i den övre halvan av skärmen
                        int rndX = random.Next(0, window.ClientBounds.Width - sprite.Width);
                        int rndY = random.Next(0, window.ClientBounds.Height / 2);

                        // Skapar fienden och lägger till de i listan enemies
                        Mine mine = new Mine(sprite, rndX, rndY, 0f, 1f, player.X, player.Y);
                        enemies.Add(mine);
                    }
                    break;

                case "spraymine":   // För att skapa fienden "spraymine"
                    for (int i = 0; i < amount; i++)
                    {
                        // Placerar fienden på en slumpmässig position i den övre halvan av skärmen
                        int rndX = random.Next(0, window.ClientBounds.Width - sprite.Width);
                        int rndY = random.Next(0, window.ClientBounds.Height / 2);

                        // Skapar fienden och lägger till de i listan enemies
                        SprayMine sprayMine = new SprayMine(sprite, rndX, rndY, bulletTexture, 0f, 1f);
                        enemies.Add(sprayMine);
                    }
                    break;
            }
        }

        // Egenskaper

        public Player Player   // Finns för att GameElements ska kunne se hur många poäng spelaren har
        {
            get { return player; }   
        }

        public int CurrentLevel    // Finns för att GameElements ska kunne se vilken bana man är på och ändra den
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public bool Win    // Finns för att GameElements ska kunne se om man har vunnit
        {
            get { return win; }
        }
    }
}
