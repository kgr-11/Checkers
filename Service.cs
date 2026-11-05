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
        // -100 дамка черная
        // -2 возможный ход черных
        //-1 - черные
        //0 свободная ячейка
        //1 - белая ячейка
        // 2 возможный ход белых
        // 100 дамка белая
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
            for(int i=0;i<8;i++)
                for(int j=0;j<8;j++)
                    place[i,j] =game_place[i,j].Value;
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

            //проверяем можно ли ходить этой шашкой
            return ThirdCheckForCheckPossibleStep(i_index,j_index);
        }

        private bool ThirdCheckForCheckPossibleStep(int index_i, int index_j)
        {
            bool res = false;
            //1. проверяем можно ли вообще ей сходить+проверка рубки выбранной шашки
            //перебираем соседей
            var checker = game_place[index_i, index_j];
            foreach (var neighbor in checker.neighbors)
            {
                if (neighbor.Value == 0)
                {   //если ход белых, то ходим в сторону понижения номера клетки
                    if (current_step == 1 && neighbor.Number < checker.Number
                        //если ход черных, то ходим в сторону повышения номера клетки
                        || current_step == -1 && neighbor.Number > checker.Number)
                    {
                        res = true;
                    }
                }
                else if (neighbor.Value == current_step)
                {
                    continue;
                }
                else if (neighbor.Value == -1 * current_step)
                {
                    //проверка рубки выбранной
                    if (Check_Kill(checker))
                        return true;
                }
            }

            //2. если у выбранной рубки не оказалось, проверяем другие шашки на наличие рубки
            for(int i=0;i<game_place.GetLength(0);i++)
            {
                for (int j = 0; j < game_place.GetLength(1); j++)
                {
                    if (game_place[i, j].Value == current_step)
                    {
                        //проверяем у шашки возможность рубки
                        if (Check_Kill(game_place[i, j]))
                            return false;//если какая-то шашка может срубить, значит выбранной нельзя ходить
                    }
                }
            }
            return res;
        }

        //проверка на возможность рубки шашкой
        bool Check_Kill(Node checker)
        {
            foreach (var neighbor in checker.neighbors)
            {
                if (neighbor.Value == 0 || neighbor.Value == current_step)
                    continue;
                //дошли до сюда -> сосед соперник- не важно с какой стороны, рубим в любую
                //мультирубка сейчас не анализируется, надо просто ответить можно рубить или нет

                //перебираем соседей соперника
                foreach (var neighbor_opponent in neighbor.neighbors)
                {
                    if (neighbor_opponent == checker)
                        continue;
                    if (neighbor_opponent.Value == 0 && Node.IsSameDiagonal(checker, neighbor_opponent))
                        return true;
                }
            }
            return false;
        }

        //функция срабатывает после всех проверок
        //устанавливает временные значения как возможные ходы, если эта функция вызвалась, значит они точно есть
        public void SetPossibleStep(Point cursor, int sizePlace)
        {
            //определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int i_index, out int j_index);

            var checker = game_place[i_index, j_index];
            foreach (var neighbor in checker.neighbors)
            {
                if (neighbor.Value == 0)
                {   //если ход белых, то ходим в сторону понижения номера клетки
                    if (current_step == 1 && neighbor.Number < checker.Number
                        //если ход черных, то ходим в сторону повышения номера клетки
                        || current_step == -1 && neighbor.Number > checker.Number)
                    {
                        if (current_step == -1)
                            neighbor.Value = -2;
                        else
                            neighbor.Value = 2;
                    }
                }
                else if (neighbor.Value == current_step)
                {
                    continue;
                }
                else if (neighbor.Value == -1 * current_step)
                {
                    SetStepForKill(checker);
                }
            }

        }

        void SetStepForKill(Node checker)
        {
            foreach (var neighbor in checker.neighbors)
            {
                if (neighbor.Value == 0 || neighbor.Value == current_step)
                    continue;
                //дошли до сюда -> сосед соперник- не важно с какой стороны, рубим в любую
                //мультирубка сейчас не анализируется, надо просто ответить можно рубить или нет

                //перебираем соседей соперника
                foreach (var neighbor_opponent in neighbor.neighbors)
                {
                    if (neighbor_opponent == checker)
                        continue;
                    if (neighbor_opponent.Value == 0 && Node.IsSameDiagonal(checker, neighbor_opponent))
                    {
                        //устанавливаю временное значение
                        neighbor_opponent.Value = current_step == -1 ? -2 : 2;
                        SetStepForKill(neighbor_opponent);//вдруг там мульти kill
                    }
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
