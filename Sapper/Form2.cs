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
    public partial class Form2 : Form
    {
        Game game;
        Form main_form;
        public Form2(int height, int width, int mines_number, Form main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
            game = new Game();
            game.Start(height, width, mines_number, this, main_form);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                main_form.Show();
            }
        }
    }
}
