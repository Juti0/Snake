using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Snake
{
    public partial class Snake : Form
    {

        private List<Circle> Snakes = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;




        public Snake()
        {
            InitializeComponent();

            new Settings();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.direction != "right")
            {

                goLeft = true;
             
            }
            if (e.KeyCode == Keys.Right && Settings.direction != "left")
            {
                goRight = true;

            }
            if (e.KeyCode == Keys.Up && Settings.direction != "Down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.direction != "Up")
            {
                goDown = true;
            }

        }

        private void KeyIsTop(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {

                goLeft = false;

            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;

            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();

        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored " + score + "and my Hightscore is" + highScore + "on the Snake Game from juti";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.LightBlue;
            caption.AutoSize = false;
            caption.Height = picCanvas.Width;
            caption.Width = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot Juti";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog()==DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if(goLeft)
            {
                Settings.direction = "left";
            }
            if(goRight)
            {
                Settings.direction = "right";
            }
            if (goDown)
            {
                Settings.direction = "down";
            }
            if (goUp)
            {
                Settings.direction = "up";
            }

            for (int i = Snakes.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch(Settings.direction)
                    {
                        case "left":
                            Snakes[i].X--;
                            break;
                        case "right":
                            Snakes[i].X++;
                            break;
                        case "down":
                            Snakes[i].Y++;
                            break;
                        case "up":
                            Snakes[i].Y--;
                            break;
                    }

                    if (Snakes[i].X < 0)
                    {
                        Snakes[i].X = maxWidth;
                    }
                    if (Snakes[i].X > maxWidth)
                    {
                        Snakes[i].X = 0;
                    }
                    if (Snakes[i].Y < 0)
                    {
                        Snakes[i].Y = maxHeight;
                    }
                    if (Snakes[i].Y > maxHeight)
                    {
                        Snakes[i].Y = 0;
                    }
                    if(Snakes[i].X==food.X&&Snakes[i].Y==food.Y)
                    {
                        EatFood();
                    }
                    for (int j = 1; j < Snakes.Count; j++)
                    {
                        if (Snakes[i].X == Snakes[j].X && Snakes[i].Y == Snakes[j].Y)
                        {
                            GameOver(); 
                        }
                    }
                }
                else
                {
                    Snakes[i].X = Snakes[i - 1].X;
                    Snakes[i].Y = Snakes[i - 1].Y;
                }

            }
            picCanvas.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColour;
            for(int i=0;i<Snakes.Count;i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }
                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snakes[i].X * Settings.Width,
                    Snakes[i].Y * Settings.Height,
                    Settings.Width, Settings.Height

                    )) ;
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                    (
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width, Settings.Height

                    ));
        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snakes.Clear();

            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snakes.Add(head);
            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snakes.Add(body);
            }
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;

            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snakes[Snakes.Count - 1].X,
                Y = Snakes[Snakes.Count - 1].Y
            };

            Snakes.Add(body);
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }

        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if (score > highScore)
            {
                highScore = score;

                txtHighScore.Text = "Hight Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
