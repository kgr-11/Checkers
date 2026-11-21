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
        //возможные значения на доске
        // -2 черная дамка
        //-1 - черная шашка
        //0 свободная ячейка
        //1 - белая шашка
        // 2 белая дамка
        // 100 возможный ход текущего шашки

        Node[,] game_place;//игровое поле
        public CurrentStep current_step; //чей текущий ход
        int countWhite;  // текущее кол-во белых шашек
        int countBlack;  // текущее кол-во черных шашек
        public int CountWhite
            { get { return countWhite; } }
        public int CountBlack
        { get { return countBlack; } }
        public Service()
        {
            game_place = new Node[8, 8];
            ResetGamePlace();
        }

        //функция сброса игрового поля в исходное состояние
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

            //game_place[5, 2].Value = 2;
            /*game_place[4,3].Value = -1;
            game_place[4, 5].Value = -1;
            game_place[5, 6].Value = 0;
            game_place[7, 4].Value = 0;
            game_place[6, 5].Value = -1;
            game_place[6, 7].Value = 0;
            game_place[5, 2].Value = 0;
            game_place[6, 1].Value = 2;
            game_place[4, 1].Value = -1;
            game_place[6, 3].Value = -1;
            game_place[7, 0].Value = 2;*/
            this.countWhite = countWhite;
            this.countBlack= countBlack;
        }

        // функция посредник между классом Form1 и классом рисования
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
                }
            }
            //отрисовываем шашки
            DrawingRepository.DrawingChekers(g, place, widthPlace, heightPlace);
        }

        // функция посредник между классом Form1 и классом рисования -
        // нужна для отрисовки чей сейчас ход в маленьком PictureBox
        public void DrawingCurrentStep(Graphics g, int widthPlace, int heightPlace)
        {
            if (current_step == CurrentStep.White)
            {
                DrawingRepository.DrawingCurrentStep(g, widthPlace, true);
            }
            else if (current_step == CurrentStep.Black)
            {
                DrawingRepository.DrawingCurrentStep(g, widthPlace, false);
            }
        }


        //функция определяет в какой клетке курсор
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
            if (current_step == CurrentStep.White&&(game_place[i_index, j_index].Value != 1 && game_place[i_index, j_index].Value != 2)
                || current_step == CurrentStep.Black && (game_place[i_index, j_index].Value != -1 && game_place[i_index, j_index].Value != -2)) 
            {
                return false;
            }

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

       /* Node choiceNode;
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
                        if (current_step == CurrentStep.Black)
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
                        if (current_step == CurrentStep.Black)
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
                        if (current_step == CurrentStep.Black)
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
        }*/

        //функция сброса возможного хода
        public void ResetPossibleStep()
        {
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j].Value == 100)
                        game_place[i, j].Value = 0;
                }
            }
        }
    }
}
