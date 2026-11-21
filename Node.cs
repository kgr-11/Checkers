using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Node
    {
        //список соседних узлов текущего узла
        public List<Node> neighbors;
        //значение узла
        public int Value { get; set; }
        //номер узла
        public int Number { get; init; }
        public bool IsKing 
        {
            get {
                if (Value == 2 || Value == -2)
                    return true;
                return false;
            }
        }
        public Node(int value, int number)
        {
            Value = value;
            neighbors = new List<Node>();
            Number = number;
        }
        //функция определяет, находятся ли два узла на одной диагонали
        public static bool IsSameDiagonal(Node node1, Node node2)
        {
            return Math.Abs(node1.Number - node2.Number) % 7 == 0|| Math.Abs(node1.Number - node2.Number) % 9 == 0;
        }
        //по номеру можно определить, что за клетка на доске данный узел
        public void IndexInMtx(out int i,out int j)
        {
            i = (Number-1) / 8;
            j = (Number-1) - i * 8;
        }
    }
}
