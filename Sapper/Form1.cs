using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sapper
{
    public partial class Form1 : Form
    {
        Form2 form;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            form = new Form2(9, 9, 10, this);
            form.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            form = new Form2(16, 16, 40, this);
            form.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            form = new Form2(16, 30, 99, this);
            form.ShowDialog();
        }
    }
}
