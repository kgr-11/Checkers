using System;
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
        }

        private void pictureBox_place_Paint(object sender, PaintEventArgs e)
        {

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
            IsBeginGame= false;
        }

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
        }
    }
}
