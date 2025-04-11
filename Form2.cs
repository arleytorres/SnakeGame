using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        int sumir = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1.00)
            {
                if (sumir >= 20)
                {
                    this.Close();
                }
                else
                {
                    sumir++;
                }
            }
            else
            {
                this.Opacity += 0.05;
            }
        }
    }
}
