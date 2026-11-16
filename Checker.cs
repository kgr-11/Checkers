using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    
    public static class Checker
    {
        //функция проверки: можно ли ходить выбранной шашкой
        public static bool IsPossibleMakeMoveChecker(Node checker, Node[,] game_place, in int current_step)
        {
            bool res = false;
            // 1. Проверяем саму шашку -просто шашку/дамка
            // а. смотрим можно ли ей сделать тихий ход
            // б. можно ли ей сделать рубку -  если да, значит этой шашкой можно ходить без дальнейших проверок
            foreach (var neighbor in checker.neighbors)
            {
                //если это дамка и если пустая клетка - тихий ход
                //без разницы в каком направлении
                if (neighbor.Value == 0 && checker.IsKing)
                {
                    res = true;
                }
                // если не дамка, то важно направление
                else if (!checker.IsKing && neighbor.Value == 0)
                {
                    if (current_step == 1 && neighbor.Number < checker.Number
                        //если ход черных, то ходим в сторону повышения номера клетки
                        || current_step == -1 && neighbor.Number > checker.Number)
                    {
                        res = true;
                    }
                }
                //если на соседней клетке соперник - проверяем можно ли срубить
                else if (neighbor.Value == -1 * current_step)
                {
                    //проверка рубки выбранной
                    if (Check_Kill(checker, current_step))
                        return true;
                }
            }

            // 2. если у выбранной дамки нет рубки
            // надо проверить остальных, ведь если у них есть рубка, то данной дамкой ходить нельзя
            for (int i = 0; i < game_place.GetLength(0); i++)
            {
                for (int j = 0; j < game_place.GetLength(1); j++)
                {
                    if (game_place[i, j].Value == current_step)
                    {
                        //здесь проверяем остальные шашки этого же цвета на возможность рубки и дамки на возможность рубки
                        if (!game_place[i, j].IsKing && Checker.Check_Kill(game_place[i, j], current_step)
                            || game_place[i, j].IsKing && Check_Kill(game_place[i, j], in current_step))
                            return false;
                    }
                }
            }
            return res;
        }
        //функция проверки: есть ли ход для рубки для выбранной дамки
        // ! не важно мульти килл или просто рубка
        private static bool Check_Kill(Node checker, in int current_step)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            queue.Enqueue(checker);
            do
            {
                Node node = queue.Dequeue();
                if (!visited.Contains(node))
                    visited.Add(node);
                //перебираем соседей
                foreach (var neighbor in node.neighbors)
                {
                    // одна диагональ и клетка свободна и мы в ней не были, идем как бы в нее
                    if (Node.IsSameDiagonal(checker, neighbor) && neighbor.Value == 0 && !queue.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                    //если одна диагональ и в соседней клетке соперник -> проверка: можно ли срубить
                    else if (Node.IsSameDiagonal(checker, neighbor) && neighbor.Value == -1 * current_step)
                    {
                        //перебираем соседей соперника
                        foreach (var neighbor_opponent in neighbor.neighbors)
                        {
                            if (neighbor_opponent == node)
                                continue;
                            //если сосед соперника пустая клетка и на той же диагонали, что наша шашка -> можно рубить
                            if (neighbor_opponent.Value == 0 && Node.IsSameDiagonal(checker, neighbor_opponent))
                                return true;
                        }
                    }
                }
            } while (queue.Count > 0 && checker.IsKing);
            return false;
        }



        //функция установки допустимых ходов для выбранной шашки
        //функция срабатывает после всех проверок
        //устанавливает временные значения как возможные ходы, если эта функция вызвалась, значит они точно есть
        public static void SetPossibleStep(Node checker, Node[,] game_place, in int current_step)
        {
            // 1. устанавливаем сначала ходы для сруба, если рубить нельзя,
            // то тогда только устанавливаем варианты тихих ходов
            if (Check_Kill(checker, in current_step))
            {
                Stack<Node> stack = new Stack<Node>();
                HashSet<Node> visited = new HashSet<Node>();    
                stack.Push(checker);
                List<Node> newPositionChecker = new List<Node>();
                newPositionChecker.Add(checker);
                while (stack.Count > 0)
                {
                    Node node = stack.Peek();
                    if(!visited.Contains(node))
                        visited.Add(node);
                    if (node.Value == 0)
                        node.Value = 3;//промежуточное значение
                    //обход соседей
                    bool flag = false;
                    foreach (var neighbor in node.neighbors)
                    {
                        if (Node.IsSameDiagonal(neighbor, newPositionChecker[newPositionChecker.Count() - 1]) && node.Value == 0&& !visited.Contains(neighbor)&& newPositionChecker[newPositionChecker.Count() - 1].IsKing)
                        {
                            stack.Push(neighbor);
                            flag = true;
                            break;
                        }
                        else if (Node.IsSameDiagonal(neighbor, newPositionChecker[newPositionChecker.Count() - 1]) && node.Value == -1 * current_step && !visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            //перебираем соседей соперника - ищем пустую клетку, в которую можно встать после рубки
                            foreach (var neighbor_opponent in neighbor.neighbors)
                            {
                                //если сосед соперника пустая клетка и на той же диагонали, что наша шашка -> можно рубить
                                if (neighbor_opponent.Value == 0 && Node.IsSameDiagonal(newPositionChecker[newPositionChecker.Count()-1], neighbor_opponent))
                                {
                                    stack.Push(neighbor_opponent);
                                    newPositionChecker.Add(neighbor_opponent);
                                    break;
                                }
                            }
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        if (!newPositionChecker.Contains(node))
                            node.Value = 0;
                        stack.Pop();
                    }
                }

            }
            // 2. рубить нечего, устанавливаем значения для тихого хода
            else 
            {
                Queue<Node>queue = new Queue<Node>();
                HashSet<Node> visited = new HashSet<Node>();   
                queue.Enqueue(checker);
                do
                {
                    Node node= queue.Dequeue();
                    if(!visited.Contains(node))
                        visited.Add(node);
                    foreach (var neighbor in node.neighbors)
                    {
                        if (neighbor.Value == 0 && checker.IsKing&&Node.IsSameDiagonal(checker,neighbor))
                        {
                            if (current_step == -1)
                            {
                                neighbor.Value = -2;
                            }
                            else
                            {
                                neighbor.Value = 2;
                            }
                            queue.Enqueue(neighbor);
                        }
                        else if (neighbor.Value == 0 && !checker.IsKing)
                        {
                            //если ход белых, то ходим в сторону понижения номера клетки
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
                    }

                } while (queue.Count() > 0&&checker.IsKing);
            }
        }

        static void SetStepForKill(Node checker, in int current_step)
        {
            foreach (var neighbor in checker.neighbors)
            {
                if (neighbor.Value != current_step * -1)
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
                        SetStepForKill(neighbor_opponent, in current_step);//вдруг там мульти kill
                    }
                }
            }
        }




    }
}
