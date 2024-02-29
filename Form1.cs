using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation
{
    public partial class Form1 : Form
    {
        bool up, left, down, right, shoot;
        int playerX = 0, playerY = 0, playerSize = 40, playerSpeed = 10, score = 0;


        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> enemiesToRemove = new List<Enemy>();
        List<Food> GoodStuff = new List<Food> { };
        List<Food> GoodStuffToRemove = new List<Food>();
        List<Projectile> PewPews = new List<Projectile> { };
        List<Projectile> PewToRemove = new List<Projectile>();
        int pewTimer = 2;
        int count = 0;
        private void spawn(object sender, EventArgs e)
        {
            int tempX = 0, tempY = 0;
            int maxX = this.ClientSize.Width;
            int maxY = this.ClientSize.Height;
            switch (count)
            {
                case 0:
                    tempX = 0;
                    tempY = 0;
                    break;
                case 1:
                    tempX = maxX;
                    tempY = 0;
                    break;
                case 2:
                    tempX = maxX;
                    tempY = maxY;
                    break;
                case 3:
                    tempX = 0;
                    tempY = maxY;
                    break;
                default:
                    count = 0;
                    break;
            }
            count++;
            if (count % 2 == 0)
            {
                enemies.Add(new Enemy(tempX,tempY));
            }
            GoodStuff.Add(new Food(tempX,tempY));
            if (score > 75)
            {
                float x = (playerX + maxX) / 2;
                float y = (playerY + maxY) / 2;
                if (this.ClientSize.Width - x < playerX / 2)
                {
                    x = playerX / 2;
                }
                if (this.ClientSize.Height - y < playerY / 2)
                {
                    y = playerY / 2;
                }
                enemies.Add(new Enemy(x, y));
                if (score > 200)
                {
                    if (score > 600)
                    {
                        if (count <= 1) { enemies.Add(new Enemy(0, 0).setSize(128)); }
                        else { enemies.Add(new Enemy(maxX, maxY).setSize(128)); }
                    }
                    else
                    {
                        enemies.Add(new Enemy(0, 0));
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            playerX = (this.ClientSize.Width-playerSize)/2;
            playerY = (this.ClientSize.Height-playerSize)/2;
        }
        private void PaintEvent(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillEllipse(Brushes.Green, new Rectangle(playerX, playerY, playerSize, playerSize));
            foreach (Enemy enemy in enemies)
            {
                g.FillEllipse(enemy.colour, new RectangleF(enemy.x, enemy.y, enemy.size, enemy.size));
            }
            foreach (Food goodie in GoodStuff)
            {
                g.FillEllipse(goodie.colour, new RectangleF(goodie.x, goodie.y, goodie.size, goodie.size));
            }
            foreach (Projectile projectile in PewPews)
            {
                g.FillEllipse(projectile.colour, new RectangleF(projectile.x, projectile.y, projectile.size, projectile.size));
            }
        }
        int cooldown = 0;
        private void moveTimer_Tick(object sender, EventArgs e)
        {
            if (left && playerX>0)
            {
                playerX -= playerSpeed;
            }
            if (right && playerX + playerSize < this.ClientSize.Width)
            {
                playerX += playerSpeed;
            }
            if (up && playerY > 0)
            {
                playerY -= playerSpeed;
            }
            if (down && playerY + playerSize < this.ClientSize.Height)
            {
                playerY += playerSpeed;
            }
            if (shoot && cooldown <= 0)
            {
                PewPews.Add(new Projectile(playerX, playerY));
                cooldown = pewTimer;
            }
            if (cooldown > 0)
            {
                cooldown--;
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.move(playerX, playerY, playerSize))
                {
                    if (score > 100)
                    {
                        score -= Convert.ToInt32(enemy.size.ToString().Split('.')[0]);
                    }
                    else
                    {
                        score--;
                    }
                    if (playerSize > 30) { playerSize-=5; }
                    else if (playerSize > 15) { playerSize--; }
                    enemiesToRemove.Add(enemy);
                }
            }
            foreach (Food food in GoodStuff)
            {
                food.move(this.ClientSize.Width, this.ClientSize.Height);
                if (food.checkCollision(playerX, playerY, playerSize)) 
                {
                    GoodStuffToRemove.Add(food);
                    if (playerSize < 50) {  playerSize += 5; }
                    else if (playerSize <100) { playerSize += 2; }
                }
            }
            foreach (Projectile pew in PewPews)
            {
                Enemy hit = pew.target(enemies);
                if (hit != null)
                {
                    enemiesToRemove.Add(hit);
                    PewToRemove.Add(pew);
                    if (hit.size > 32)
                    {
                        enemies.Add(new Enemy(hit.x + 10, hit.y + 10).halfSize(hit));
                        enemies.Add(new Enemy(hit.x - 10, hit.y - 10).halfSize(hit));
                    }
                    else
                    {
                        score++;
                    }
                }
            }
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
            }
            enemiesToRemove.Clear();
            foreach (Food food in GoodStuffToRemove)
            {
                GoodStuff.Remove(food);
            }
            GoodStuffToRemove.Clear();
            foreach (Projectile pew in PewToRemove)
            {
                PewPews.Remove(pew);
            }
            PewToRemove.Clear();
            this.Invalidate();
            txtScore.Text = score.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                up = true;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                down = true;
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                left = true;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                right = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                shoot = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                up = false;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                down = false;
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                left = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                right = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                shoot = false;
            }
        }
        
    }
    public class Enemy
    {
        public float x = 0, y = 0, size = 64, speed = 5;
        public Brush colour = Brushes.Red;
        public Enemy(float newX, float newY)
        {
            x = newX;
            y = newY;
        }
        public Enemy setSize(int s)
        {
            size = s;
            speed = 320 / size;
            return this;
        }
        public Enemy halfSize(Enemy prev)
        {
            size = prev.size * 0.5f;
            speed = 320 / size;
            return this;
        }
        public bool move(int playerX, int playerY, int playerSize)
        {
            float diffX = (playerX+playerSize/2) - (x+size/2);
            float diffY = (y+size/2) - (playerY+playerSize/2);
            float magnitude = (float)Math.Sqrt(diffX * diffX + diffY * diffY);
            if (magnitude > playerSize/2)
            {
                diffX /= magnitude;
                diffY /= magnitude;
                x += diffX * speed;
                y -= diffY * speed;
                return false;
            }
            else
            {               //Collision
                size = 0;
                return true;
            }   
        }
    }
    public class Food
    {
        public float x = 0, y = 0, size = 30, speedX = 5, speedY = 5;
        private bool up, left;
        public Brush colour = Brushes.Blue;
        int ttl = 800, life = 0;
        public Food(int newX, int newY)
        {
            x = newX;
            y = newY;
        }
        public void move(int maxWidth, int maxHeight)
        {
            speedY = maxHeight * 5 / maxWidth;
            if (ttl > life)
            {
                if (x <= 10)
                {
                    left = false;
                }
                else if (x >= maxWidth - 10)
                {
                    left = true;
                }
                if (y <= 10)
                {
                    up = false;
                }
                else if (y >= maxHeight - 10)
                {
                    up = true;
                }
                if (left) { x -= speedX; }
                else { x += speedX; }
                if (up) { y -= speedY; }
                else { y += speedY; }
                life++;
            }
            else { size = 0; }
        }
        public bool checkCollision(int playerX, int playerY, int playerSize)
        {
            float diffX = (playerX + playerSize / 2) - (x + size / 2);
            float diffY = (y + size / 2) - (playerY + playerSize / 2);
            float magnitude = (float)Math.Sqrt(diffX * diffX + diffY * diffY);
            return magnitude < playerSize / 2;
        }
    }
    public class Projectile
    {
        public float x = 0, y = 0, size = 20, speed = 30;
        public Brush colour = Brushes.Purple;
        public Projectile(float newX, float newY)
        {
            x = newX;
            y = newY;
        }
        public Enemy target(List<Enemy> enemies)
        {
            Enemy selected = new Enemy(0,0);
            float prevMag = 999;
            float finalDiffX = 0;
            float finalDiffY = 0;
            foreach (Enemy enemy in enemies)
            {
                float diffX = (enemy.x + enemy.size / 2) - (x + size / 2);
                float diffY = (y + size / 2) - (enemy.y + enemy.size / 2);
                float magnitude = (float)Math.Sqrt(diffX * diffX + diffY * diffY);
                if (magnitude < prevMag)
                {
                    selected = enemy;
                    prevMag = magnitude;
                    finalDiffX = diffX;
                    finalDiffY = diffY;
                }
            }
            if (prevMag > selected.size / 2)
            {
                finalDiffX /= prevMag;
                finalDiffY /= prevMag;
                x += finalDiffX * speed;
                y -= finalDiffY * speed;
                return null;
            }
            else
            {
                size = 0;
                speed = 0;
                return selected;
            }
        }
    }

}
//Code projectile movement & spawning when space is pressed. Remove projectile & enemy when overlap.
