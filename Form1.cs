using System;
using System.Windows.Forms;
namespace Checkers
{
    public partial class Form1 : Form
    {
        Service service;
        bool IsBeginGame;
        public Form1()
        {
            InitializeComponent();
            service = new Service();
            button_reset.Enabled = false;
            IsBeginGame = false;
            pressed = new HashSet<Keys>();//хранит зажатые клавиши
            //обработчики клавиш для движения шашек
            this.KeyPreview = true;//чтобы контроллы не перехватили управление
        }

        private void pictureBox_place_Paint(object sender, PaintEventArgs e)
        {
            textBox_countBlack.Text = service.CountBlack.ToString();
            textBox_countWhite.Text = service.CountWhite.ToString();
            Bitmap map = new Bitmap(pictureBox_place.Width, pictureBox_place.Height);
            Graphics buffer_g = Graphics.FromImage(map);
            buffer_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            service.DrawingGamePlace(buffer_g, pictureBox_place.Width, pictureBox_place.Height);
            e.Graphics.DrawImage(map, 0, 0);
        }

        private void pictureBox_current_step_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Silver);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            service.DrawingCurrentStep(e.Graphics, pictureBox_current_step.Width, pictureBox_current_step.Height);
        }

        private void button_game_start_Click(object sender, EventArgs e)
        {
            /*if (checkBox_bot.Checked)
            {
                if (whiteBottom)
                {
                    MessageBox.Show("Вы начинаете игру!");
                }
                else
                {
                    MessageBox.Show("Бот начинает игру!");
                }
            }*/
            service.ResetGamePlace();
            service.CurrentStep = 1;
            textBox_countBlack.Text = service.CountBlack.ToString();
            textBox_countWhite.Text = service.CountWhite.ToString();
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = true;
            button_game_start.Enabled = false;
            checkBox_bot.Enabled = false;
            IsBeginGame = true;
            //из-за этой кнопки, клавиши перестают регировать
            this.ActiveControl = null;//снять фокус с кнопки
            this.Focus();//возвращаем фокус форме
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            service.ResetGamePlace();
            service.CurrentStep = 0;
            textBox_countBlack.Text = "";
            textBox_countWhite.Text = "";
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = false;
            button_game_start.Enabled = true;
            checkBox_bot.Enabled = true;
            checkBox_bot.Checked = false;
            IsBeginGame = false;
            //из-за этой кнопки, клавиши перестают регировать
            this.ActiveControl = null;//снять фокус с кнопки
            this.Focus();//возвращаем фокус форме
        }

        //обработчик выбора шашки
        private void pictureBox_place_MouseClick(object sender, MouseEventArgs e)
        {
            //проверка начала игры, если игра не начaлась -> выходим
            if (!IsBeginGame)
                return;
            //сброс возможного хода, если до этого что-то выбрано было
            service.ResetPossibleStep();
            //проверка выбранного хода, можно ли ходить этой шашкой
            if (!service.CheckPossibleStep(e.Location, pictureBox_place.Width))
            {
                pictureBox_place.Invalidate();
                return;
            }

            //заполняем матрицу нужными ходами(меняем значения на временные)
            service.SetPossibleStep(e.Location, pictureBox_place.Width);

            //отдаем на отрисовку
            pictureBox_place.Invalidate();
            //из-за фокуса на пискчер боксе клавиши перестают регировать
            this.ActiveControl = null;//снять фокус с контролла
            this.Focus();//возвращаем фокус форме
        }



        //хранит нажатые клавишы
        HashSet<Keys> pressed;
        //обработчик для движения шашкой
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsBeginGame)
                return;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        if (pressed.Count() >= 0 && pressed.Count() < 2)
                        {
                            pressed.Add(Keys.Left);
                            if (pressed.Count() == 2)
                            {
                                Keys key1=Keys.Up;
                                Keys key2= Keys.Up;
                                bool flag = true;
                                foreach (var k in pressed)
                                {
                                    if (flag)
                                    {
                                        key1 = k;
                                        flag = false;
                                    }
                                    else
                                        key2 = k;
                                }
                                service.MakeMove(key1, key2);
                            }
                        }
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Right:
                    {
                        if (pressed.Count() >= 0 && pressed.Count() < 2)
                        {
                            pressed.Add(Keys.Right);
                            if (pressed.Count() == 2)
                            {
                                //пробуем сделать ход
                                Keys key1 = Keys.Up;
                                Keys key2 = Keys.Up;
                                bool flag = true;
                                foreach (var k in pressed)
                                {
                                    if (flag)
                                    {
                                        key1 = k;
                                        flag = false;
                                    }
                                    else
                                        key2 = k;
                                }
                                service.MakeMove(key1, key2);
                            }
                        }
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Up:
                    {
                        if (pressed.Count() >= 0 && pressed.Count() < 2)
                        {
                            pressed.Add(Keys.Up);
                            if (pressed.Count() == 2)
                            {
                                //пробуем сделать ход
                                Keys key1 = Keys.Up;
                                Keys key2 = Keys.Up;
                                bool flag = true;
                                foreach (var k in pressed)
                                {
                                    if (flag)
                                    {
                                        key1 = k;
                                        flag = false;
                                    }
                                    else
                                        key2 = k;
                                }
                                service.MakeMove(key1, key2);
                            }
                        }
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Down:
                    {
                        if (pressed.Count() >= 0 && pressed.Count() < 2)
                        {
                            pressed.Add(Keys.Down);
                            if (pressed.Count() == 2)
                            {
                                //пробуем сделать ход
                                Keys key1 = Keys.Up;
                                Keys key2 = Keys.Up;
                                bool flag = true;
                                foreach (var k in pressed)
                                {
                                    if (flag)
                                    {
                                        key1 = k;
                                        flag = false;
                                    }
                                    else
                                        key2 = k;
                                }
                                service.MakeMove(key1, key2);
                            }
                        }
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
            }
            //отдаем на отрисовку
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            //из-за фокуса на пискчер боксе клавиши перестают регировать
            this.ActiveControl = null;//снять фокус с контролла
            this.Focus();//возвращаем фокус форме
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsBeginGame)
                return;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        pressed.Remove(Keys.Left);
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Right:
                    {
                        pressed.Remove(Keys.Right);
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Up:
                    {
                        pressed.Remove(Keys.Up);
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;
                case Keys.Down:
                    {
                        pressed.Remove(Keys.Down);
                        e.Handled = true;//чтобы до контролла, который в фокусе ничего не дошло
                    }
                ; break;

            }
            if (pressed.Count() == 0)
            {
                service.ResetPossibleStep();
                service.CurrentStep *= -1;
            }
            //отдаем на отрисовку
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            //из-за фокуса на пискчер боксе клавиши перестают регировать
            this.ActiveControl = null;//снять фокус с контролла
            this.Focus();//возвращаем фокус форме
        }

    }
}
