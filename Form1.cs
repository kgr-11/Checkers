using Checkers.GA;
using System;
using System.Reflection;
using System.Windows.Forms;
namespace Checkers
{
    enum WhoseStep 
    {
        Human,
        Bot,
        None
    }
    public partial class Form1 : Form
    {
        Service service;
        bool IsBeginGame;
        WhoseStep step;
        Computer computer;
        public Form1()
        {
            InitializeComponent();
            service = new Service();
            computer = new Computer();
            //computer.LearningNetwork();
            button_reset.Enabled = false;
            IsBeginGame = false;
            step = WhoseStep.None;
            buffer = null;
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
            service.ResetGamePlace();
            textBox_countBlack.Text = service.CountBlack.ToString();
            textBox_countWhite.Text = service.CountWhite.ToString();
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = true;
            button_game_start.Enabled = false;
            checkBox_bot.Enabled = false;
            IsBeginGame = true;
            if (checkBox_bot.Checked)
            {
                if (Random.Shared.Next(0, 2) == 0)// если 0, игру начинает человек, если 1 - бот
                {
                    MessageBox.Show("Вы начинаете игру!");
                    step = WhoseStep.Human;
                }
                else
                {
                    MessageBox.Show("Бот начинает игру!");
                    step = WhoseStep.Bot;
                    StepComputer();
                }
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            service.ResetGamePlace();
            service.current_step= CurrentStep.None;
            textBox_countBlack.Text = "";
            textBox_countWhite.Text = "";
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
            button_reset.Enabled = false;
            button_game_start.Enabled = true;
            checkBox_bot.Enabled = true;
            checkBox_bot.Checked = false;
            IsBeginGame = false;
            step = WhoseStep.None;
            buffer = null;
        }

        Point? buffer;
        private void pictureBox_place_MouseClick(object sender, MouseEventArgs e)
        {
            //проверка начала игры, если игра не начaлась -> выходим
            if (!IsBeginGame||(checkBox_bot.Checked && step != WhoseStep.Human))
                return;
            if (buffer == null||!service.IsMakeMove(e.Location, pictureBox_place.Width))
            {
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
                buffer = e.Location;//запомнили прошлое нажатие
                //отдаем на отрисовку
                pictureBox_place.Invalidate();
                pictureBox_current_step.Invalidate();
            }
            else
            {
                //делаем ход
                service.MakeMove(e.Location, pictureBox_place.Width);
                //сброс ходов
                service.ResetPossibleStep();
                //смена хода
                service.ChangePace();
                if(checkBox_bot.Checked)
                    step = WhoseStep.Bot;

                //проверяем не закончилась ли игра - нет ходов для текущего цвета - игра закончилась, чей сейчас был ход победил
                if (!service.IsHasMovies())
                {
                    string winner = "";
                    if (service.current_step == CurrentStep.White)
                        winner = "ЧЁРНЫЕ";
                    else winner = "БЕЛЫЕ";

                    //отдаем на отрисовку сразу, чтобы сначала был сделан ход, а только потом сообщение вывелось
                    pictureBox_place.Invalidate();
                    pictureBox_current_step.Invalidate();
                    Thread.Sleep(300);
                    MessageBox.Show($"Победили {winner}!", "Игра закончена!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    service.current_step = CurrentStep.None;
                    pictureBox_current_step.Invalidate();
                    IsBeginGame = false;//закончили игру
                    return;
                }
                //проверка на ничью
                if (service.movesWithoutCapture >= 30)
                {
                    //отдаем на отрисовку сразу, чтобы сначала был сделан ход, а только потом сообщение вывелось
                    pictureBox_place.Invalidate();
                    Thread.Sleep(300);
                    MessageBox.Show($"Ничья! 15 ходов без взятий!", "Игра закончена!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    service.current_step = CurrentStep.None;
                    pictureBox_current_step.Invalidate();
                    IsBeginGame = false;//закончили игру
                    return;
                }
                //отдаем на отрисовку
                pictureBox_place.Invalidate();
                pictureBox_current_step.Invalidate();
                buffer = null;
                if(checkBox_bot.Checked)
                    StepComputer();
            }
        }
        //Ход компьютера
        private void StepComputer()
        {
            computer.NextStepComputer(service);
            step = WhoseStep.Human;
            //проверяем не закончилась ли игра - нет ходов для текущего цвета - игра закончилась,  чей сейчас был ход победил
            if (!service.IsHasMovies())
            {
                string winner = "";
                if (service.current_step == CurrentStep.White)
                    winner = "ЧЁРНЫЕ";
                else winner = "БЕЛЫЕ";

                //отдаем на отрисовку сразу, чтобы сначала был сделан ход, а только потом сообщение вывелось
                pictureBox_place.Invalidate();
                pictureBox_current_step.Invalidate();

                Thread.Sleep(300);
                MessageBox.Show($"Победили {winner}!", "Игра закончена!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                service.current_step = CurrentStep.None;
                IsBeginGame = false;//закончили игру

            }
            //проверка на ничью
            if (service.movesWithoutCapture >= 30)
            {
                //отдаем на отрисовку сразу, чтобы сначала был сделан ход, а только потом сообщение вывелось
                pictureBox_place.Invalidate();
                pictureBox_current_step.Invalidate();
                Thread.Sleep(300);
                MessageBox.Show($"Ничья! 15 ходов без взятий!", "Игра закончена!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                service.current_step = CurrentStep.None;
                IsBeginGame = false;//закончили игру
            }
            //отдаем на отрисовку
            pictureBox_place.Invalidate();
            pictureBox_current_step.Invalidate();
        }

    }
}
