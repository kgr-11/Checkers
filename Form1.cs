using System;
namespace Checkers
{
    public partial class Form1 : Form
    {
        Service service;
        bool whiteBottom;
        public Form1()
        {
            InitializeComponent();
            whiteBottom = true;//белые всегда начинают снизу 
            service = new Service(whiteBottom);
            button_reset.Enabled = false;
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
            //генерируем, где сто€т белые-сверху/снизу
            Random random = new Random();
            whiteBottom = random.Next(2) == 1;//1 - значит белые снизу -> true
            if (checkBox_bot.Checked)
            {
                if (whiteBottom)
                {
                    MessageBox.Show("¬ы начинаете игру!");
                }
                else
                {
                    MessageBox.Show("Ѕот начинает игру!");
                }
            }
            service.ResetGamePlace(whiteBottom);
            service.CurrentStep = 1;
            textBox_countBlack.Text = service.CountBlack.ToString();
            textBox_countWhite.Text = service.CountWhite.ToString();
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = true;
            button_game_start.Enabled = false;
            checkBox_bot.Enabled = false;
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            whiteBottom = true;
            service.ResetGamePlace(whiteBottom);
            service.CurrentStep = 0;
            textBox_countBlack.Text = "";
            textBox_countWhite.Text = "";
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = false;
            button_game_start.Enabled = true;
            checkBox_bot.Enabled = true;
            checkBox_bot.Checked = false;
        }

        private void pictureBox_place_MouseClick(object sender, MouseEventArgs e)
        {
            service.CoordCursorInNumberCell(e.Location, pictureBox_place.Width, out int i,out int j);
        }
    }
}
