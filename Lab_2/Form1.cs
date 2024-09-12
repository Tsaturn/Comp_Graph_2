using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2
{
    public partial class Form1 : Form
    {
        protected Form task1, task2, task3;

        public Form1()
        { 
            InitializeComponent();
            task1 = new Task1();
            task1.Hide();

            task2 = new Task2();
            task2.Hide();

            task3 = new Task3();
            task3.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            task1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            task2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            task3.ShowDialog();
        }
    }
}
