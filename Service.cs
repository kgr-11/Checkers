using System;
using System.Collections.Generic;
using System.Linq;
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
        int[,] game_place;
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

        public Service(bool whiteBottom)
        {
            game_place = new int[8, 8];
            ResetGamePlace(whiteBottom);
        }
        public void ResetGamePlace(bool whiteBottom)
        {
            //whiteBottom==true - снизу белые, иначе снизу черные
            int countBlack = 0;
            int countWhite = 0; 
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (countBlack < 12)
                    {
                        if (whiteBottom)//черные сверху
                        {
                            if ((j + 1) % 2 == 0 && (i + 1) % 2 != 0|| (j + 1) % 2 != 0 && (i + 1) % 2 == 0)
                            {
                                game_place[i, j] = -1;
                                countBlack++;
                            }
                        }
                        else//черные снизу
                        {
                            if ((j + 1) % 2 != 0 && (game_place.GetUpperBound(0)-i + 1) % 2 == 0 ||
                                (j + 1) % 2 == 0 && (game_place.GetUpperBound(0) - i + 1) % 2 != 0)
                            {
                                game_place[game_place.GetUpperBound(0) - i, j] = -1;
                                countBlack++;
                            }
                        }
                    }
                    if (countWhite < 12)
                    {
                        if (whiteBottom)
                        {
                            if ((j + 1) % 2 != 0 && (game_place.GetUpperBound(0) - i + 1) % 2 == 0 ||
                                (j + 1) % 2 == 0 && (game_place.GetUpperBound(0) - i + 1) % 2 != 0)
                            {
                                game_place[game_place.GetUpperBound(0) - i, j] = 1;
                                countWhite++;
                            }
                        }
                        else
                        {
                            if ((j + 1) % 2 == 0 && (i + 1) % 2 != 0 || (j + 1) % 2 != 0 && (i + 1) % 2 == 0)
                            {
                                game_place[i, j] = 1;
                                countWhite++;
                            }
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
            //отрисовываем шашки
            DrawingRepository.DrawingChekers(g, game_place, widthPlace, heightPlace);
        }

        public void SearchPossibleStep(bool whiteBottom)
        {
            if (whiteBottom)
            {
                if (current_step == -1)//ход черных, они сверху
                {
                    //если шашкой можно сходить, ставим ее в соответ состояние 
                    for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
                    {
                        for (int j = 0; j < game_place.GetUpperBound(1); j++)
                        {
                            //для четных(для массива) строк и четных ячеек
                            if (i % 2 == 0 && j%2== 0)
                            {
                                //смотрим, можно ли с текущей клетки съесть
                                if (j + 1 <= game_place.GetUpperBound(1))
                                {
                                    if (game_place[i + 1, j + 1] == 1)
                                        game_place[i, j] = -2;
                                }
                            }
                            //для нечетных строк
                            if (i % 2 != 0 && j % 2 != 0)
                            {

                            }
                        }
                    }

                }
                else if (current_step == 1)
                {

                }
            }
            else
            {
                if (current_step == -1)
                {


                }
                else if (current_step == 1)
                {

                }
            }
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
    }
}
