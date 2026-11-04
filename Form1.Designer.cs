namespace Checkers
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            pictureBox_place = new PictureBox();
            groupBox2 = new GroupBox();
            pictureBox_current_step = new PictureBox();
            textBox_countBlack = new TextBox();
            textBox_countWhite = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox3 = new GroupBox();
            checkBox_bot = new CheckBox();
            button_reset = new Button();
            label4 = new Label();
            button_game_start = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_place).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_current_step).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(pictureBox_place);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(914, 935);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Игровое поле";
            // 
            // pictureBox_place
            // 
            pictureBox_place.Location = new Point(6, 26);
            pictureBox_place.Name = "pictureBox_place";
            pictureBox_place.Size = new Size(900, 900);
            pictureBox_place.TabIndex = 0;
            pictureBox_place.TabStop = false;
            pictureBox_place.Paint += pictureBox_place_Paint;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pictureBox_current_step);
            groupBox2.Controls.Add(textBox_countBlack);
            groupBox2.Controls.Add(textBox_countWhite);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(932, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(334, 222);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Информация об игре";
            // 
            // pictureBox_current_step
            // 
            pictureBox_current_step.Location = new Point(213, 26);
            pictureBox_current_step.Name = "pictureBox_current_step";
            pictureBox_current_step.Size = new Size(80, 80);
            pictureBox_current_step.TabIndex = 5;
            pictureBox_current_step.TabStop = false;
            pictureBox_current_step.Paint += pictureBox_current_step_Paint;
            // 
            // textBox_countBlack
            // 
            textBox_countBlack.Enabled = false;
            textBox_countBlack.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold);
            textBox_countBlack.Location = new Point(195, 172);
            textBox_countBlack.Name = "textBox_countBlack";
            textBox_countBlack.Size = new Size(125, 34);
            textBox_countBlack.TabIndex = 4;
            // 
            // textBox_countWhite
            // 
            textBox_countWhite.Enabled = false;
            textBox_countWhite.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold);
            textBox_countWhite.Location = new Point(195, 123);
            textBox_countWhite.Name = "textBox_countWhite";
            textBox_countWhite.Size = new Size(125, 34);
            textBox_countWhite.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 13.8F);
            label3.Location = new Point(6, 175);
            label3.Name = "label3";
            label3.Size = new Size(192, 26);
            label3.TabIndex = 2;
            label3.Text = "Черных осталось: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 13.8F);
            label2.Location = new Point(18, 126);
            label2.Name = "label2";
            label2.Size = new Size(171, 26);
            label2.TabIndex = 1;
            label2.Text = "Белых осталось:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 13.8F);
            label1.Location = new Point(39, 61);
            label1.Name = "label1";
            label1.Size = new Size(150, 26);
            label1.TabIndex = 0;
            label1.Text = "Текущий ход: ";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkBox_bot);
            groupBox3.Controls.Add(button_reset);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(button_game_start);
            groupBox3.Location = new Point(932, 240);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(334, 287);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Панель управления";
            // 
            // checkBox_bot
            // 
            checkBox_bot.AutoSize = true;
            checkBox_bot.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            checkBox_bot.Location = new Point(156, 33);
            checkBox_bot.Name = "checkBox_bot";
            checkBox_bot.Size = new Size(116, 30);
            checkBox_bot.TabIndex = 4;
            checkBox_bot.Text = "С ботом";
            checkBox_bot.UseVisualStyleBackColor = true;
            // 
            // button_reset
            // 
            button_reset.Font = new Font("Times New Roman", 13.8F);
            button_reset.Location = new Point(6, 184);
            button_reset.Name = "button_reset";
            button_reset.Size = new Size(322, 92);
            button_reset.TabIndex = 2;
            button_reset.Text = "Сброс";
            button_reset.UseVisualStyleBackColor = true;
            button_reset.Click += button_reset_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 13.8F);
            label4.Location = new Point(6, 33);
            label4.Name = "label4";
            label4.Size = new Size(144, 26);
            label4.TabIndex = 3;
            label4.Text = "Режим игры: ";
            // 
            // button_game_start
            // 
            button_game_start.Font = new Font("Times New Roman", 13.8F);
            button_game_start.Location = new Point(6, 76);
            button_game_start.Name = "button_game_start";
            button_game_start.Size = new Size(322, 92);
            button_game_start.TabIndex = 0;
            button_game_start.Text = "Начать игру";
            button_game_start.UseVisualStyleBackColor = true;
            button_game_start.Click += button_game_start_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 959);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Игра \"Шашки\"";
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox_place).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_current_step).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private PictureBox pictureBox_place;
        private GroupBox groupBox2;
        private PictureBox pictureBox_current_step;
        private TextBox textBox_countBlack;
        private TextBox textBox_countWhite;
        private Label label3;
        private Label label2;
        private Label label1;
        private GroupBox groupBox3;
        private Button button_game_start;
        private Button button_reset;
        private Label label4;
        private CheckBox checkBox_bot;
    }
}
