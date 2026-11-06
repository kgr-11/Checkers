using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    static class DrawingRepository
    {
        //рисование игрового поля
        //1. заливка клеток поля
        //2. отрисовка поверх сетки
        public static void DrawingGamePlace(Graphics g,int widthPlace, int heightPlace)
        {
            //размеры клетки
            float widthCell = (float)widthPlace /8;
            float heightCell = (float)heightPlace /8;

            //заливка поля
            using (Brush brushForWhiteCell = new SolidBrush(Color.FromArgb(255, 222, 193, 150)))
            using (Pen penForCheckers= new Pen(Color.FromArgb(255, 168, 163, 153),2.0f))
            using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
            using (Brush brushForBlackCell = new SolidBrush(Color.FromArgb(255, 150, 105, 70)))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //расчет координат верхнего левого угла клетки
                        float x = j * widthCell;
                        float y = i * heightCell;
                        //закрашиваем клетку
                        if (j % 2 == 0)
                        {
                            if (i % 2 == 0)
                            {
                                g.FillRectangle(brushForWhiteCell, x, y, widthCell, heightCell);
                            }
                            else
                            {
                                g.FillRectangle(brushForBlackCell, x, y, widthCell, heightCell);
                            }
                        }
                        else
                        {
                            if (i % 2 == 0)
                            {
                                g.FillRectangle(brushForBlackCell, x, y, widthCell, heightCell);
                            }
                            else
                            {
                                g.FillRectangle(brushForWhiteCell, x, y, widthCell, heightCell);
                            }
                        }
                    }
                }
            }
            //рисуем сетку
            using (Pen penForBorder = new Pen(Color.Black, 0.5f))
            {
                //горизонтали
                for (int i = 0; i < 9; i++)
                {
                    if (i == 8)
                        g.DrawLine(penForBorder, 0, i * heightCell - 1, widthPlace, i * heightCell - 1);
                    else
                        g.DrawLine(penForBorder, 0, i * heightCell, widthPlace, i * heightCell);
                }
                //вертикали
                for (int i = 0; i < 9; i++)
                {
                    if(i==8)
                        g.DrawLine(penForBorder, i * widthCell-1, 0, i * widthCell-1, heightPlace);
                    else
                        g.DrawLine(penForBorder, i * widthCell, 0, i * widthCell, heightPlace);
                }
            }
        }

        //функция рисования одной шашки, которую вызывает функция, которая отрисовывает все шашки доски
        private static void DrawingCheker(Graphics g,Brush brush,Pen pen,
            float x, float y,float size)
        {
            //вычисление радиусов 
            float D1 = size*0.92f;
            float D2 = D1 * 0.66f;
            float D3 = D1 * 0.33f;
            //рисование шашки
            //заливка цвета
            g.FillEllipse(brush, (float)(x + ((size - D1) / 2)),
                (float)(y + ((size - D1) / 2)), D1, D1);
            //рисование границ
            //больший радиус
            g.DrawEllipse(pen, (float)(x + ((size - D1) / 2)),
                (float)(y + ((size - D1) / 2)), D1, D1);
            //средний радиус
            g.DrawEllipse(pen, (float)(x + ((size - D2) / 2)),
                (float)(y + ((size - D2) / 2)), D2, D2);
            //меньший радиус
            g.DrawEllipse(pen, (float)(x + ((size - D3) / 2)),
                (float)(y + ((size - D3) / 2)), D3, D3);
        }

        //отрисовка всех шашек доски на текущих клетках
        public static void DrawingChekers(Graphics g, int[,]game_place, int widthPlace, int heightPlace)
        {
            //размеры клетки
            float widthCell = (float)widthPlace / 8;
            float heightCell = (float)heightPlace / 8;

            //заливка поля
            using (Pen penForWhiteKing = new Pen(Color.FromArgb(255, 168, 163, 153), 2.0f))
            using (Brush brushForWhiteKing = new SolidBrush(Color.FromArgb(255, 248, 243, 43)))
            using (Pen penForBlackKing = new Pen(Color.FromArgb(255, 77, 77, 77), 2.0f))
            using (Brush brushForBlackKing = new SolidBrush(Color.FromArgb(255, 193, 135, 107)))
            using (Pen penForPossibleStep = new Pen(Color.FromArgb(255, 102, 255, 0), 2.0f))
            using (Brush brushForPossibleStep = new SolidBrush(Color.FromArgb(255, 102, 255, 0)))
            using (Pen penForWhiteCheckers = new Pen(Color.FromArgb(255, 168, 163, 153), 2.0f))
            using (Brush brushForWhiteChekers = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
            using (Pen penForBlackCheckers = new Pen(Color.FromArgb(255, 77, 77, 77), 2.0f))
            using (Brush brushForBlackChekers = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
            {
                for (int i = 0; i < game_place.GetUpperBound(0)+1; i++)
                {
                    for (int j = 0; j < game_place.GetUpperBound(1)+1; j++)
                    {
                        //расчет координат верхнего левого угла клетки
                        float x = j * widthCell;
                        float y = i * heightCell;
                        if (game_place[i, j] == -1)
                        {
                            DrawingCheker(g, brushForBlackChekers, penForBlackCheckers, x, y, widthCell);
                        }
                        else if (game_place[i, j] == 1)
                        {
                            DrawingCheker(g, brushForWhiteChekers, penForWhiteCheckers, x, y, widthCell);
                        }
                        else if (game_place[i, j] == 2 || game_place[i, j] == -2)
                        {
                            DrawingCheker(g, brushForPossibleStep, penForPossibleStep, x, y, widthCell);
                        }
                        else if (game_place[i, j] == 100)
                        {
                            DrawingCheker(g, brushForWhiteKing,penForWhiteKing, x, y, widthCell);
                        }
                        else if (game_place[i, j] == -100)
                        {
                            DrawingCheker(g, brushForBlackKing, penForBlackKing, x, y, widthCell);
                        }
                    }
                }
            }
        }

        //функция рисует чей сейчас ход
        public static void DrawingCurrentStep(Graphics g, int size, bool IsWhite)
        {
            if (IsWhite)
            {
                using (Pen penForWhiteCheckers = new Pen(Color.FromArgb(255, 168, 163, 153), 2.0f))
                using (Brush brushForWhiteChekers = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
                {
                    DrawingCheker(g, brushForWhiteChekers, penForWhiteCheckers, 0, 0, size);
                }
            }
            else
            {
                using (Pen penForBlackCheckers = new Pen(Color.FromArgb(255, 77, 77, 77), 2.0f))
                using (Brush brushForBlackChekers = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                {
                    DrawingCheker(g, brushForBlackChekers, penForBlackCheckers, 0, 0, size);
                }
            }
        }

        
    }
}
