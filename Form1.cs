using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circulo> Snake = new List<Circulo>();
        private Circulo comida = new Circulo();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            Form2 form = new Form2();
            form.Visible = true;
            form.FormClosed += Form_FormClosed;
        }

        bool jogou = false;

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Focus();
            new Config();

            Config.GameOver = true;
            timer1.Interval = Convert.ToInt32(150 / Config.velocidade);
            timer1.Start();

            if (!Config.GameOver)
            {
                StartGame();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Config.GameOver)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            jogou = true;
            new Config();

            button1.Visible = false;
            this.Focus();
            pictureBox2.Visible = false;
            Snake.Clear();

            Circulo cabeça = new Circulo();
            cabeça.X = 10;
            cabeça.Y = 5;
            Snake.Add(cabeça);

            cabeça = new Circulo();
            cabeça.X = 10;
            cabeça.Y = 5;
            Snake.Add(cabeça);

            label1.Text = "Score: " + Config.score.ToString() + "  |  Velocidade: " + Config.velocidade;
            label1.Visible = true;
            label2.Visible = false;
            GerarComida();
            cobra.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }

        bool especial = false;

        private void GerarComida()
        {
            int maxPosX = pictureBox1.Width/Config.largura;
            int maxPosY = pictureBox1.Height / Config.altura;

            Random escolha = new Random();

            comida = new Circulo();
            comida.X = escolha.Next(0, maxPosX);
            comida.Y = escolha.Next(0, maxPosY);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
            cobra = Properties.Resources.cobra;

            if (Input.KeyPressed(Keys.Right) && Config.direcao != Direcao.Left)
            {
                Config.direcao = Direcao.Right;
                cobra.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (Input.KeyPressed(Keys.Left) && Config.direcao != Direcao.Right)
            {
                Config.direcao = Direcao.Left;
                cobra.RotateFlip(RotateFlipType.Rotate180FlipNone);
                cobra.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (Input.KeyPressed(Keys.Up) && Config.direcao != Direcao.Down)
            {
                Config.direcao = Direcao.Up;
            }
            else if (Input.KeyPressed(Keys.Down) && Config.direcao != Direcao.Up)
            {
                Config.direcao = Direcao.Down;
                cobra.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {

            if (Config.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                timer1.Interval = Convert.ToInt32(150 / Config.velocidade);
                MovePlayer();
                pictureBox1.Invalidate();
            }
        }

        Image cobra = Properties.Resources.cobra;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!Config.GameOver)
            {
                Graphics canvas = e.Graphics;


                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColor = Brushes.Black;
                    Brush foodColor = Brushes.Gray;

                    if (especial)
                    {
                        foodColor = Brushes.Yellow;
                    }


                    if (i == 0)
                    {
                        canvas.DrawImage(cobra, new Rectangle(Snake[i].X * Config.largura, Snake[i].Y * Config.altura, Config.largura, Config.altura));
                    }
                    else
                    {
                        canvas.FillRectangle(snakeColor, new Rectangle(Snake[i].X * Config.largura, Snake[i].Y * Config.altura, Config.largura, Config.altura));
                    }

                    canvas.FillEllipse(foodColor, new Rectangle(comida.X * Config.largura, comida.Y * Config.altura, Config.largura, Config.altura));
                }
            }
            else
            {
                string gameOver;
                if (jogou)
                {
                    gameOver = "Fim de Jogo \nSeu recorde durante a partida foi de: " + Config.score + "\nPressione Enter para recomeçar!";
                }
                else
                {
                    gameOver = "Pressione Enter para começar";
                }

                label2.Text = gameOver;
                label2.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count -1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Config.direcao)
                    {
                        case Direcao.Up:
                            Snake[i].Y--;
                            break;
                        case Direcao.Down:
                            Snake[i].Y++;
                            break;
                        case Direcao.Left:
                            Snake[i].X--;
                            break;
                        case Direcao.Right:
                            Snake[i].X++;
                            break;
                    }

                    int maxPosX = pictureBox1.Width / Config.largura;
                    int maxPosY = pictureBox1.Height / Config.altura;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxPosX || Snake[i].Y >= maxPosY)
                    {
                        Morrer();
                    }

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Morrer();
                        }
                    }

                    if (Snake[i].X == comida.X && Snake[i].Y == comida.Y)
                    {
                        Comer();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        SoundPlayer efeitoSonoro = new SoundPlayer();

        private void Comer()
        {
            efeitoSonoro = new SoundPlayer(Properties.Resources.eat);
            efeitoSonoro.Play();

            Circulo comida = new Circulo();
            comida.X = Snake[Snake.Count - 1].X;
            comida.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(comida);

            Config.score += 10;
            label1.Text = "Score: " + Config.score + "  |  Velocidade: " + Config.velocidade;

            if (especial)
            {
                Circulo comida2 = new Circulo();
                comida2.X = Snake[Snake.Count - 1].X;
                comida2.Y = Snake[Snake.Count - 1].Y;

                Snake.Add(comida2);
                especial = false;
            }

            Random escolha = new Random();

            if (escolha.Next(0, 100) >= 80)
            {
                especial = true;
            }

            GerarComida();

            if (Snake.Count % 3 == 0)
            {
                Config.velocidade += 0.02;
            }
        }

        private void Morrer()
        {
            efeitoSonoro = new SoundPlayer(Properties.Resources.fail);
            efeitoSonoro.Play();
            pictureBox2.Visible = true;
            label1.Visible = false;
            Config.GameOver = true;
            button1.Visible = true;
        }
    }
}
