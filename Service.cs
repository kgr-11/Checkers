using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Service
    {
        //игровое поле
        // -2 возможный ход черных
        //-1 - черные
        //0 свободная ячейка
        //1 - белая ячейка
        // 2 возможный ход белых
        Node[,] game_place;
        int current_step;
        int countWhite;
        int countBlack;
        public int CountWhite
            { get { return countWhite; } }
        public int CountBlack
        { get { return countBlack; } }
        public int CurrentStep
        {
            set { current_step = value; }
            get { return current_step; }
        }
        public Service()
        {
            game_place = new Node[8, 8];
            ResetGamePlace();
        }
        public void ResetGamePlace()
        {
            int countBlack = 0;
            int countWhite = 0;
            int count = 1;
            //создаем игровое поле
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    game_place[i, j] = new Node(0,count++);
                }
            }
            //заполняем поле
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (countBlack < 12)
                    {
                        if ((j + 1) % 2 == 0 && (i + 1) % 2 != 0 || (j + 1) % 2 != 0 && (i + 1) % 2 == 0)
                        {
                            game_place[i, j] = new Node(-1, game_place[i, j].Number);
                            countBlack++;
                        }
                    }
                    if (countWhite < 12)
                    {
                        if ((j + 1) % 2 != 0 && (game_place.GetUpperBound(0) - i + 1) % 2 == 0 ||
                                (j + 1) % 2 == 0 && (game_place.GetUpperBound(0) - i + 1) % 2 != 0)
                        {
                            game_place[game_place.GetUpperBound(0) - i, j] = new Node(1, game_place[game_place.GetUpperBound(0) - i, j].Number);
                            countWhite++;
                        }
                    }
                }
            }
            //настраиваем связи
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    //верхняя строка - добавляем соседей cо строки ниже
                    if (i == 0 && j % 2 != 0)
                    {
                        if (j - 1 >= 0)
                            game_place[i, j].neighbors.Add(game_place[i + 1, j - 1]);
                        if (j + 1 <= game_place.GetUpperBound(1))
                            game_place[i, j].neighbors.Add(game_place[i + 1, j + 1]);
                    }
                    //нижняя строка -добавляем соседей со строки выше
                    else if (i == game_place.GetUpperBound(0) && j % 2 == 0)
                    {
                        if (j - 1 >= 0)
                            game_place[i, j].neighbors.Add(game_place[i - 1, j - 1]);
                        if (j + 1 <= game_place.GetUpperBound(1))
                            game_place[i, j].neighbors.Add(game_place[i - 1, j + 1]);
                    }
                    //у этих соседи и сверху, и снизу
                    else if (i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0)
                    {
                        if (j - 1 >= 0)
                        {
                            game_place[i, j].neighbors.Add(game_place[i + 1, j - 1]);
                            game_place[i, j].neighbors.Add(game_place[i - 1, j - 1]);
                        }
                        if (j + 1 <= game_place.GetUpperBound(1))
                        {
                            game_place[i, j].neighbors.Add(game_place[i + 1, j + 1]);
                            game_place[i, j].neighbors.Add(game_place[i - 1, j + 1]);
                        }
                    }  
                }
            }
            this.countWhite = countWhite;
            this.countBlack= countBlack;
        }

        public void DrawingGamePlace(Graphics g, int widthPlace, int heightPlace)
        {
            //отрисовываем игровое поле
            DrawingRepository.DrawingGamePlace(g, widthPlace, heightPlace);
            int[,] place= new int[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                { 
                    place[i, j] = game_place[i, j].Value;
                    if (game_place[i, j].IsKing)
                        place[i, j] *= 100;
                }
            }
            //отрисовываем шашки
            DrawingRepository.DrawingChekers(g, place, widthPlace, heightPlace);
        }

        public void DrawingCurrentStep(Graphics g, int widthPlace, int heightPlace)
        {
            if (current_step == 1)
            {
                DrawingRepository.DrawingCurrentStep(g, widthPlace, true);
            }
            else if (current_step == -1)
            {
                DrawingRepository.DrawingCurrentStep(g, widthPlace, false);
            }
        }

        private void CoordCursorInNumberCell(Point cursor, int sizePlace, out int i,out int j)
        {
            //размеры клетки
            float sizeCell = (float)sizePlace / 8;
            i = (int)(cursor.Y / sizeCell);
            j = (int)(cursor.X / sizeCell);
        }
        public bool CheckPossibleStep(Point cursor, int sizePlace)
        {
            //определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int i_index, out int j_index);

            //проверка 1 - эта клетка с шашкой?
            if (game_place[i_index, j_index].Value == 0)
                return false;

            //проверка 2 - эта шашка цвета текущего хода?
            if (game_place[i_index, j_index].Value != current_step)
                return false;

            //проверка 3 -  на возможность сходить выбранной шашкой - без разницы дамка или просто шашка, функция обобщена
            return Checker.IsPossibleMakeMoveChecker(game_place[i_index, j_index], game_place, current_step);
        }

        //прослойка
        public void SetPossibleStep(Point cursor, int sizePlace)
        {
            //определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int i, out int j);
            Checker.SetPossibleStep(game_place[i,j], game_place, in current_step);
        }

        Node choiceNode;
        //принимает сразу допустимые комбинации клавиш
        public void MakeMove(Keys key1, Keys key2)
        {
            if (choiceNode is null)
            {
                return;
            }
            choiceNode.IndexInMtx(out int i, out int j);
            //вверх-влево
            if (key1 == Keys.Up && key2 == Keys.Left ||
                key1 == Keys.Left && key2 == Keys.Up)
            {
                while (i >= 0 && j >= 0)
                {
                    if (game_place[i,j].Value==2|| game_place[i, j].Value == -2) 
                    {
                        if (Math.Abs(choiceNode.Value) == 1)
                        {
                            if (current_step == -1)
                            {
                                game_place[i, j].Value = -1;
                                choiceNode.Value = 0;
                                if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                                {
                                    game_place[i + 1, j + 1].Value = 0;
                                    choiceNode = game_place[i, j];
                                    countWhite--;
                                    if (choiceNode.Number >= 57)
                                        choiceNode.IsKing = true;
                                }
                                else if (game_place[i, j].Number >= 57)
                                {
                                    choiceNode.IsKing = true;
                                }

                            }
                            else
                            {
                                game_place[i, j].Value = 1;
                                choiceNode.Value = 0;
                                if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                                {
                                    game_place[i + 1, j + 1].Value = 0;
                                    choiceNode = game_place[i, j];
                                    countBlack--;
                                    if (choiceNode.Number <=8)
                                        choiceNode.IsKing = true;
                                }
                                else if (game_place[i, j].Number <=8)
                                {
                                    choiceNode.IsKing = true;
                                }
                            }
                        }
                        else//иначе это дамка
                        {
                            if (current_step == -1)
                            {
                                choiceNode.IsKing = true;
                                choiceNode.Value = 0;
                                if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                                {
                                    game_place[i + 1, j + 1].Value = 0;
                                    choiceNode = game_place[i, j];
                                    countWhite--;
                                }
                            }
                            else
                            {
                                choiceNode.IsKing = true;
                                choiceNode.Value = 0;
                                if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                                {
                                    game_place[i + 1, j + 1].Value = 0;
                                    choiceNode = game_place[i, j];
                                    countBlack--;
                                }
                            }
                        }
                        break;
                    }
                    i--;j--;
                }
            }
            //вверх-вправо
            else if (key1 == Keys.Up && key2 == Keys.Right ||
                key1 == Keys.Right && key2 == Keys.Up)
            {
                while (i >= 0 && j<=game_place.GetUpperBound(1))
                {
                    if (game_place[i, j].Value == 2 || game_place[i, j].Value == -2)
                    {
                        if (current_step == -1)
                        {
                            game_place[i, j].Value = -1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 7)
                            {
                                game_place[i + 1, j - 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countWhite--;
                            }
                        }
                        else
                        {
                            game_place[i, j].Value = 1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 7)
                            {
                                game_place[i + 1, j - 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countBlack--;
                            }
                        }
                        break;
                    }
                    i--; j++;
                }
            }
            //вниз-влево
            else if (key1 == Keys.Down && key2 == Keys.Left ||
                key1 == Keys.Left && key2 == Keys.Down)
            {
                while (i<=game_place.GetUpperBound(0) && j >= 0)
                {
                    if (game_place[i, j].Value == 2 || game_place[i, j].Value == -2)
                    {
                        if (current_step == -1)
                        {
                            game_place[i, j].Value = -1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 7)
                            {
                                game_place[i - 1, j + 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countWhite--;
                            }
                        }
                        else
                        {
                            game_place[i, j].Value = 1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 7)
                            {
                                game_place[i - 1, j + 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countBlack--;
                            }
                        }
                        break;
                    }
                    i++; j--;
                }
            }
            //вниз-вправо
            else
            {
                while (i <=game_place.GetUpperBound(0) && j <=game_place.GetUpperBound(1))
                {
                    if (game_place[i, j].Value == 2 || game_place[i, j].Value == -2)
                    {
                        if (current_step == -1)
                        {
                            game_place[i, j].Value = -1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                            {
                                game_place[i - 1, j - 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countWhite--;
                            }
                        }
                        else
                        {
                            game_place[i, j].Value = 1;
                            choiceNode.Value = 0;
                            if (Math.Abs(game_place[i, j].Number - choiceNode.Number) > 9)
                            {
                                game_place[i - 1, j - 1].Value = 0;
                                choiceNode = game_place[i, j];
                                countBlack--;

                            }
                        }
                        break;
                    }
                    i++; j++;
                }
            }
        }

        public void ResetPossibleStep()
        {
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j].Value == 2 || game_place[i, j].Value == -2)
                        game_place[i, j].Value = 0;
                }
            }
        }
    }
}
