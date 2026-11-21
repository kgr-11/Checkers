using Checkers.GA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Checkers
{

    public static class Checker
    {
        //функция проверки: можно ли ходить выбранной шашкой
        public static bool IsPossibleMakeMoveChecker(Node checker, Node[,] game_place, in CurrentStep current_step)
        {
            bool res = false;
            // 1. Проверяем саму шашку -просто шашку/дамка
            // а. смотрим можно ли ей сделать тихий ход - без рубки
            // б. можно ли ей сделать рубку -  если да, значит этой шашкой можно ходить без дальнейших проверок
            foreach (var neighbor in checker.neighbors)
            {
                //если это дамка и если пустая клетка - тихий ход
                //без разницы в каком направлении
                if (neighbor.Value == 0 && checker.IsKing)
                {
                    res = true;
                    //проверка рубки дамкой 
                    if (Check_Kill(checker, current_step))
                        return true;
                }
                // если не дамка, то важно направление
                else if (!checker.IsKing && neighbor.Value == 0)
                {
                    //если ход белых, то ходим в сторону понижения номера клетки
                    if (current_step == CurrentStep.White && neighbor.Number < checker.Number
                        //если ход черных, то ходим в сторону повышения номера клетки
                        || current_step == CurrentStep.Black && neighbor.Number > checker.Number)
                    {
                        res = true;
                    }
                }
                //если на соседней клетке соперник - проверяем можно ли срубить
                else if ((current_step == CurrentStep.White && (neighbor.Value == -1 || neighbor.Value == -2))
                    || (current_step == CurrentStep.Black && (neighbor.Value == 1 || neighbor.Value == 2)))
                {
                    //проверка рубки выбранной
                    if (Check_Kill(checker, current_step))
                        return true;
                }
            }

            // 2. если у выбранной шашки нет рубки
            // надо проверить остальных этого же цвета, ведь если у них есть рубка, то данной шашкой ходить нельзя
            for (int i = 0; i < game_place.GetLength(0); i++)
            {
                for (int j = 0; j < game_place.GetLength(1); j++)
                {
                    if ((current_step == CurrentStep.White && (game_place[i, j].Value == 1 || game_place[i, j].Value == 2))
                    || (current_step == CurrentStep.Black && (game_place[i, j].Value == -1 || game_place[i, j].Value == -2)))
                    {
                        //здесь проверяем шашку, можно ли ей срубить
                        if (Check_Kill(game_place[i, j], in current_step))
                            return false;
                    }
                }
            }
            return res;
        }

        //функция проверки: можно ли срубить выбранной шашкой - проверяем с помощью обхода графа в ширину
        // ! не важно мульти килл или просто рубка
        private static bool Check_Kill(Node checker, in CurrentStep current_step)
        {
            if (!checker.IsKing)
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
                        //если одна диагональ + клетка свободна + мы в ней не были -> идем как бы в нее, если мы дамка
                        // если дамка, то все ок, просто раньше выйдем из цикла
                        if (Node.IsSameDiagonal(checker, neighbor) && neighbor.Value == 0 && !queue.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);
                        }
                        //если одна диагональ и в соседней клетке соперник -> проверка: можно ли срубить
                        else if (Node.IsSameDiagonal(checker, neighbor) &&
                            ((current_step == CurrentStep.White && (neighbor.Value == -1 || neighbor.Value == -2))
                            || (current_step == CurrentStep.Black && (neighbor.Value == 1 || neighbor.Value == 2))))
                        {
                            //перебираем соседей соперника на поиск за ним пустой клетки, чтобы туда встать = как бу срубить
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
            else
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
                    //перебор соседей
                    foreach (var neighbor in node.neighbors)
                    {
                        //ищем соседа на одной диагонали + другого цвета(соперника) + ту строну не ходили(т.е. сосед не в списке посещенных)- пробуем срубить
                        if (Node.IsSameDiagonal(checker, neighbor) && !visited.Contains(neighbor) &&
                            ((current_step == CurrentStep.White && (neighbor.Value == -1 || neighbor.Value == -2))
                            || (current_step == CurrentStep.Black && (neighbor.Value == 1 || neighbor.Value == 2))))
                        {
                            //перебираем соседей соперника и хотим найти пустую клетку, чтобы туда встать 
                            foreach (var neighbor_opp in neighbor.neighbors)
                            {
                                if (neighbor_opp == node)
                                    continue;
                                //если клетка пустая+ одна диагональ с текущим node`м -> рубим соседа
                                if (neighbor_opp.Value == 0 && Node.IsSameDiagonal(node, neighbor_opp))
                                {
                                    return true;
                                }
                            }
                        }
                        //если выше условие не работает, то если мы дамка, просто идем дальше в разрешённые стороны
                        //если соседняя клетка пустая+соседняя клетка не посещалась одна диагональ соседа с хз с кем
                        else if (neighbor.Value == 0 && !visited.Contains(neighbor) && Node.IsSameDiagonal(neighbor, checker))
                        {
                            queue.Enqueue(neighbor);//встаем в пустую клетку
                        }
                    }
                    } while (queue.Count > 0 && checker.IsKing);
                return false;
            }
        }


        //функция установки допустимых ходов для выбранной шашки
        //функция срабатывает после всех проверок
        //устанавливает временные значения как возможные ходы, если эта функция вызвалась, значит они точно есть
        public static void SetPossibleStep(Node checker, Node[,] game_place, in CurrentStep current_step)
        {
            // 1. устанавливаем сначала ходы для сруба(ограничение такое,
            // что ставим дамку пока что сразу за последней срубленной),
            // если рубить нельзя,
            // то тогда только устанавливаем варианты тихих ходов
            if (Check_Kill(checker, in current_step))
            {

                //очередь для обхода в ширину
                Queue<Node> queue = new Queue<Node>();
                queue.Enqueue(checker);
                //список посещенных
                HashSet<Node> visited = new HashSet<Node>();
                if (checker.IsKing)
                {
                    //изначально запоминаем все дамки, чтобы потом не удалить
                    HashSet<Node> Kings = new HashSet<Node>();
                    for (int i = 0; i < game_place.GetLength(0); i++)
                    {
                        for (int j = 0; j < game_place.GetLength(1); j++)
                        {
                            if (game_place[i, j].IsKing)
                            {
                                Kings.Add(game_place[i, j]);
                            }
                        }
                    }

                    //словарь предков
                    Dictionary<Node, List<Node>> parents = new Dictionary<Node, List<Node>>();
                    //key-потомок
                    //value-список предков, к узлу может везти не один предок, т.е. запоминаем через кого мы пришли в этот узел
                    parents[checker] = null;
                    while (queue.Count > 0)
                    {
                        Node node = queue.Dequeue();
                        if (!visited.Contains(node))
                            visited.Add(node);

                        // Ебаннутый костыль
                        Node parentDiag = node;
                        if (!node.IsKing)//заходим, если текущая не дамка
                        {
                            parentDiag = parents[node][parents[node].Count - 1];//последего предка достаем
                            //из какого узла диагонали, на которой стоит Node, пришли - надо для дамки эту хуйню
                            while (!parentDiag.IsKing)
                            {
                                parentDiag = parents[parentDiag][parents[node].Count - 1];
                            }
                        }

                        //перебор соседей
                        foreach (var neighbor in node.neighbors)
                        {
                            //ищем соседа на одной диагонали + другого цвета(соперника) + ту строну не ходили(т.е. сосед не в списке посещенных)- пробуем срубить
                            if (Node.IsSameDiagonal(parentDiag, neighbor) && !visited.Contains(neighbor) &&
                                ((current_step == CurrentStep.White && (neighbor.Value == -1 || neighbor.Value == -2))
                                || (current_step == CurrentStep.Black && (neighbor.Value == 1 || neighbor.Value == 2))))
                            {
                                //перебираем соседей соперника и хотим найти пустую клетку, чтобы туда встать 
                                foreach (var neighbor_opp in neighbor.neighbors)
                                {
                                    if (neighbor_opp == node)
                                        continue;
                                    //если клетка пустая+ одна диагональ с текущим node`м -> рубим соседа
                                    if (neighbor_opp.Value == 0 && Node.IsSameDiagonal(node, neighbor_opp))
                                    {
                                        //добавляем соседа в список, чтобы случайно не пойти в то направление 
                                        visited.Add(neighbor);
                                        //запоминаем предков откуда пришли
                                        if (parents.ContainsKey(neighbor))
                                        {
                                            parents[neighbor].Add(node);  //в соседа из текущего node главного цикла
                                        }
                                        else
                                        {
                                            parents[neighbor] = new List<Node> { node };
                                        }
                                        if (parents.ContainsKey(neighbor_opp))
                                        {
                                            parents[neighbor_opp].Add(neighbor);  //в соседа из текущего node главного цикла
                                        }
                                        else
                                        {
                                            parents[neighbor_opp] = new List<Node> { neighbor };
                                        }
                                        queue.Enqueue(neighbor_opp);//встаем в пустую клетку
                                        neighbor_opp.Value = current_step == CurrentStep.White ? 2 : -2;//типа дамка сюда встала
                                    }
                                }
                            }
                            //если выше условие не работает, то если мы дамка, просто идем дальше в разрешённые стороны
                            //если соседняя клетка пустая+соседняя клетка не посещалась одна диагональ соседа с хз с кем
                            else if (neighbor.Value == 0 && !visited.Contains(neighbor) && Node.IsSameDiagonal(neighbor, parentDiag))
                            {
                                if (parents.ContainsKey(neighbor))
                                {
                                    parents[neighbor].Add(node);  //в соседа из текущего node главного цикла
                                }
                                else
                                {
                                    parents[neighbor] = new List<Node> { node };
                                }
                                queue.Enqueue(neighbor);//встаем в пустую клетку
                            }
                        }

                    }
                    //надо что-то придумать чтобы не удалить другие дамки
                    for (int i = 0; i < game_place.GetLength(0); i++)
                    {
                        for (int j = 0; j < game_place.GetLength(1); j++)
                        {
                            //если обнаружили дамку не исходную
                            if (game_place[i, j] != checker && game_place[i, j].IsKing && !Kings.Contains(game_place[i, j]))
                            {
                                bool flag = true;
                                foreach (var pair in parents)
                                {
                                    if (pair.Value is null)
                                        continue;
                                    foreach (var node in pair.Value)
                                    {
                                        if (game_place[i, j] == node)//если является предком-> неконечный ход
                                        {
                                            flag = false;
                                            goto metka;
                                        }
                                    }
                                }
                            metka:
                                if (flag)//если является предком-> неконечный ход
                                    game_place[i, j].Value = 100;
                                else
                                    game_place[i, j].Value = 0;//возвращаем на пустую клетку

                            }
                        }
                    }
                }
                else
                {
                    //словарь предков
                    Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
                    //key-потомок
                    //value-список предков, к узлу может везти не один предок, т.е. запоминаем через кого мы пришли в этот узел
                    parents[checker] = null;
                    while (queue.Count > 0)
                    {
                        Node node = queue.Dequeue();
                        if (!visited.Contains(node))
                            visited.Add(node);

                        //перебор соседей
                        foreach (var neighbor in node.neighbors)
                        {
                            //ищем соседа на одной диагонали + другого цвета(соперника) + ту строну не ходили(т.е. сосед не в списке посещенных)- пробуем срубить
                            if (Node.IsSameDiagonal(node, neighbor) && !visited.Contains(neighbor) &&
                                ((current_step == CurrentStep.White && (neighbor.Value == -1 || neighbor.Value == -2))
                                || (current_step == CurrentStep.Black && (neighbor.Value == 1 || neighbor.Value == 2))))
                            {
                                //перебираем соседей соперника и хотим найти пустую клетку, чтобы туда встать 
                                foreach (var neighbor_opp in neighbor.neighbors)
                                {
                                    if (neighbor_opp == node)
                                        continue;
                                    //если клетка пустая+ одна диагональ с текущим node`м -> рубим соседа
                                    if (neighbor_opp.Value == 0 && Node.IsSameDiagonal(node, neighbor_opp))
                                    {
                                        //добавляем соседа в список, чтобы случайно не пойти в то направление 
                                        visited.Add(neighbor);
                                        //запоминаем предков откуда пришли
                                        parents[neighbor] = node;  //в соседа из текущего node главного цикла
                                        parents[neighbor_opp] = neighbor;  //в пустую из соперника
                                        queue.Enqueue(neighbor_opp);//встаем в пустую клетку
                                    }
                                }
                            }
                        }

                    }
                    //на этом шаге знаем все возможные пути
                    //нужно пометить ту клетку как ход, которая является ничейным предком
                    foreach (var pair1 in parents)
                    {
                        if (!parents.ContainsValue(pair1.Key))//если потомок не является ничейным предком
                        {
                            pair1.Key.Value = 100;//конечный возможный ход
                        }
                    }
                }

            }
            // 2. рубить нечего, устанавливаем значения для тихого хода - ход по прямой - с помощью обхода в ширину
            // для обычной шашки, это ход на следующую клетку
            // для дамки пока не упрется куда-нибудь
            else
            {
                Queue<Node> queue = new Queue<Node>();
                HashSet<Node> visited = new HashSet<Node>();
                queue.Enqueue(checker);
                do
                {
                    Node node = queue.Dequeue();
                    if (!visited.Contains(node))
                        visited.Add(node);
                    //перебор соседей
                    foreach (var neighbor in node.neighbors)
                    {
                        //если клетка пустая и на одной диагонали с выбранной ДАМКОЙ, то устанавливаем временные значения, что туда можно ходить
                        if (neighbor.Value == 0 && checker.IsKing && Node.IsSameDiagonal(checker, neighbor))
                        {
                            neighbor.Value = 100;
                            queue.Enqueue(neighbor);
                        }
                        //если соседняя клетка для выбранной ШАШКИ пустая, то проверяем можно ли туда ходить, т.к. ходить нельзя
                        // Не проверяем на одной ли диагонали, т.к. при первой итерации все соседи на одной диагонали для шашки,
                        // а после итерации цикл просто прекратится
                        else if (neighbor.Value == 0 && !checker.IsKing)
                        {
                            //если ход белых, то ходим в сторону понижения номера клетки
                            if (current_step == CurrentStep.White && neighbor.Number < checker.Number
                                //если ход черных, то ходим в сторону повышения номера клетки
                                || current_step == CurrentStep.Black && neighbor.Number > checker.Number)
                            {
                                neighbor.Value = 100;
                            }
                        }
                    }

                } while (queue.Count() > 0 && checker.IsKing);
            }
        }



    }
}