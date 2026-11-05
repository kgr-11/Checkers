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
        public List<Node> neighbors;
        public int Value { get; set; }
        public int Number { get; init; }
        public Node(int value, int number)
        {
            Value = value;
            neighbors = new List<Node>();
            Number = number;
        }
        public static bool IsSameDiagonal(Node node1, Node node2)
        {
            return Math.Abs(node1.Number - node2.Number) % 7 == 0|| Math.Abs(node1.Number - node2.Number) % 9 == 0;
        }
        public void IndexInMtx(out int i,out int j)
        {
            i = (Number-1) / 8;
            j = (Number-1) - i * 8;
        }
    }
}
