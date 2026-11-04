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
        bool whiteBottom;
        public Service(bool whiteBottom)
        {
            game_place = new int[8, 8];
            ResetGamePlace(whiteBottom);
        }
        public void ResetGamePlace(bool whiteBottom)
        {
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
            this.whiteBottom = whiteBottom;
        }

        public void DrawingGamePlace(Graphics g, int widthPlace, int heightPlace)
        {
            //отрисовываем игровое поле
            DrawingRepository.DrawingGamePlace(g, widthPlace, heightPlace);
            //отрисовываем шашки
            DrawingRepository.DrawingChekers(g, game_place, widthPlace, heightPlace);
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
            if (game_place[i_index, j_index]==0)
                return false;

            //проверка 2 - эта шашка цвета текущего хода?
            if (game_place[i_index, j_index]!=current_step)
                return false;

            
        }

        

        public void ResetPossibleStep()
        {
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j] == 2 || game_place[i,j]==-2)
                        game_place[i,j] = 0;
                }
            }
        }
    }
}
