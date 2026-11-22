using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public Node[,] game_place;//игровое поле
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
            current_step = CurrentStep.White;
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

            //game_place[0, 0].Value = 0;
            //game_place[0, 1].Value = 0;
            //game_place[0, 2].Value = 0;
            //game_place[0, 3].Value = 0;
            //game_place[0, 4].Value = 0;
            //game_place[0, 5].Value = 0;
            //game_place[0, 6].Value = 0;
            //game_place[0, 7].Value = 0;
            //game_place[1, 0].Value = 0;
            //game_place[1, 1].Value = 0;
            //game_place[1, 2].Value = 0;
            //game_place[1, 3].Value = 0;
            //game_place[1, 4].Value = 0;
            //game_place[1, 5].Value = 0;
            //game_place[1, 6].Value = 0;
            //game_place[1, 7].Value = 0;
            //game_place[2, 0].Value = 0;
            //game_place[2, 1].Value = 0;
            //game_place[2, 2].Value = 0;
            //game_place[2, 3].Value = 0;
            //game_place[2, 4].Value = 0;
            //game_place[2, 5].Value = 0;
            //game_place[2, 6].Value = 0;
            //game_place[2, 7].Value = 0;
            //game_place[3, 0].Value = 0;
            //game_place[3, 1].Value = 0;
            //game_place[3, 2].Value = 0;
            //game_place[3, 3].Value = 0;
            //game_place[3, 4].Value = 0;
            //game_place[3, 5].Value = 0;
            //game_place[3, 6].Value = 0;
            //game_place[3, 7].Value = 0;
            //game_place[4, 0].Value = 0;
            //game_place[4, 1].Value = 0;
            //game_place[4, 2].Value = 0;
            //game_place[4, 3].Value = 0;
            //game_place[4, 4].Value = 0;
            //game_place[4, 5].Value = 0;
            //game_place[4, 6].Value = 0;
            //game_place[4, 7].Value = 0;
            //game_place[5, 0].Value = 0;
            //game_place[5, 1].Value = 0;
            //game_place[5, 2].Value = 0;
            //game_place[5, 3].Value = 0;
            //game_place[5, 4].Value = 0;
            //game_place[5, 5].Value = 0;
            //game_place[5, 6].Value = 0;
            //game_place[5, 7].Value = 0;
            //game_place[6, 0].Value = 0;
            //game_place[6, 1].Value = 0;
            //game_place[6, 2].Value = 0;
            //game_place[6, 3].Value = 0;
            //game_place[6, 4].Value = 0;
            //game_place[6, 5].Value = 0;
            //game_place[6, 6].Value = 0;
            //game_place[6, 7].Value = 0;
            //game_place[7, 0].Value = 0;
            //game_place[7, 1].Value = 0;
            //game_place[7, 2].Value = 0;
            //game_place[7, 3].Value = 0;
            //game_place[7, 4].Value = 0;
            //game_place[7, 5].Value = 0;
            //game_place[7, 6].Value = 0;
            //game_place[7, 7].Value = 0;
            //game_place[3, 4].Value = 2;
            //game_place[1, 0].Value = -2;
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
            movesWithoutCapture = 0;
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
        //этот вариант метода вызывается вне класса
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
        //этот вариант метода вызывается из класса
        private bool CheckPossibleStep(in int i_index, in int j_index)
        {
            //проверка 1 - эта клетка с шашкой?
            if (game_place[i_index, j_index].Value == 0)
                return false;

            //проверка 2 - эта шашка цвета текущего хода?
            if (current_step == CurrentStep.White && (game_place[i_index, j_index].Value != 1 && game_place[i_index, j_index].Value != 2)
                || current_step == CurrentStep.Black && (game_place[i_index, j_index].Value != -1 && game_place[i_index, j_index].Value != -2))
            {
                return false;
            }

            //проверка 3 -  на возможность сходить выбранной шашкой - без разницы дамка или просто шашка, функция обобщена
            return Checker.IsPossibleMakeMoveChecker(game_place[i_index, j_index], game_place, current_step);
        }

        List<List<Node>> buffer;
        //прослойка
        public void SetPossibleStep(Point cursor, int sizePlace)
        {
            //определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int i, out int j);
            //принимаем список путей из текущей клетки
            buffer = Checker.SetPossibleStep(game_place[i,j], game_place, in current_step);
        }

        
        public bool IsMakeMove(Point cursor, int sizePlace)
        {
            // определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int ii, out int jj);
            int number =ii * 8 + (jj + 1);
            foreach (var lst in buffer)
            {
                if (lst[lst.Count-1].Number==number)
                    return true;
            }
            return false;
        }
        public int movesWithoutCapture = 0;      // счётчик полуходов без рубки
        public void MakeMove(Point cursor, int sizePlace)
        {
            // определяем индексы клетки в матрице игры
            CoordCursorInNumberCell(cursor, sizePlace, out int ii, out int jj);
            int number = ii  * 8 + (jj + 1);
            int indexPath = -1;
            //находим нужный путь
            for(int i=0;i<buffer.Count;i++)
            {
                if (buffer[i][buffer[i].Count-1].Number == number)
                { 
                    indexPath=i; break;
                }  
            }
            var val=buffer[indexPath][0].Value;
            bool IsKill = false;
            //идем по пути
            for (int i = 1; i < buffer[indexPath].Count-1; i++)
            {
                if (buffer[indexPath][i].Value == -1 || buffer[indexPath][i].Value == -2)
                {
                    countBlack--;
                    IsKill=true;
                }
                else if (buffer[indexPath][i].Value == 1 || buffer[indexPath][i].Value == 2)
                {
                    countWhite--;
                    IsKill = true;
                }
                //проверка, что шашка может стать дамкой - могли прошмыгнуть ту сторону
                if (current_step == CurrentStep.White && buffer[indexPath][i].Number < 9)
                    val = 2;
                else if(current_step == CurrentStep.Black && buffer[indexPath][i].Number>56)
                    val = -2;

                buffer[indexPath][i].Value = 0;
            }
            buffer[indexPath][0].Value = 0;
            //если четко встали на противоположную сторону
            if ((current_step == CurrentStep.Black && buffer[indexPath][buffer[indexPath].Count - 1].Number > 56)
                || (current_step == CurrentStep.White && buffer[indexPath][buffer[indexPath].Count - 1].Number <9) )
            { 
                val=current_step==CurrentStep.White ? 2 : -2;
            }
            buffer[indexPath][buffer[indexPath].Count - 1].Value = val;

            // Обновляем счётчики
            if (IsKill)
                movesWithoutCapture = 0;
            else
                movesWithoutCapture++;

        }
        //для инс
        public void MakeMove(List<Node> steps)
        {
            int index_i, index_j;
            var val = steps[0].Value;
            //идем по пути
            for (int i = 1; i < steps.Count - 1; i++)
            {
                //проверка, что шашка может стать дамкой - могли прошмыгнуть ту сторону
                if (current_step == CurrentStep.White && steps[i].Number < 9)
                    val = 2;
                else if (current_step == CurrentStep.Black && steps[i].Number > 56)
                    val = -2;
                steps[i].IndexInMtx(out index_i, out index_j);
                game_place[index_i, index_j].Value = 0;
            }
            steps[0].IndexInMtx(out index_i, out index_j);
            game_place[index_i, index_j].Value = 0;
            //если четко встали на противоположную сторону
            if ((current_step == CurrentStep.Black && steps[steps.Count - 1].Number > 56)
                || (current_step == CurrentStep.White && steps[steps.Count - 1].Number < 9))
            {
                val = current_step == CurrentStep.White ? 2 : -2;
            }
            steps[steps.Count - 1].IndexInMtx(out index_i, out index_j);
            game_place[index_i, index_j].Value = val;
        }


        //для оценки
        //функция для обучения -> передается доска, ходы, текущий ход и доска изменяется в соответствие с ходами
        public static void MakeMove(Node[,]game_place, List<Node>steps, CurrentStep currentStep)
        {
            int index_i,index_j;
            var val = steps[0].Value;
            //идем по пути
            for (int i = 1; i < steps.Count - 1; i++)
            {
                //проверка, что шашка может стать дамкой - могли прошмыгнуть ту сторону
                if (currentStep == CurrentStep.White && steps[i].Number < 9)
                    val = 2;
                else if (currentStep == CurrentStep.Black && steps[i].Number > 56)
                    val = -2;
                steps[i].IndexInMtx(out index_i, out index_j);
                game_place[index_i, index_j].Value = 0;
            }
            steps[0].IndexInMtx(out index_i, out index_j);
            game_place[index_i, index_j].Value = 0;
            //если четко встали на противоположную сторону
            if ((currentStep == CurrentStep.Black && steps[steps.Count - 1].Number > 56)
                || (currentStep == CurrentStep.White && steps[steps.Count - 1].Number < 9))
            {
                val = currentStep == CurrentStep.White ? 2 : -2;
            }
            steps[steps.Count - 1].IndexInMtx(out index_i, out index_j);
            game_place[index_i, index_j].Value = val;
        }

        //функция поверхностного копирования всего игрового поля, создается полная копия
        public static Node[,] CopyGamePlace(Node[,] game_place)
        {
            Node[,]copy=new Node[game_place.GetLength(0), game_place.GetLength(1)];
            //создаем игровое поле
            int count = 1;
            for (int i = 0; i < copy.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < copy.GetUpperBound(1) + 1; j++)
                {
                    copy[i, j] = new Node(0, count++);
                }
            }
            //заполняем поле
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    copy[i,j].Value=game_place[i,j].Value;
                }
            }

            //настраиваем связи
            for (int i = 0; i < copy.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < copy.GetUpperBound(1) + 1; j++)
                {
                    //верхняя строка - добавляем соседей cо строки ниже
                    if (i == 0 && j % 2 != 0)
                    {
                        if (j - 1 >= 0)
                            copy[i, j].neighbors.Add(copy[i + 1, j - 1]);
                        if (j + 1 <= copy.GetUpperBound(1))
                            copy[i, j].neighbors.Add(copy[i + 1, j + 1]);
                    }
                    //нижняя строка -добавляем соседей со строки выше
                    else if (i == copy.GetUpperBound(0) && j % 2 == 0)
                    {
                        if (j - 1 >= 0)
                            copy[i, j].neighbors.Add(copy[i - 1, j - 1]);
                        if (j + 1 <= copy.GetUpperBound(1))
                            copy[i, j].neighbors.Add(copy[i - 1, j + 1]);
                    }
                    //у этих соседи и сверху, и снизу
                    else if (i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0)
                    {
                        if (j - 1 >= 0)
                        {
                            copy[i, j].neighbors.Add(copy[i + 1, j - 1]);
                            copy[i, j].neighbors.Add(copy[i - 1, j - 1]);
                        }
                        if (j + 1 <= copy.GetUpperBound(1))
                        {
                            copy[i, j].neighbors.Add(copy[i + 1, j + 1]);
                            copy[i, j].neighbors.Add(copy[i - 1, j + 1]);
                        }
                    }
                }
            }
            return copy;
        }


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
        public void ChangePace()
        { 
            current_step=current_step==CurrentStep.White?CurrentStep.Black:CurrentStep.White;
        }


        public bool IsHasMovies()
        {
            for (int i = 0; i < game_place.GetLength(0); i++)
            {
                for (int j = 0; j < game_place.GetLength(1); j++)
                {
                    //проверяем ходы для белых
                    if ((game_place[i, j].Value == 1 || game_place[i, j].Value == 2) && current_step == CurrentStep.White)
                    {
                        if (CheckPossibleStep(in i,in j))
                            return true;
                    }
                    //проверяем ходы для черных
                    else if ((game_place[i, j].Value == -1 || game_place[i, j].Value == -2) && current_step == CurrentStep.Black)
                    {
                        if (CheckPossibleStep(in i, in j))
                            return true;
                    }
                }
            }
            return false;
        }
        //функция определяет все шашки, которыми сейчас можно сходить - нужна для ИНС
        public Dictionary<Node, List<List<Node>>> All_Possible_Move()
        { 
            //список пар: ключ- какой шашкой можно сходить, значение список ходов, которые можно сделать этой шашкой
            Dictionary<Node,List<List<Node>>> steps= new Dictionary<Node, List<List<Node>>>();
            for (int i = 0; i < game_place.GetLength(0); i++)
            {
                for (int j = 0; j < game_place.GetLength(1); j++)
                {
                    //проверяем ходы для белых
                    if ((game_place[i, j].Value == 1 || game_place[i, j].Value == 2) && current_step == CurrentStep.White)
                    {
                        if (CheckPossibleStep(in i, in j))
                        { 
                            Node key=game_place[i, j];//это ключ пары
                            //теперь надо определить значение, -> куда можно сходить данной шашкой
                            SetPossibleStep(key);
                            //в buffer2 находится возможные пути
                            List<List<Node>> value= new List<List<Node>>();
                            foreach (var lst in buffer2)
                            {
                                value.Add(new List<Node>());
                                foreach (var node in lst)
                                {
                                    value[value.Count - 1].Add(new Node(node.Value, node.Number));
                                }
                            }
                            steps[key]=value;
                            ResetPossibleStep();
                        }

                    }
                    //проверяем ходы для черных
                    else if ((game_place[i, j].Value == -1 || game_place[i, j].Value == -2) && current_step == CurrentStep.Black)
                    {
                        if (CheckPossibleStep(in i, in j))
                        {
                            Node key = game_place[i, j];//это ключ пары
                            //теперь надо определить значение, -> куда можно сходить данной шашкой
                            SetPossibleStep(key);
                            //в buffer2 находится возможные пути
                            List<List<Node>> value = new List<List<Node>>();
                            foreach (var lst in buffer2)
                            {
                                value.Add(new List<Node>());
                                foreach (var node in lst)
                                {
                                    value[value.Count - 1].Add(new Node(node.Value, node.Number));
                                }
                            }
                            steps[key] = value;
                            ResetPossibleStep();
                        }
                    }
                }
            }
            return steps;
        }
        // это для ИНС
        List<List<Node>> buffer2;
        public void SetPossibleStep(Node checker)
        {
            //принимаем список путей из текущей клетки
            buffer2 = Checker.SetPossibleStep(checker, game_place, in current_step);
        }
    }
}
