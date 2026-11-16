using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public static class Genetic_Algorithm
    {
        public static Network GA(List<int> scelet, int countIndivid, int countGeneration)
        {
            //генерация начальной популяции
            List<KeyValuePair<Network, int>> Population = new List<KeyValuePair<Network, int>>();
            for (int i = 0; i < countIndivid; i++)
            {
                Population.Add(new KeyValuePair<Network, int>(new Network(scelet), 0));//генерация особей популяции c нулевым рейтингом
            }
            Random random = new Random();


            List<KeyValuePair<Network, int>> elites = new List<KeyValuePair<Network, int>>();//отсюда потом вернем самую элитную особь

            //идем по поколениям
            for (int generation = 0; generation < countGeneration + 1; generation++)
            {
                //турнирный отбор - селекция
                
                //применение элетизма
                

                //скрещивание
                

                //мутации
               

                //сохранение новой популяции
                //проталкиваем элитных особей в следующее поколение 

            }
            
           
        }

        static int CheckWinner(int[,] game_place)
        {
            int countHorizontalCell = 3;
            int countVerticallCell = 3;
            //проверка строк
            int countZero = 0;
            int countCross = 0;
            int countVoid = 0;
            //проверка строк
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                countCross = 0;
                countZero = 0;
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j] == -1)
                        countCross++;
                    else if (game_place[i, j] == 1)
                        countZero++;
                    else
                        countVoid++;
                }
                if (countCross == countHorizontalCell)
                {
                    return -1;
                }
                if (countZero == countHorizontalCell)
                {
                    return 1;
                }
            }

            //проверка столбцов
            for (int i = 0; i < game_place.GetUpperBound(1) + 1; i++)
            {
                countCross = 0;
                countZero = 0;
                for (int j = 0; j < game_place.GetUpperBound(0) + 1; j++)
                {
                    if (game_place[j, i] == -1)
                        countCross++;
                    else if (game_place[j, i] == 1)
                        countZero++;
                }
                if (countCross == countVerticallCell)
                {
                    return -1;
                }
                if (countZero == countVerticallCell)
                {
                    return 1;
                }
            }

            countCross = 0;
            countZero = 0;
            //проверка главной диагонали
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                if (game_place[i, i] == -1)
                    countCross++;
                else if (game_place[i, i] == 1)
                    countZero++;
            }
            if (countCross == countVerticallCell)
            {
                return -1;
            }
            if (countZero == countVerticallCell)
            {
                return 1;
            }

            countCross = 0;
            countZero = 0;
            //проверка побочной диагонали
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                if (game_place[i, game_place.GetUpperBound(0) - i] == -1)
                    countCross++;
                else if (game_place[i, game_place.GetUpperBound(0) - i] == 1)
                    countZero++;
            }
            if (countCross == countVerticallCell)
            {
                return -1;
            }
            if (countZero == countVerticallCell)
            {
                return 1;
            }

            //если не вышли раньше, надо понять, игра закончена в ничью, или еще играем 
            if (countVoid != 0)//не все ячейки заполнены - продолжаем играть
                return 100;
            else//ничья
                return 0;
        }
    }
}