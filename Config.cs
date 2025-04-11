using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public enum Direcao
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class Config
    {
        public static int largura { get; set; }
        public static int altura { get; set; }
        public static double velocidade { get; set; }
        public static int score { get; set; }
        public static bool GameOver { get; set; }
        public static Direcao direcao { get; set; }

        public Config()
        {
            largura = 16;
            altura = 16;
            velocidade = 1;
            score = 0;
            GameOver = false;
            direcao = Direcao.Down;
        }
    }
}
