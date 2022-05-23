using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sapper
{
    internal class Game
    {
        private int width;
        private int height;
        private int mines_number;
        private int cell_size;
        private int[,] map;
        private int[,] flags;
        private bool[,] pressed;
        private Button[,] buttons;
        private Form form;
        private Form main_form;
        private Image sprites;
        private bool game_beginning;
        private Point first_click;
        public void Start(int n, int m, int mines, Form form, Form main_form)
        {
            height = n;
            width = m;
            cell_size = 50;
            mines_number = mines;
            this.form = form;
            this.main_form = main_form;
            map = new int[height, width];
            flags = new int[height, width];
            pressed = new bool[height, width];
            buttons = new Button[height, width];
            string c = new DirectoryInfo(Directory.GetCurrentDirectory()).FullName.ToString();

            sprites = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName.ToString(), @"Sprites\cells.png"));
            game_beginning = true;
            SetFormSize();
            CreateButtons();
        }
        private void SetFormSize()
        {
            form.Width = width * cell_size + cell_size / 2 - 7;
            form.Height = height * cell_size + cell_size - 2;
        }
        private void GenerateMines()
        {
            Random rand = new Random();
            for (int i = 0; i < mines_number; i++)
            {
                int x = rand.Next(0, width - 1);
                int y = rand.Next(0, height - 1);
                while (map[y, x] == -1 || Math.Abs(x - first_click.X) <= 1 && Math.Abs(y - first_click.Y) <= 1)
                {
                    x = rand.Next(0, width - 1);
                    y = rand.Next(0, height - 1);
                }
                map[y, x] = -1;
            }
        }
        private void CreateButtons()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * cell_size, i * cell_size);
                    button.Size = new Size(cell_size, cell_size);
                    button.Image = GetImage(1, 4);
                    button.MouseDown += new MouseEventHandler(PressButton);
                    form.Controls.Add(button);
                    buttons[i, j] = button;
                }
            }
        }
        private void PressButton(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button.ToString() == "Left")
            {
                LeftButton(button);
            }
            else if (e.Button.ToString() == "Right")
            {
                RightButton(button);
            }
        }
        private void LeftButton(Button button)
        {
            int mouse_x = button.Location.X / cell_size;
            int mouse_y = button.Location.Y / cell_size;
            if (!pressed[mouse_y, mouse_x])
            {
                pressed[mouse_y, mouse_x] = true;
                if (game_beginning)
                {
                    first_click = new Point(mouse_x, mouse_y);
                    GenerateMines();
                    CountMines();
                    game_beginning = false;
                }
                OpenMap(mouse_x, mouse_y);
                if (map[mouse_y, mouse_x] == -1)
                {
                    OpenMines(mouse_x, mouse_y, GetImage(2, 3));
                    if (MessageBox.Show("Поражение! Запустить новую игру?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        form.Close();
                        main_form.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                if (Victory())
                {
                    OpenMines(mouse_x, mouse_y, GetImage(2, 1));
                    if (MessageBox.Show("Победа! Запустить новую игру?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        form.Close();
                        main_form.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }
        private void RightButton(Button button)
        {
            int j = button.Location.X / cell_size;
            int i = button.Location.Y / cell_size;
            if(!pressed[i, j])
            {
                if (flags[i, j] == 0)
                {
                    buttons[i, j].Image = GetImage(2, 0);
                    flags[i, j] = 10;
                }
                else if (flags[i, j] == 10)
                {
                    buttons[i, j].Image = GetImage(2, 2);
                    flags[i, j] = 11;
                }
                else if (flags[i, j] == 11)
                {
                    buttons[i, j].Image = GetImage(1, 4);
                    flags[i, j] = 0;
                }
            }
        }
        private void CountMines()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i, j] == -1)
                    {
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (k >= 0 && k < height && l >= 0 && l < width && map[k, l] != -1)
                                {
                                    map[k, l]++;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void OpenMines(int x, int y, Image image)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i, j] == -1)
                    {
                        buttons[i, j].Image = image;
                    }
                }
            }
        }
        private void OpenCell(int x, int y)
        {
            pressed[y, x] = true;
            if (map[y, x] == -1)
            {
                buttons[y, x].Image = GetImage(2, 1);
            }
            else if (map[y, x] == 0)
            {
                buttons[y, x].Image = GetImage(0, 0);
            }
            else if (map[y, x] == 1)
            {
                buttons[y, x].Image = GetImage(0, 1);
            }
            else if (map[y, x] == 2)
            {
                buttons[y, x].Image = GetImage(0, 2);
            }
            else if (map[y, x] == 3)
            {
                buttons[y, x].Image = GetImage(0, 3);
            }
            else if (map[y, x] == 4)
            {
                buttons[y, x].Image = GetImage(0, 4);
            }
            else if (map[y, x] == 5)
            {
                buttons[y, x].Image = GetImage(1, 0);
            }
            else if (map[y, x] == 6)
            {
                buttons[y, x].Image = GetImage(1, 1);
            }
            else if (map[y, x] == 7)
            {
                buttons[y, x].Image = GetImage(1, 2);
            }
            else if (map[y, x] == 8)
            {
                buttons[y, x].Image = GetImage(1, 3);
            }
        }
        private void OpenMap(int x, int y)
        {
            OpenCell(x, y);
            if (map[y, x] == 0)
            {
                for (int k = y - 1; k < y + 2; k++)
                {
                    for (int l = x - 1; l < x + 2; l++)
                    {
                        if (k >= 0 && k < height && l >= 0 && l < width && !pressed[k, l])
                        {
                            if (map[k, l] == 0)
                            {
                                OpenMap(l, k);
                            }
                            else if (map[k, l] > 0)
                            {
                                OpenCell(l, k);
                            }
                        }
                    }
                }
            }
        }
        private Image GetImage(int i, int j)
        {
            Image image = new Bitmap(cell_size, cell_size);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(sprites, new Rectangle(new Point(0, 0), new Size(cell_size, cell_size)), j * 32, i * 32, 33, 33, GraphicsUnit.Pixel);
            return image;
        }
        private bool Victory()
        {
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    if(!(pressed[i, j] || map[i, j] == -1))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}