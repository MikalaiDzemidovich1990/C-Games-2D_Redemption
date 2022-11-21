using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace _2D_Redemption
{
    public partial class Form1 : Form
    {
        PictureBox[] cloud;
        int backgroundspeed;
        Random rnd;
        int playerSpeed;

        PictureBox[] bullets;
        int bulletsSpeed;

        PictureBox[] enemies;
        int sizeEnemy;
        int enemiesSpeed;

        WindowsMediaPlayer Shoot;
        WindowsMediaPlayer GameSong;


        public Form1()
        {
            InitializeComponent();
        }
        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < cloud.Length; i++)
            {
                cloud[i].Left += backgroundspeed;

                if(cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Height;
                }
            }
            for (int i = cloud.Length; i < cloud.Length; i++)
            {
                cloud[i].Left += backgroundspeed - 10;

                if (cloud[i].Left >= 1280)
                {
                    cloud[i].Left = cloud[i].Left;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundspeed = 5;
            cloud = new PictureBox[20];
            rnd = new Random();
            playerSpeed = 3;

            bullets = new PictureBox[1];
            bulletsSpeed = 80;

            enemies = new PictureBox[4];
            int sizeEnemy = rnd.Next(60, 80);
            enemiesSpeed = 3;

            Image easyEnemies = Image.FromFile("assets\\apap.gif");

            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BackColor = Color.Transparent;
                enemies[i].Image = easyEnemies;
                enemies[i].Location = new Point((i + 1) * rnd.Next(90, 160) + 1080, rnd.Next(450, 600));

                this.Controls.Add(enemies[i]);
            }

            Shoot = new WindowsMediaPlayer();
            Shoot.URL = "songs\\shoot.mp3";
            Shoot.settings.volume = 5;

            GameSong = new WindowsMediaPlayer();
            GameSong.URL = "songs\\GameSong.mp3";
            GameSong.settings.setMode("loop", true);
            GameSong.settings.volume = 15;



            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new PictureBox();
                bullets[i].BorderStyle = BorderStyle.None;
                bullets[i].Size = new Size(20, 5);
                bullets[i].BackColor = Color.White;

                this.Controls.Add(bullets[i]);
            }

            for (int i = 0; i < cloud.Length; i++)
            {
                cloud[i] = new PictureBox();
                cloud[i].BorderStyle = BorderStyle.None;
                cloud[i].Location = new Point(rnd.Next(-1000, 200), rnd.Next(140, 320));
                if(i%2 == 1)
                {
                    cloud[i].Size = new Size(rnd.Next(100, 255), rnd.Next(30, 70));
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 25, 200, 200);
                }
                else
                {
                    cloud[i].Size = new Size(150,25);
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 25, 205, 205);

                }
                this.Controls.Add(cloud[i]);
            }
            GameSong.controls.play();
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if(mainPlayer.Left > 10)
            {
                mainPlayer.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left < 1150)
            {
                mainPlayer.Left += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            mainPlayer.Top -= playerSpeed;
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            mainPlayer.Top += playerSpeed;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_run;
            if(e.KeyCode == Keys.Left)
            {
                LeftMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Right)
            {
                RightMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Up)
            {
                UpMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Down)
            {
                DownMoveTimer.Start();
            }
            if(e.KeyCode ==Keys.Space)
            {
                Shoot.controls.play();
                for(int i = 0; i < bullets.Length; i++)
                {
                    Intersect();
                    if(bullets[i].Left > 1280)
                    {
                        bullets[i].Location = new Point(mainPlayer.Location.X + 100 + i * 50, mainPlayer.Location.Y + 50);
                    }
                }
            }    
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_idble;
            LeftMoveTimer.Stop();
            RightMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }

        private void MoveBulletsTimer_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Left += bulletsSpeed;
            }
        }

        private void MoveEnemiesTImer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiesSpeed);
        }
        private void MoveEnemies(PictureBox[] enemies, int speed)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Left -= speed + (int)(Math.Sin(enemies[i].Left * Math.PI / 180) + Math.Cos(enemies[i].Left * Math.PI / 100));

                Intersect();

                if (enemies[i].Left < this.Left)
                {
                    int sizeEnemy = rnd.Next(60, 90);
                    enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                    enemies[i].Location = new Point((i + 1) * rnd.Next(150, 250) + 1080, rnd.Next(450, 650));
                }
            }
         
        }
        private void Intersect()
        {
            for(int i = 0; i < enemies.Length; i++ )
            {
                if (bullets[0].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    enemies[i].Location = new Point((i+1)*rnd.Next(150,250)+1280, rnd.Next(420,600));
                    bullets[0].Location = new Point(2000, mainPlayer.Location.Y + 50);


                }
                if(mainPlayer.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    mainPlayer.Visible = false;
                }
            }
        }
    }
}
